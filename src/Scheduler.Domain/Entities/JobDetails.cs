namespace Scheduler.Domain.Entities
{
    public class JobDetails
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public string Description { get; set; }
        public string Schedule { get; set; }
        public string State { get; set; }
        public string ActionState { get; set; }
        public string PreviousFireTime { get; set; }
        public string NextFireTime { get; set; }

        #region Property Specified

        private bool _isDescSpecified;
        public bool DescriptionSpecified
        {
            get => _isDescSpecified || !string.IsNullOrEmpty(Description);
            set => _isDescSpecified = value;
        }

        private bool _isScheduleSpecified;
        public bool ScheduleSpecified
        {
            get => _isScheduleSpecified || !string.IsNullOrEmpty(Schedule);
            set => _isScheduleSpecified = value;
        }

        private bool _isStateSpecified;
        public bool StateSpecified
        {
            get => _isStateSpecified || !string.IsNullOrEmpty(State);
            set => _isStateSpecified = value;
        }

        private bool _isNextRunSpecified;
        public bool NextFireTimeSpecified
        {
            get => _isNextRunSpecified || !string.IsNullOrEmpty(NextFireTime);
            set => _isNextRunSpecified = value;
        }

        private bool _isLastRunSpecified;
        public bool PrevFireTimeSpecified
        {
            get => _isLastRunSpecified || !string.IsNullOrEmpty(PreviousFireTime);
            set => _isLastRunSpecified = value;
        }

        #endregion
    }
}
