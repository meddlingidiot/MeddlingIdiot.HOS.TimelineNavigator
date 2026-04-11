namespace Automation.HOS.TimelineNavigator.Moments
{
    [Serializable]
    public sealed class DutyStatusChangeMoment : Moment
    {
        public DutyStatus CurrentDutyStatus { get; private set; }
        public string? Comment { get; private set; }

        public DutyStatusChangeMoment()
        {
            CurrentDutyStatus = DutyStatus.Unknown;
        }

        public DutyStatusChangeMoment(DateTime timestamp, DutyStatus dutyStatus, string? comment = null, string? driverIdNumber = null,
            string? truckNumber = null)
        {
            Timestamp = timestamp;
            CurrentDutyStatus = dutyStatus;
            Comment = comment;
            DriverIdNumber = driverIdNumber;
            TruckNumber = truckNumber;
        }

        public override object Clone()
        {
            var src = this;
            DutyStatusChangeMoment? dest = (DutyStatusChangeMoment?)Activator.CreateInstance(src.GetType());
            dest = (DutyStatusChangeMoment)PopulateClone(src, dest!);
            dest.CurrentDutyStatus = src.CurrentDutyStatus;
            dest.Comment = src.Comment;
            return dest;
        }

        public override string ToString()
        {
            return $"TS:{Timestamp} DS:{CurrentDutyStatus} C:{Comment}";
        }
    }

}