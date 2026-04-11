using Automation.HOS.TimelineNavigator.Moments;

namespace Automation.HOS.TimelineNavigator.Timelines;

internal static class StartOfDayMomentFactory
{
    public static StartOfDayMoment CreateInstance(DateTime timestamp, string? driverIdNumber, string? truckNumber)
    {
        return new StartOfDayMoment(timestamp, driverIdNumber, truckNumber);
    }
}