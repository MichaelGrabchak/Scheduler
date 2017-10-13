using System;

namespace Scheduler.Core.Helpers
{
    public static class ProxyDomainFactory
    {
        public static ProxyDomain BuildProxy(AppDomain domain)
        {
            var type = typeof(ProxyDomain);

            return (ProxyDomain)domain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);
        }
    }
}
