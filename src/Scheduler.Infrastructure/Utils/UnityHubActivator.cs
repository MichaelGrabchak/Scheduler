using System;

using Microsoft.Practices.Unity;
using Microsoft.AspNet.SignalR.Hubs;

namespace Scheduler.Infrastructure.Utils
{
    public class UnityHubActivator : IHubActivator
    {
        private readonly IUnityContainer _container;

        public UnityHubActivator(IUnityContainer container)
        {
            _container = container;
        }

        public IHub Create(HubDescriptor descriptor)
        {
            if (descriptor == null)
            {
                throw new ArgumentNullException(nameof(descriptor));
            }

            if (descriptor.HubType == null)
            {
                return null;
            }

            object hub = _container.Resolve(descriptor.HubType) ?? Activator.CreateInstance(descriptor.HubType);
            return hub as IHub;
        }
    }
}
