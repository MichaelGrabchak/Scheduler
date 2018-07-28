using System;

using Microsoft.AspNet.SignalR.Hubs;

using Scheduler.Core.Dependencies;

namespace Scheduler.WebConsole.Utilities.Activators
{
    public class HubActivator : IHubActivator
    {
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

            object hub = Container.Resolve<IHub>(descriptor.HubType) ?? Activator.CreateInstance(descriptor.HubType);

            return hub as IHub;
        }
    }
}
