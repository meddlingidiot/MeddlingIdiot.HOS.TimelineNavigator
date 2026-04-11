using NUnit.Framework;
using Automation.HOS.TimelineNavigator.Mapper;

namespace Automation.HOS.TimelineNavigator.UnitTests.Mapper;

[TestFixture]
public class GeotabDutyStatusToDutyStatusMapperShould
{
    [TestCase("OFF", DutyStatus.OffDuty)]
    [TestCase("SB",  DutyStatus.Sleeper)]
    [TestCase("D",   DutyStatus.Driving)]
    [TestCase("ON",  DutyStatus.OnDuty)]
    [TestCase("WT",  DutyStatus.OffDutyWaitingAtWellSite)]
    public void MapKnownGeotabStatus_ReturnsExpectedDutyStatus(string input, DutyStatus expected)
    {
        Assert.That(GeotabDutyStatusToDutyStatusMapper.Map(input), Is.EqualTo(expected));
    }

    [TestCase("")]
    [TestCase("off")]
    [TestCase("sb")]
    [TestCase("d")]
    [TestCase("on")]
    [TestCase("wt")]
    [TestCase("UNKNOWN")]
    [TestCase("X")]
    public void MapUnknownGeotabStatus_ReturnsUnknown(string input)
    {
        Assert.That(GeotabDutyStatusToDutyStatusMapper.Map(input), Is.EqualTo(DutyStatus.Unknown));
    }
}
