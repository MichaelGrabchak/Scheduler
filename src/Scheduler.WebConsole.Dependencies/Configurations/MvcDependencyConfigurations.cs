using System.Web.Mvc;

using Scheduler.Core.Dependencies;
using Scheduler.Core.Dependencies.Configurations;

using Unity.Mvc5;

namespace Scheduler.WebConsole.Dependencies.Configurations
{
    public class MvcDependencyConfigurations : IDependencyConfiguration
    {
        public void Configure()
        {
            DependencyResolver.SetResolver(new UnityDependencyResolver(Container.GetInstance()));
        }
    }
}