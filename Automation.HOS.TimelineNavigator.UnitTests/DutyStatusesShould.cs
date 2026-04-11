using NUnit.Framework;

namespace Automation.HOS.TimelineNavigator.UnitTests;

[TestFixture]
public class DutyStatusesShould
{
    [Test]
    public void HaveCorrectEnumValues()
    {
        Assert.That((int)DutyStatus.Unknown, Is.EqualTo(0));
        Assert.That((int)DutyStatus.OffDuty, Is.EqualTo(1));
        Assert.That((int)DutyStatus.Sleeper, Is.EqualTo(2));
        Assert.That((int)DutyStatus.Driving, Is.EqualTo(3));
        Assert.That((int)DutyStatus.OnDuty, Is.EqualTo(4));
        Assert.That((int)DutyStatus.OffDutyWaitingAtWellSite, Is.EqualTo(5));
    }

    [Test]
    public void HaveNoDutyStatusesAsEmptyList()
    {
        Assert.That(DutyStatuses.NoDutyStatuses, Is.Empty);
    }

    [Test]
    public void HaveDrivingDutyStatusContainingOnlyDriving()
    {
        Assert.That(DutyStatuses.DrivingDutyStatus, Has.Count.EqualTo(1));
        Assert.That(DutyStatuses.DrivingDutyStatus, Contains.Item(DutyStatus.Driving));
    }

    [Test]
    public void HaveWorkingDutyStatusesContainingDrivingAndOnDuty()
    {
        Assert.That(DutyStatuses.WorkingDutyStatuses, Has.Count.EqualTo(2));
        Assert.That(DutyStatuses.WorkingDutyStatuses, Contains.Item(DutyStatus.Driving));
        Assert.That(DutyStatuses.WorkingDutyStatuses, Contains.Item(DutyStatus.OnDuty));
    }

    [Test]
    public void HaveRestDutyStatusesContainingOffDutySleeperAndOffDutyWaitingAtWellSite()
    {
        Assert.That(DutyStatuses.RestDutyStatuses, Has.Count.EqualTo(3));
        Assert.That(DutyStatuses.RestDutyStatuses, Contains.Item(DutyStatus.OffDuty));
        Assert.That(DutyStatuses.RestDutyStatuses, Contains.Item(DutyStatus.Sleeper));
        Assert.That(DutyStatuses.RestDutyStatuses, Contains.Item(DutyStatus.OffDutyWaitingAtWellSite));
    }

    [Test]
    public void HaveAllRestDutyStatusesContainingUnknownOffDutySleeperAndOffDutyWaitingAtWellSite()
    {
        Assert.That(DutyStatuses.AllRestDutyStatuses, Has.Count.EqualTo(4));
        Assert.That(DutyStatuses.AllRestDutyStatuses, Contains.Item(DutyStatus.Unknown));
        Assert.That(DutyStatuses.AllRestDutyStatuses, Contains.Item(DutyStatus.OffDuty));
        Assert.That(DutyStatuses.AllRestDutyStatuses, Contains.Item(DutyStatus.Sleeper));
        Assert.That(DutyStatuses.AllRestDutyStatuses, Contains.Item(DutyStatus.OffDutyWaitingAtWellSite));
    }

    [Test]
    public void HaveAllNormalDutyStatusesContainingOffDutySleeperDrivingAndOnDuty()
    {
        Assert.That(DutyStatuses.AllNormalDutyStatuses, Has.Count.EqualTo(4));
        Assert.That(DutyStatuses.AllNormalDutyStatuses, Contains.Item(DutyStatus.OffDuty));
        Assert.That(DutyStatuses.AllNormalDutyStatuses, Contains.Item(DutyStatus.Sleeper));
        Assert.That(DutyStatuses.AllNormalDutyStatuses, Contains.Item(DutyStatus.Driving));
        Assert.That(DutyStatuses.AllNormalDutyStatuses, Contains.Item(DutyStatus.OnDuty));
    }

    [Test]
    public void HaveAllButDrivingDutyStatusesContainingOffDutySleeperOnDutyAndOffDutyWaitingAtWellSite()
    {
        Assert.That(DutyStatuses.AllButDrivingDutyStatuses, Has.Count.EqualTo(4));
        Assert.That(DutyStatuses.AllButDrivingDutyStatuses, Contains.Item(DutyStatus.OffDuty));
        Assert.That(DutyStatuses.AllButDrivingDutyStatuses, Contains.Item(DutyStatus.Sleeper));
        Assert.That(DutyStatuses.AllButDrivingDutyStatuses, Contains.Item(DutyStatus.OnDuty));
        Assert.That(DutyStatuses.AllButDrivingDutyStatuses, Contains.Item(DutyStatus.OffDutyWaitingAtWellSite));
    }

    [Test]
    public void NotContainDrivingInAllButDrivingDutyStatuses()
    {
        Assert.That(DutyStatuses.AllButDrivingDutyStatuses, Does.Not.Contain(DutyStatus.Driving));
    }

    [Test]
    public void NotContainUnknownInWorkingDutyStatuses()
    {
        Assert.That(DutyStatuses.WorkingDutyStatuses, Does.Not.Contain(DutyStatus.Unknown));
    }

    [Test]
    public void NotContainDrivingInRestDutyStatuses()
    {
        Assert.That(DutyStatuses.RestDutyStatuses, Does.Not.Contain(DutyStatus.Driving));
    }

    [Test]
    public void NotContainUnknownInAllNormalDutyStatuses()
    {
        Assert.That(DutyStatuses.AllNormalDutyStatuses, Does.Not.Contain(DutyStatus.Unknown));
    }
}
