using Automation.HOS.TimelineNavigator.Moments;
using NUnit.Framework;

namespace Automation.HOS.TimelineNavigator.UnitTests.Moments
{
    public class DutyChangeMomentShould
    {
        [Test]
        public void HaveDefaultPropertyValues()
        {
            var sut = new DutyStatusChangeMoment();
            Assert.That(sut.Timestamp, Is.EqualTo(DateTime.MinValue));
            Assert.That(sut.DriverIdNumber, Is.Null);
            Assert.That(sut.CurrentDutyStatus, Is.EqualTo(DutyStatus.Unknown));
        }

     }
}