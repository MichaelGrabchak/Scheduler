using System;
using System.Reflection;

namespace Scheduler.Core.Loader
{
    internal class ProxyDomain : MarshalByRefObject
    {
        internal Assembly GetAssembly(string path)
        {
            try
            {
                return Assembly.LoadFile(path);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }
    }
}
