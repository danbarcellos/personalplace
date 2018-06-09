using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Core;

namespace Pactor.Infra.Crosscutting.IoC.Core
{
    public class AutofacContainer : IContainer
    {
        private readonly ILifetimeScope _lifetimeScope;

        public AutofacContainer(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public object Resolve(Type type, params Parameter[] parameters)
        {
            return parameters == null || !parameters.Any() ? _lifetimeScope.Resolve(type) : _lifetimeScope.Resolve(type, GetAutofacParameters(parameters));
        }

        public T Resolve<T>(params Parameter[] parameters)
        {
            return parameters == null || !parameters.Any() ? _lifetimeScope.Resolve<T>() : _lifetimeScope.Resolve<T>(GetAutofacParameters(parameters));
        }

        public T ResolveKeyed<T>(object serviceKey)
        {
            return _lifetimeScope.ResolveKeyed<T>(serviceKey);
        }

        public T ResolveKeyed<T>(object serviceKey, params Parameter[] parameters)
        {
            return parameters == null || !parameters.Any() ? _lifetimeScope.ResolveKeyed<T>(serviceKey) : _lifetimeScope.ResolveKeyed<T>(serviceKey, GetAutofacParameters(parameters));
        }

        public T ResolveNamed<T>(string name)
        {
            return _lifetimeScope.ResolveNamed<T>(name);
        }

        public object ResolveNamed(string name, Type serviceType)
        {
            return _lifetimeScope.ResolveNamed(name, serviceType);
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            return _lifetimeScope.Resolve<IEnumerable<T>>();
        }

        public IEnumerable<object> ResolveAll(Type type)
        {
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(type);
            return _lifetimeScope.Resolve(enumerableType) as IEnumerable<object>;
        }

        public Meta<object> ResolveMeta(Type type, params Parameter[] parameters)
        {
            return parameters == null || !parameters.Any() ? GetMetadata(_lifetimeScope.Resolve(TranslateType(type)), type) : GetMetadata(_lifetimeScope.Resolve(TranslateType(type), GetAutofacParameters(parameters)), type);
        }

        public Meta<T> ResolveMeta<T>(params Parameter[] parameters)
        {
            return parameters == null || !parameters.Any() ? GetMetadata(_lifetimeScope.Resolve<Autofac.Features.Metadata.Meta<T>>()) : GetMetadata(_lifetimeScope.Resolve<Autofac.Features.Metadata.Meta<T>>(GetAutofacParameters(parameters)));
        }

        public Meta<object> ResolveNamedMeta(string name, Type serviceType)
        {
            return GetMetadata(_lifetimeScope.ResolveNamed(name, TranslateType(serviceType)), serviceType);
        }

        public Meta<T> ResolveNamedMeta<T>(string name)
        {
            return GetMetadata(_lifetimeScope.ResolveNamed<Autofac.Features.Metadata.Meta<T>>(name));
        }

        public IEnumerable<Meta<object>> ResolveAllMeta(Type type)
        {
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(typeof(Autofac.Features.Metadata.Meta<>).MakeGenericType(type));
            return ((IEnumerable<Autofac.Features.Metadata.Meta<object>>)_lifetimeScope.Resolve(enumerableType)).Select(m => GetMetadata(m, type));
        }

        public IEnumerable<Meta<T>> ResolveAllMeta<T>()
        {
            return _lifetimeScope.Resolve<IEnumerable<Autofac.Features.Metadata.Meta<T>>>().Select(GetMetadata);
        }

        public bool IsRegistered<T>()
        {
            return _lifetimeScope.IsRegistered<T>();
        }

        public bool IsRegistered(Type type)
        {
            return _lifetimeScope.IsRegistered(type);
        }

        public bool TryResolve<T>(out T instance)
        {
            return _lifetimeScope.TryResolve(out instance);
        }

        public bool TryResolve(Type serviceType, out object instance)
        {
            return _lifetimeScope.TryResolve(serviceType, out instance);
        }

        public bool TryResolveNamed(string serviceName, Type serviceType, out object instance)
        {
            return _lifetimeScope.TryResolveNamed(serviceName, serviceType, out instance);
        }

        public bool TryResolveNamed<T>(string serviceName, out object instance)
        {
            return _lifetimeScope.TryResolveNamed(typeof(T).Name, typeof(T), out instance);
        }

        public T InjectProperties<T>(T instance)
        {
            return _lifetimeScope.InjectProperties(instance);
        }

        public T InjectUnsetProperties<T>(T instance)
        {
            return _lifetimeScope.InjectUnsetProperties(instance);
        }

        public IContainer BeginLifetimeScope()
        {
            return new AutofacContainer(_lifetimeScope.BeginLifetimeScope());
        }

        public IContainer BeginLifetimeScope(object tag)
        {
            return new AutofacContainer(_lifetimeScope.BeginLifetimeScope(tag));
        }

        public IContainer BeginLifetimeScope(Action<ContainerBuilder> configurationAction)
        {
            return new AutofacContainer(_lifetimeScope.BeginLifetimeScope(configurationAction));
        }

        public IContainer BeginLifetimeScope(object tag, Action<ContainerBuilder> configurationAction)
        {
            return new AutofacContainer(_lifetimeScope.BeginLifetimeScope(tag, configurationAction));
        }

        internal IComponentRegistry GetInternalComponentRegistry()
        {
            return _lifetimeScope.ComponentRegistry;
        }

        internal ILifetimeScope GetInternalContainer()
        {
            return _lifetimeScope;
        }

        private static IEnumerable<Autofac.Core.Parameter> GetAutofacParameters(IEnumerable<Parameter> parameters)
        {
            IList<Autofac.Core.Parameter> autofacParameters = new List<Autofac.Core.Parameter>();

            foreach (var parameter in parameters)
            {
                if (parameter is TypedParameter)
                {
                    autofacParameters.Add(new Autofac.TypedParameter(((TypedParameter)parameter).Type, parameter.Value));
                }
                else if (parameter is NamedParameter)
                {
                    autofacParameters.Add(new Autofac.NamedParameter(((NamedParameter)parameter).Name, parameter.Value));
                }
            }

            return autofacParameters;
        }

        private static Meta<object> GetMetadata(object value, Type type)
        {
            var metadata = Activator.CreateInstance(typeof(Meta<>).MakeGenericType(type), Convert.ChangeType(value, type), ((Meta<object>)value).Metadata);
            return (Meta<object>)metadata;
        }

        private static Meta<T> GetMetadata<T>(Autofac.Features.Metadata.Meta<T> value)
        {
            return new Meta<T>(value.Value, value.Metadata);
        }

        private static Type TranslateType(Type type)
        {
            return typeof(Autofac.Features.Metadata.Meta<>).MakeGenericType(type.GenericTypeArguments[1]);
        }

        public override int GetHashCode()
        {
            return _lifetimeScope.GetHashCode();
        }

        public void Dispose()
        {
            _lifetimeScope.Dispose();
        }
    }
}
