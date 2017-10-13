using System;
using System.Reflection;

namespace Scheduler.Core.Helpers
{
    public class ProxyDomain : MarshalByRefObject
    {
        public Assembly GetAssembly(string path)
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
