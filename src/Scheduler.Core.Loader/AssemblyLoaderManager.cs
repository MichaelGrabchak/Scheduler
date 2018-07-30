using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Scheduler.Core.Dependencies;
using Scheduler.Logging;
using Scheduler.Logging.Loggers;

namespace Scheduler.Core.Loader
{
    public class AssemblyLoaderManager
    {
        private const string SchedulerAssembliesPatternName = "Scheduler.*";

        private static readonly ILogger Logger = LogManager.GetLogger();

        private static string RootPath => Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);

        private static readonly List<Type> ExtractedTypes;

        static AssemblyLoaderManager()
        {
            ExtractedTypes = new List<Type>();
        }

        public static void LoadAssemblies(string asmFolder, string asmName = "*", bool cleanLoad = false)
        {
            if(cleanLoad)
            {
                ExtractedTypes.Clear();

                // extracting current assembly types
                var assemblyName = SchedulerAssembliesPatternName; 
                ExtractedTypes.AddRange(ExtractTypes(RootPath, assemblyName));
            }

            // loading external assemblies and extract types from them
            ExtractedTypes.AddRange(ExtractTypes(asmFolder, asmName));
        }

        public static Type GetType(string typeName)
        {
            try
            {
                if (!string.IsNullOrEmpty(typeName))
                {
                    // Returns the assembly of the type by enumerating loaded assemblies
                    return ExtractedTypes.FirstOrDefault(type => type.FullName == typeName);
                }
            }
            catch (Exception ex)
            {
                // ignoring exception
                Logger.Warn($"An unexpected error has occurred during getting the type '{typeName}'. Exception:{Environment.NewLine}{ex}");
            }

            return null;
        }

        public static IList<T> GetImplementorsOf<T>()
        {
            var implementors = new List<T>();

            try
            {
                ExtractedTypes
                    .Where(type => type != typeof(T) && typeof(T).IsAssignableFrom(type) && !type.IsAbstract)
                    .ToList()
                    .ForEach(type => implementors.Add(Container.Resolve<T>(type)));
            }
            catch (Exception ex)
            {
                // ignoring exception
                Logger.Warn($"An unexpected error has occurred during getting the implementor of '{typeof(T)}'. Exception:{Environment.NewLine}{ex}");
            }

            return implementors;
        }

        private static IList<Type> ExtractTypes(string assemblyFolder, string assemblyName = "*", string extension = "dll")
        {
            var types = new List<Type>();

            var directory = new DirectoryInfo(assemblyFolder);

            var files = directory.GetFiles($"{assemblyName}.{extension}", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                try
                {
                    // creating a new application domain
                    var appDomain = AppDomainManager.CreateDomain(file.FullName, RootPath);
                    AppDomainManager.SetResolverOptions(assemblyFolder, assemblyName, extension);

                    // building a proxy object for newly created application domain
                    var asmLoaderProxy = ProxyDomainFactory.BuildProxy(appDomain);

                    // getting assembly object via proxy domain
                    var assembly = asmLoaderProxy.GetAssembly(file.FullName);

                    // populating types collection with the types from assembly
                    types.AddRange(assembly.GetTypes());

                    // unload the domain
                    AppDomainManager.ReleaseDomain(appDomain);
                }
                catch (Exception ex)
                {
                    // skip error
                    // continue extracting types from the assemblies
                    Logger.Warn($"An unexpected error has occurred during extracting of the types from assembly '{file.FullName}'. Exception:{Environment.NewLine}{ex}");
                }
            }

            return types;
        }
    }
}
