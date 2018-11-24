using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using AutoMapper;

namespace Pactor.Infra.Crosscutting.ObjectMapper
{
    public static class ObjectMapperBootstrap
    {
        private const string DefaultAssemblyPrefix = "";

        public static void Config(ContainerBuilder builder, string[] assemblyPrefixes)
        {
            var profiles = new List<Profile>();
            foreach (var assemblyPrefix in assemblyPrefixes)
            {
                if (string.IsNullOrWhiteSpace(assemblyPrefix))
                    profiles.AddRange(GetModuleTypesFromAssemblyLocation(DefaultAssemblyPrefix));
                else if (!assemblyPrefix.EndsWith("."))
                    profiles.AddRange(GetModuleTypesFromAssemblyLocation(assemblyPrefix + "."));
                else
                    profiles.AddRange(GetModuleTypesFromAssemblyLocation(assemblyPrefix));
            }
            ConfigMapping(profiles, builder);
        }

        public static void Config(ContainerBuilder builder, string assemblyPrefix)
        {
            if (string.IsNullOrWhiteSpace(assemblyPrefix))
                assemblyPrefix = DefaultAssemblyPrefix;
            else if (!assemblyPrefix.EndsWith("."))
                assemblyPrefix += ".";

            var profiles = GetModuleTypesFromAssemblyLocation(assemblyPrefix);
            ConfigMapping(profiles, builder);
        }

        private static void ConfigMapping(IEnumerable<Profile> profiles, ContainerBuilder builder)
        {
            var config = new MapperConfiguration(cfg =>
                {
                    foreach (var profile in profiles)
                    {
                        cfg.AddProfile(profile);
                    }
                });
            
            var mapper = config.CreateMapper();

            builder.RegisterInstance(config)
                   .As<IConfigurationProvider>()
                   .SingleInstance();

            builder.RegisterInstance(mapper)
                   .As<IMapper>()
                   .SingleInstance();
        }

        private static IEnumerable<Profile> GetModuleTypesFromAssemblyLocation(string assemblyPrefix)
        {
            var profilesTypes = new List<Profile>();
            var files = GetProfilesAssemblies(assemblyPrefix);

            foreach (var dllFilePathAndName in files)
            {
                try
                {
                    var assembly = LoadProfileModuleAssembly(dllFilePathAndName);
                    var profileTypesInAssembly = assembly.GetTypes()
                                                         .Where(t => typeof(Profile).IsAssignableFrom(t))
                                                         .ToArray();

                    profilesTypes.AddRange(profileTypesInAssembly.Select(profileType => (Profile)Activator.CreateInstance(profileType)));
                }
                catch (Exception e)
                {
                    if (e is FileLoadException || e is FileNotFoundException || e is BadImageFormatException)
                        continue;

                    throw;
                }
            }

            return profilesTypes.ToArray();
        }

        private static string[] GetProfilesAssemblies(string assemblyPrefix)
        {
            var ioCModulesAssemblies = GetHostPath();
            var assemblies = new List<string>(ioCModulesAssemblies == null
                                              ? new string[0]
                                              : Directory.GetFiles(ioCModulesAssemblies, assemblyPrefix + "*.dll")) { Assembly.GetEntryAssembly().Location };
            return assemblies.ToArray();
        }

        private static string GetHostPath()
        {
            var rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return rootPath;
        }

        private static Assembly LoadProfileModuleAssembly(string dllFilePathAndName)
        {
            return Assembly.LoadFrom(dllFilePathAndName);
        }
    }
}
