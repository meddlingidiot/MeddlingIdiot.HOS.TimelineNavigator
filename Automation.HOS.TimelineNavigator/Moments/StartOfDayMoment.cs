namespace Automation.HOS.TimelineNavigator.Moments
{
    [Serializable]
    public class StartOfDayMoment : Moment
    {
        public StartOfDayMoment() {  }

        public StartOfDayMoment(DateTime timestamp, string? driverIdNumber = null, string? truckNumber = null)
        {
            Timestamp = timestamp;
            DriverIdNumber = driverIdNumber;
            TruckNumber = truckNumber;
        }

        public override object Clone()
        {
            var src = this;
            StartOfDayMoment? dest = (StartOfDayMoment?)Activator.CreateInstance(src.GetType());
            dest = (StartOfDayMoment)PopulateClone(src, dest!);
            return dest;
        }
    }
}
