using System;

using Scheduler.Core.Context;
using Scheduler.Domain.Data;
using Scheduler.Domain.Data.Dto;
using Scheduler.Domain.Data.Services;

namespace Scheduler.Infrastructure.Data.Services
{
    public class SchedulerInstanceService : ISchedulerInstanceService
    {
        private readonly IDbContext _dbContext;
        private readonly ISchedulerContext _schedulerContext;

        public SchedulerInstanceService(IDbContext dbContext, ISchedulerContext schedulerContext)
        {
            _dbContext = dbContext;
            _schedulerContext = schedulerContext;
        }

        public InstanceSettings GetSettings()
        {
            var schedulerInstance = _dbContext.SchedulerInstances.GetInstanceDetails(_schedulerContext.InstanceId);

            if(schedulerInstance != null)
            {
                return new InstanceSettings
                {
                    InstanceId = schedulerInstance.Id.ToString(),
                    InstanceName = schedulerInstance.InstanceName,
                    IsImmediateEngineStartEnabled = schedulerInstance.IsImmediateEngineStartEnabled,
                    IsJobsDirectoryTrackingEnabled = schedulerInstance.IsJobsDirectoryTrackingEnabled
                };
            }

            return new InstanceSettings
            {
                InstanceId = _schedulerContext.InstanceId,
                InstanceName = "<unregistered>"
            };
        }
    }
}
