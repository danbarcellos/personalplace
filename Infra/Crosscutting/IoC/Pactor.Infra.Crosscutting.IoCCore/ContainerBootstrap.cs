using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;

namespace Pactor.Infra.Crosscutting.IoC.Core
{
    public class ContainerBootstrap
    {
        private Autofac.IContainer _container;

        public ContainerBuilder Config(ContainerBuilder builder = null)
        {
            builder = builder ?? new ContainerBuilder();
            builder.RegisterBuildCallback(container => { _container = container; });
            LoadModules(builder);
            InternalRegisters(builder);
            return builder;
        }

        private void LoadModules(ContainerBuilder builder)
        {
            var moduleTypes = GetModuleTypesFromAssemblyLocation();

            foreach (var moduleType in moduleTypes)
            {
                builder.RegisterModule(Activator.CreateInstance(moduleType) as IModule);
            }
        }

        private IEnumerable<Type> GetModuleTypesFromAssemblyLocation()
        {
            var moduleTypes = new List<Type>();

            var files = GetIoCModulesAssemblies();

            foreach (var dllFilePathAndName in files)
            {
                try
                {
                    var assembly = LoadIoCModuleAssembly(dllFilePathAndName);
                    var modulesInAssembly = assembly.GetTypes()
                                          .Where(t => typeof(IModule).IsAssignableFrom(t))
                                          .ToArray();

                    moduleTypes.AddRange(modulesInAssembly);
                }
                catch (Exception e)
                {
                    if (e is FileLoadException || e is FileNotFoundException || e is BadImageFormatException)
                        continue;

                    throw;
                }
            }

            return moduleTypes.ToArray();
        }

        private void InternalRegisters(ContainerBuilder builder)
        {
            builder.Register(c => new AutofacContainer(_container))
                   .Named<IContainer>(ContainerTag.Root)
                   .SingleInstance();

            builder.Register(c => new AutofacContainer(((Autofac.Core.Resolving.IInstanceLookup)c).ActivationScope))
                   .As<IContainer>()
                   .InstancePerLifetimeScope();

            builder.Register(c => new AutofacServiceLocator(((Autofac.Core.Resolving.IInstanceLookup)c).ActivationScope))
                   .As<IServiceLocator>()
                   .As<IServiceProvider>()
                   .InstancePerLifetimeScope();
        }

        protected virtual Assembly LoadIoCModuleAssembly(string dllFilePathAndName)
        {
            return Assembly.LoadFile(dllFilePathAndName);
        }

        protected virtual string[] GetIoCModulesAssemblies()
        {
            var ioCModulesAssemblies = GetHostPath();
            return ioCModulesAssemblies == null ? new string[0]
                                                : Directory.GetFiles(ioCModulesAssemblies, "*.IoCM.dll");
        }

        internal virtual string GetHostPath()
        {
            var rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return rootPath;
        }
    }
}