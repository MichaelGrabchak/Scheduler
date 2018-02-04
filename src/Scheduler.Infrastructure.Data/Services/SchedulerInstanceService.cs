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
            var instanceSettings = _dbContext.SchedulerInstanceSettings.GetById(new { InstanceId = _schedulerContext.InstanceId });

            if(instanceSettings == null)
            {
                throw new ArgumentException("Couldn't find instance settings");
            }

            return new InstanceSettings {
                InstanceId = _schedulerContext.InstanceId,

                IsImmediateEngineStartEnabled = instanceSettings.IsImmediateEngineStartEnabled,
                IsJobsDirectoryTrackingEnabled = instanceSettings.IsJobsDirectoryTrackingEnabled
            };
        }
    }
}
