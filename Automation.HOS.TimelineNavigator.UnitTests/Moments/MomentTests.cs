using Automation.HOS.TimelineNavigator;
using Automation.HOS.TimelineNavigator.Moments;

namespace Automation.HOS.TImelineNavigator.UnitTests.Moments;

public class MomentTests
{
    [Test]
    public async Task MakeAClone()
    {
        var sut = new StartOfDayMoment(DateTime.MaxValue, "123", "234");
        var clone = (Moment)sut.Clone();
        await Assert.That(clone.DriverIdNumber).IsEqualTo(sut.DriverIdNumber);
        await Assert.That(clone.TruckNumber).IsEqualTo(sut.TruckNumber);
        await Assert.That(clone.Timestamp).IsEqualTo(sut.Timestamp);
    }
}
