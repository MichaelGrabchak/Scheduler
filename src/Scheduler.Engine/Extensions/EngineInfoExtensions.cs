using System;
using System.Globalization;

using Scheduler.Domain.Entities;

namespace Scheduler.Engine.Extensions
{
    public static class EngineInfoExtensions
    {
        public static EngineDetails ToEngineDetails(this EngineInfo engineDetails)
        {
            return new Domain.Entities.EngineDetails
            {
                RunningSince = (engineDetails.StartDate != DateTimeOffset.MinValue) ? engineDetails.StartDate.ToString(CultureInfo.InvariantCulture) : null,
                Version = engineDetails.Version,
                Engine = engineDetails.Name,
                State = engineDetails.State,
                InstanceId = engineDetails.InstanceId,
                InstanceName = engineDetails.InstanceName
            };
        }
    }
}
