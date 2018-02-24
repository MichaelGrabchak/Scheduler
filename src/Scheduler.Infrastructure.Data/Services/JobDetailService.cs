using Scheduler.Domain.Data.Dto;
using Scheduler.Domain.Data.Repositories;
using Scheduler.Domain.Data.Services;

namespace Scheduler.Infrastructure.Data.Services
{
    public class JobDetailService : IJobDetailService
    {
        private readonly IJobDetailRepository _jobDetailRepository;

        public JobDetailService(IJobDetailRepository jobDetailRepository)
        {
            _jobDetailRepository = jobDetailRepository;
        }

        public JobDetail GetJobDetail(string jobName, string jobGroup)
        {
            var jobDetail = _jobDetailRepository.GetJobDetail(jobName, jobGroup);

            return jobDetail != null
                ? new JobDetail {
                    Id = jobDetail.Id,
                    InstanceId = jobDetail.InstanceId,
                    JobName = jobDetail.JobName,
                    JobGroup = jobDetail.JobGroup,
                    JobDescription = jobDetail.JobDescription,
                    JobSchedule = jobDetail.JobSchedule,
                    JobLastRunTime = jobDetail.JobLastRunTime,
                    JobNextRunTime = jobDetail.JobNextRunTime,
                    StatusId = jobDetail.StatusId
                }
                : null;
        }

        public void UpdateJobDetail(JobDetail jobDetail, bool updateChangedOnly = false)
        {
            if(jobDetail != null)
            {
                var entity = _jobDetailRepository.GetJobDetail(jobDetail.JobName, jobDetail.JobGroup);

                if(entity != null)
                {
                    if (updateChangedOnly)
                    {
                        if (jobDetail.JobDescriptionSpecified)
                        {
                            entity.JobDescription = jobDetail.JobDescription;
                        }

                        if (jobDetail.JobScheduleSpecified)
                        {
                            entity.JobSchedule = jobDetail.JobSchedule;
                        }

                        if (jobDetail.JobNextRunTimeSpecified)
                        {
                            entity.JobNextRunTime = jobDetail.JobNextRunTime;
                        }

                        if (jobDetail.JobLastRunTimeSpecified)
                        {
                            entity.JobLastRunTime = jobDetail.JobLastRunTime;
                        }

                        if (jobDetail.StatusId > 0)
                        {
                            entity.StatusId = jobDetail.StatusId;
                        }
                    }
                    else
                    {
                        entity.JobDescription = jobDetail.JobDescription;
                        entity.JobSchedule = jobDetail.JobSchedule;
                        entity.JobNextRunTime = jobDetail.JobNextRunTime;
                        entity.StatusId = jobDetail.StatusId;

                        if(jobDetail.JobLastRunTime.HasValue)
                        {
                            entity.JobLastRunTime = jobDetail.JobLastRunTime;
                        }
                    }

                    _jobDetailRepository.Update(entity);

                    return;
                }

                entity = new Domain.Data.BusinessEntities.JobDetail
                {
                    Id = jobDetail.Id,
                    JobName = jobDetail.JobName,
                    JobGroup = jobDetail.JobGroup,
                    JobDescription = jobDetail.JobDescription,
                    JobSchedule = jobDetail.JobSchedule,
                    JobLastRunTime = jobDetail.JobLastRunTime,
                    JobNextRunTime = jobDetail.JobNextRunTime,
                    StatusId = jobDetail.StatusId
                };

                _jobDetailRepository.Add(entity);
            }
        }
    }
}
