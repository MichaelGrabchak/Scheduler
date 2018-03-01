using System;

namespace Scheduler.Domain.Data.Dto
{
    public class JobDetail
    {
        public int Id { get; set; }
        public string JobName { get; set; }
        public string JobGroup { get; set; }
        public string JobDescription { get; set; }
        public string JobSchedule { get; set; }
        public DateTimeOffset? JobLastRunTime { get; set; }
        public DateTimeOffset? JobNextRunTime { get; set; }
        public byte StatusId { get; set; }
        public Guid InstanceId { get; set; }

        #region Property Specified

        private bool _isDescSpecified;
        public bool JobDescriptionSpecified
        {
            get
            {
                return (_isDescSpecified || !string.IsNullOrEmpty(JobDescription));
            }
            set
            {
                _isDescSpecified = value;
            }
        }

        private bool _isScheduleSpecified;
        public bool JobScheduleSpecified
        {
            get
            {
                return (_isScheduleSpecified || !string.IsNullOrEmpty(JobSchedule));
            }
            set
            {
                _isScheduleSpecified = value;
            }
        }

        private bool _isNextRunSpecified;
        public bool JobNextRunTimeSpecified
        {
            get
            {
                return (_isNextRunSpecified || JobNextRunTime.HasValue);
            }
            set
            {
                _isNextRunSpecified = value;
            }
        }

        private bool _isLastRunSpecified;
        public bool JobLastRunTimeSpecified
        {
            get
            {
                return (_isLastRunSpecified || JobLastRunTime.HasValue);
            }
            set
            {
                _isLastRunSpecified = value;
            }
        }

        #endregion
    }
}
