using System.Web.Mvc;

using Scheduler.Core.Dependencies;
using Scheduler.Core.Dependencies.Configurations;

using Unity.Mvc5;

namespace Scheduler.WebConsole.Utilities.Dependencies
{
    public class MvcDependencyConfigurations : DependencyConfiguration
    {
        public override void Configure()
        {
            DependencyResolver.SetResolver(new UnityDependencyResolver(Container.GetInstance()));
        }
    }
}