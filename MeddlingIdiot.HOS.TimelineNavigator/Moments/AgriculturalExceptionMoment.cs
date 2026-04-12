namespace MeddlingIdiot.HOS.TimelineNavigator.Moments
{
    [Serializable]
    public sealed class AgriculturalExceptionMoment : Moment
    {
        public bool IsEnabled { get; private set; }

        public AgriculturalExceptionMoment()
        {
            IsEnabled = false;
        }

        public AgriculturalExceptionMoment(DateTime timestamp,
            bool isEnabled = false,
            string? driverIdNumber = null,
            string? truckNumber = null)
        {
            Timestamp = timestamp;
            IsEnabled = isEnabled;
            DriverIdNumber = driverIdNumber;
            TruckNumber = truckNumber;
        }

        public override object Clone()
        {
            var src = this;
            AgriculturalExceptionMoment? dest = (AgriculturalExceptionMoment?)Activator.CreateInstance(src.GetType());
            dest = (AgriculturalExceptionMoment)PopulateClone(src, dest!);
            dest.IsEnabled = src.IsEnabled;
            return dest;
        }
    }
}