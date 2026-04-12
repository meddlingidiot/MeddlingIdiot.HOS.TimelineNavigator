using Automation.HOS.TimelineNavigator;
using Automation.HOS.TimelineNavigator.Moments;

namespace Automation.HOS.TImelineNavigator.UnitTests.Moments;

public class DutyChangeMomentTests
{
    [Test]
    public async Task HaveDefaultPropertyValues()
    {
        var sut = new DutyStatusChangeMoment();
        await Assert.That(sut.Timestamp).IsEqualTo(DateTime.MinValue);
        await Assert.That(sut.DriverIdNumber).IsNull();
        await Assert.That(sut.CurrentDutyStatus).IsEqualTo(DutyStatus.Unknown);
    }
}
