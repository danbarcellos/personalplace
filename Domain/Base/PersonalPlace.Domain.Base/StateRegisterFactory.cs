using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using PersonalPlace.Domain.Base.Component;

namespace PersonalPlace.Domain.Base
{
    public static class StateRegisterFactory
    {
        private const string GeneratedAssemblyNameExtension = ".Dynamics";
        private const string StateRecordSufix = "State";
        private static readonly IDictionary<Type, Type> StateRecordEntities = new Dictionary<Type, Type>();
        private static readonly IDictionary<string, Assembly> DynamicAssemblies = new Dictionary<string, Assembly>();

        public static IDisposable GetStateRegistryFactorySession()
        {
            return new StateRegstrySession();
        }

        public static StateRegister<TTipoEstado> GetStateRecord<TTipoEstado>(this EntityWithStateRecord<TTipoEstado> entity, EntityState<TTipoEstado> state)
        {
            if (StateRecordEntities.ContainsKey(typeof(TTipoEstado)))
            {
                var registroEstadoType = StateRecordEntities[typeof(TTipoEstado)];
                var registroEstadoInstance = (StateRegister<TTipoEstado>)Activator.CreateInstance(registroEstadoType, state, entity.ScopeTag);
                SetEntity(registroEstadoInstance, entity);
                return registroEstadoInstance;
            }

            throw new ApplicationException("Compatible state record not found");
        }

        public static Type GetStateRecordType(Type tipoEstadoType)
        {
            return StateRecordEntities.ContainsKey(tipoEstadoType) ? StateRecordEntities[tipoEstadoType] : null;
        }

        public static Type[] RegisterEntitiesStateRecord(Assembly entitiesAssembly)
        {
            var entitiesAssembyName = string.Concat(entitiesAssembly.GetName().Name, GeneratedAssemblyNameExtension);
            var assemblyName = new AssemblyName { Name = entitiesAssembyName };
            var dynamicAssembly = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

            if (DynamicAssemblies.ContainsKey(dynamicAssembly.FullName))
            {
                return DynamicAssemblies[dynamicAssembly.FullName].DefinedTypes.Cast<Type>().ToArray();
            }

            var baseTypes = GetStateBaseTypesCatalog(entitiesAssembly);

            if (!baseTypes.Any())
                return new Type[] { };

            var moduleBuilder = GetModuleBuilder(dynamicAssembly, entitiesAssembyName);
            var customAttributes = GetEntityParametersBuilders(baseTypes.Keys);

            foreach (var baseType in baseTypes)
            {
                var typeBuilder = GetTypeBuilder(baseType.Key, baseType.Value, moduleBuilder, entitiesAssembyName);

                if (customAttributes.ContainsKey(baseType.Key))
                    typeBuilder.SetCustomAttribute(customAttributes[baseType.Key]);

                CreateType(typeBuilder, baseType.Key);
            }

            var assemblyTypes = dynamicAssembly.DefinedTypes.Cast<Type>().ToArray();

            foreach (var type in assemblyTypes)
            {
                // ReSharper disable once PossibleNullReferenceException
                var tipoEstadoType = type.BaseType.GetGenericArguments()[0];
                StateRecordEntities.Add(tipoEstadoType, type);
            }

            DynamicAssemblies.Add(dynamicAssembly.FullName, dynamicAssembly);
            return assemblyTypes;
        }

        private static Assembly MyAssemblyResolveHandler(object sender, ResolveEventArgs args)
        {
            return DynamicAssemblies.ContainsKey(args.Name) 
                   ? DynamicAssemblies[args.Name] 
                   : Assembly.Load(args.Name);
        }

        private static void CreateType(TypeBuilder typeBuilder, Type entityType)
        {
            try
            {
                EmitConstructors(typeBuilder);
                EmitProperty(typeBuilder, entityType);
                typeBuilder.CreateTypeInfo();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Concat("Could not CreateType ", typeBuilder.Name), ex);
            }
        }

        private static IDictionary<Type, CustomAttributeBuilder> GetEntityParametersBuilders(IEnumerable<Type> entityTypes)
        {
            var attributeCatalog = new Dictionary<Type, CustomAttributeBuilder>();
            foreach (var entity in entityTypes)
            {
                var entityParameter = entity.GetCustomAttributes(typeof(EntityParameterAttribute), inherit: false).Cast<EntityParameterAttribute>().FirstOrDefault();
                if (entityParameter != null)
                {
                    var ctorParams = new[] { typeof(Segregation), typeof(bool) };
                    var classCtorInfo = typeof(EntityParameterAttribute).GetConstructor(ctorParams);
                    // ReSharper disable once AssignNullToNotNullAttribute
                    var entityParameterAttributeBuilder = new CustomAttributeBuilder(classCtorInfo, new object[] { entityParameter.Segregation, false });
                    attributeCatalog.Add(entity, entityParameterAttributeBuilder);
                }
            }

            return attributeCatalog;
        }

        private static void EmitConstructors(TypeBuilder builder)
        {
            builder.DefineDefaultConstructor(MethodAttributes.Family | MethodAttributes.RTSpecialName);

            var constructor = GetConstructor(builder);
            var constructorParameters = GetConstructorParameters(constructor);

            // Define constructor
            var constructorBuilder = builder.DefineConstructor(MethodAttributes.Public | MethodAttributes.RTSpecialName, CallingConventions.Standard, constructorParameters);
            var generator = constructorBuilder.GetILGenerator();

            generator.Emit(OpCodes.Ldarg_0); // this

            for (short i = 1; i <= constructorParameters.Length; i++)
            {
                generator.Emit(OpCodes.Ldarg, i); // load parameter
            }

            generator.Emit(OpCodes.Call, constructor); // Call base constructor
            generator.Emit(OpCodes.Ret); // Return
        }

