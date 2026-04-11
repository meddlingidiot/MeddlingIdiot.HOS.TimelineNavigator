using Automation.HOS.TimelineNavigator.Moments;
using NUnit.Framework;

namespace Automation.HOS.TimelineNavigator.UnitTests.Moments
{
    [TestFixture]
    public class MomentShould
    {

        [Test]
        public void MakeAClone()
        {
            var sut = new StartOfDayMoment(DateTime.MaxValue, "123", "234"); 
            var clone = (Moment)sut.Clone();
            Assert.That(clone.DriverIdNumber, Is.EqualTo(sut.DriverIdNumber));
            Assert.That(clone.TruckNumber, Is.EqualTo(sut.TruckNumber));
            Assert.That(clone.Timestamp, Is.EqualTo(sut.Timestamp));

        }
    }
}
