using System;
using System.IO;
using System.Reflection;
using System.Security.Policy;

namespace Scheduler.Core.Loader
{
    internal static class AppDomainManager
    {
        private static string RootPath => Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);

        internal static AppDomain CreateDomain(string name, string applicationBase = null)
        {
            var appDomainSetup = new AppDomainSetup()
            {
                ApplicationBase = (string.IsNullOrEmpty(applicationBase)) ? RootPath : applicationBase
            };

            Evidence adevidence = AppDomain.CurrentDomain.Evidence;

            return AppDomain.CreateDomain(name, adevidence, appDomainSetup);
        }

        internal static void ReleaseDomain(AppDomain domain)
        {
            if(domain != null)
            {
                AppDomain.Unload(domain);
            }
        }

        internal static void SetResolverOptions(string assemblyFolder, string assemblyName = "*", string assemblyExtension = "dll")
        {
            AppDomain.CurrentDomain.AssemblyResolve += (s, e) => NewDomain_AssemblyResolve(s, e, assemblyFolder, assemblyName, assemblyExtension);
        }

        private static Assembly NewDomain_AssemblyResolve(object sender, ResolveEventArgs args, string asmFolder, string asmName, string asmExt)

        {
            string[] files = Directory.GetFiles(asmFolder, $"{asmName}.{asmExt}", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                AssemblyName aname = AssemblyName.GetAssemblyName(file);
                if (aname.FullName.Equals(args.Name))
                {
                    return Assembly.LoadFile(file);
                }
            }

            return null;
        }
    }
}