        private static ConstructorInfo GetConstructor(TypeBuilder builder)
        {
            var baseClassType = builder.BaseType;
            if (baseClassType == null)
                throw new ApplicationException("Generated type has no base type");

            var constructors = baseClassType
                .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);

            var constructor = constructors.FirstOrDefault(ctor => ctor.GetParameters().Any(param => param.ParameterType.IsGenericType && param.ParameterType.GetGenericTypeDefinition() == typeof(EntityState<>)));

            if (constructor == null)
                throw new Exception($"Invalid proxy base class {baseClassType.Name}. It has no constructors.");

            return constructor;
        }

        private static Type[] GetConstructorParameters(ConstructorInfo constructor)
        {
            return constructor.GetParameters()
                              .Select(parameter => parameter.ParameterType).ToArray();
        }

        private static void EmitProperty(TypeBuilder builder, Type entityType)
        {
            var propName = entityType.Name;
            var fieldName = String.Concat("_", propName.Substring(0, 1).ToLower(), propName.Substring(1));
            var customerNameBldr = builder.DefineField(fieldName,
                                                       entityType,
                                                       FieldAttributes.Private);

            var custNamePropBldr = builder.DefineProperty(propName,
                                                          PropertyAttributes.HasDefault,
                                                          entityType,
                                                          null);

            const MethodAttributes getSetAttributes = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.Virtual;

            var getPropertyBuilder =
                builder.DefineMethod(string.Concat("get_", propName),
                                     getSetAttributes,
                                     entityType,
                                     Type.EmptyTypes);

            var getGenerator = getPropertyBuilder.GetILGenerator();

            getGenerator.Emit(OpCodes.Ldarg_0);
            getGenerator.Emit(OpCodes.Ldfld, customerNameBldr);
            getGenerator.Emit(OpCodes.Ret);

            var setPropertyBuilder =
                builder.DefineMethod(string.Concat("set_", propName),
                                     getSetAttributes,
                                     null,
                                     new[] { entityType });

            var setGenerator = setPropertyBuilder.GetILGenerator();

            setGenerator.Emit(OpCodes.Ldarg_0);
            setGenerator.Emit(OpCodes.Ldarg_1);
            setGenerator.Emit(OpCodes.Stfld, customerNameBldr);
            setGenerator.Emit(OpCodes.Ret);

            custNamePropBldr.SetGetMethod(getPropertyBuilder);
            custNamePropBldr.SetSetMethod(setPropertyBuilder);
        }

        private static TypeBuilder GetTypeBuilder(Type entity, Type baseClass, ModuleBuilder moduleBuilder, string enttitiesAssembyName)
        {
            TypeBuilder builder;
            var typeName = string.Concat(enttitiesAssembyName, ".", entity.Name, StateRecordSufix);
            try
            {
                builder = moduleBuilder.DefineType(typeName, TypeAttributes.Public, baseClass);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Concat("Could not DefineType ", typeName, " : ", baseClass), ex);
            }
            if (builder == null)
            {
                throw new ApplicationException(string.Concat("Could not DefineType ", typeName, " : ", baseClass));
            }
            return builder;
        }

        private static ModuleBuilder GetModuleBuilder(AssemblyBuilder assemblyBuilder, string moduleName)
        {
            if (assemblyBuilder == null)
                throw new ArgumentNullException(nameof(assemblyBuilder));

            if (assemblyBuilder.FullName == null)
                throw new ArgumentException("Generated assembly has no name");

            ModuleBuilder moduleBuilder;

            try
            {
                moduleBuilder = assemblyBuilder.DefineDynamicModule(moduleName);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Could not generate assembly module {assemblyBuilder.FullName}", ex);
            }

            return moduleBuilder;
        }

        private static void SetEntity<TTipoEstado>(StateRegister<TTipoEstado> stateRegisterInstance, EntityWithStateRecord<TTipoEstado> entity)
        {
            var propertyInfo = stateRegisterInstance.GetType().GetProperty(entity.GetType().Name);
            propertyInfo?.SetValue(stateRegisterInstance, entity);
        }

        private static IDictionary<Type, Type> GetStateBaseTypesCatalog(Assembly entitiesAssembly)
        {
            var typesCatalog = entitiesAssembly
                .GetExportedTypes()
                .Where(type => type.BaseType != null
                               && type.BaseType.IsGenericType
                               && type.BaseType.GetGenericTypeDefinition() == typeof(EntityWithStateRecord<>)
                               && !StateRecordEntities.ContainsKey(type.BaseType.GetGenericArguments()[0]))
                // ReSharper disable once PossibleNullReferenceException
                .ToDictionary(type => type, type => typeof(StateRegister<>).MakeGenericType(type.BaseType.GetGenericArguments()[0]));

            return typesCatalog;
        }

        private class StateRegstrySession : IDisposable
        {
            public StateRegstrySession()
            {
                AppDomain.CurrentDomain.AssemblyResolve += MyAssemblyResolveHandler;
            }

            public void Dispose()
            {
                AppDomain.CurrentDomain.AssemblyResolve -= MyAssemblyResolveHandler;
            }
        }
    }
}