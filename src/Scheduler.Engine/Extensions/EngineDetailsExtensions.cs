using System;
using System.Globalization;

using Scheduler.Domain.Entities;

namespace Scheduler.Engine.Extensions
{
    public static class EngineDetailsExtensions
    {
        public static EngineInfo ToEngineInfo(this EngineDetails engineDetails)
        {
            return new EngineInfo
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
