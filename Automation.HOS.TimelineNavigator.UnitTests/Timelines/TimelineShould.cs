using Automation.HOS.TimelineNavigator.Moments;
using Automation.HOS.TimelineNavigator.Timelines;
using NUnit.Framework;

namespace Automation.HOS.TimelineNavigator.UnitTests.Timelines
{
    [TestFixture]
    internal class TimelineShould
    {

        public void AddSomeMomentsToTimeline(Timeline<DutyStatusChangeMoment> sut)
        {
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/20/2023 01:00:00"), DutyStatus.OffDuty, "1"));
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/20/2023 10:00:00"), DutyStatus.Driving, "1"));
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/20/2023 21:00:00"), DutyStatus.OnDuty, "1"));
        }

        [Test]
        public void LoadAndWalkThruHappyPath()
        {
            var sut = new Timeline<DutyStatusChangeMoment>();
            AddSomeMomentsToTimeline(sut);

            sut.First();
            Assert.That(sut.IsBeginningOfTime(), Is.False);
            Assert.That(sut.IsEndOfTime(), Is.False);
            Assert.That(sut.CurrentMoment.CurrentDutyStatus, Is.EqualTo(DutyStatus.OffDuty));
            sut.Next();
            Assert.That(sut.IsBeginningOfTime(), Is.False);
            Assert.That(sut.IsEndOfTime(), Is.False);
            Assert.That(sut.CurrentMoment.CurrentDutyStatus, Is.EqualTo(DutyStatus.Driving));
            sut.Next();
            Assert.That(sut.IsBeginningOfTime(), Is.False);
            Assert.That(sut.IsEndOfTime(), Is.True);
            Assert.That(sut.CurrentMoment.CurrentDutyStatus, Is.EqualTo(DutyStatus.OnDuty));
            sut.Next();
        }

        [Test]
        public void WalkBackwardsFromFirst()
        {
            var sut = new Timeline<DutyStatusChangeMoment>();
            AddSomeMomentsToTimeline(sut);

            sut.First();
            sut.Prior();
            Assert.That(sut.IsBeginningOfTime, Is.True);
            Assert.That(sut.IsEndOfTime(), Is.False);
            Assert.That(sut.CurrentMoment.CurrentDutyStatus, Is.EqualTo(DutyStatus.Unknown));
            sut.Next();
            Assert.That(sut.IsBeginningOfTime(), Is.False);
            Assert.That(sut.IsEndOfTime(), Is.False);
            Assert.That(sut.CurrentMoment.CurrentDutyStatus, Is.EqualTo(DutyStatus.OffDuty));

        }

        [Test]
        public void WalkBackwardsBeyondBeginningOfTime()
        {
            var sut = new Timeline<DutyStatusChangeMoment>();
            AddSomeMomentsToTimeline(sut);

            sut.First();
            sut.Prior();
            sut.Prior();
            sut.Prior();
            sut.Prior();
            Assert.That(sut.IsBeginningOfTime, Is.True);
            Assert.That(sut.IsEndOfTime(), Is.False);
            Assert.That(sut.CurrentMoment.CurrentDutyStatus, Is.EqualTo(DutyStatus.Unknown));
            sut.Next();
            Assert.That(sut.IsBeginningOfTime(), Is.False);
            Assert.That(sut.IsEndOfTime(), Is.False);
            Assert.That(sut.CurrentMoment.CurrentDutyStatus, Is.EqualTo(DutyStatus.OffDuty));

        }

        [Test]
        public void WalkBeyondTheEndOfTime()
        {
            var sut = new Timeline<DutyStatusChangeMoment>();
            AddSomeMomentsToTimeline(sut);

            sut.First();
            sut.Next();
            sut.Next();
            sut.Next();
            sut.Next();
            sut.Next();
            sut.Next();
            sut.Next();
            sut.Next();
            Assert.That(sut.IsBeginningOfTime(), Is.False);
            Assert.That(sut.IsEndOfTime(), Is.True);
            Assert.That(sut.CurrentMoment.CurrentDutyStatus, Is.EqualTo(DutyStatus.OnDuty));
            sut.Prior();
            Assert.That(sut.IsBeginningOfTime(), Is.False);
            Assert.That(sut.IsEndOfTime(), Is.False);
            Assert.That(sut.CurrentMoment.CurrentDutyStatus, Is.EqualTo(DutyStatus.Driving));
        }

        [Test]
        public void MoveOnOrBeforeHappyPath()
        {
            var sut = new Timeline<DutyStatusChangeMoment>();
            AddSomeMomentsToTimeline(sut);

            //Between Points
            sut.MoveOnOrBefore(DateTime.Parse("01/20/2023 10:30:00"));
            Assert.That(sut.CurrentMoment.Timestamp, Is.EqualTo(DateTime.Parse("01/20/2023 10:00:00")));

            //On A point
            sut.MoveOnOrBefore(DateTime.Parse("01/20/2023 10:00:00"));
            Assert.That(sut.CurrentMoment.Timestamp, Is.EqualTo(DateTime.Parse("01/20/2023 10:00:00")));

            //Before first point
            sut.MoveOnOrBefore(DateTime.Parse("01/20/2023 00:00:00"));
            Assert.That(sut.CurrentMoment.Timestamp, Is.EqualTo(DateTime.MinValue));

            //After last point
            sut.MoveOnOrBefore(DateTime.Parse("01/20/2023 23:00:00"));
            Assert.That(sut.CurrentMoment.Timestamp, Is.EqualTo(DateTime.Parse("01/20/2023 21:00:00")));

        }

        [Test]
        public void MoveOnOrAfterHappyPath()
        {
            var sut = new Timeline<DutyStatusChangeMoment>();
            AddSomeMomentsToTimeline(sut);

            //Between Points
            sut.MoveOnOrAfter(DateTime.Parse("01/20/2023 10:30:00"));
            Assert.That(sut.CurrentMoment.Timestamp, Is.EqualTo(DateTime.Parse("01/20/2023 21:00:00")));

            //On A point
            sut.MoveOnOrAfter(DateTime.Parse("01/20/2023 10:00:00"));
            Assert.That(sut.CurrentMoment.Timestamp, Is.EqualTo(DateTime.Parse("01/20/2023 10:00:00")));

            //Before first point
            sut.MoveOnOrAfter(DateTime.Parse("01/20/2023 00:00:00"));
            Assert.That(sut.CurrentMoment.Timestamp, Is.EqualTo(DateTime.Parse("01/20/2023 01:00:00")));

            //After last point
            sut.MoveOnOrAfter(DateTime.Parse("01/20/2023 23:00:00"));
            Assert.That(sut.CurrentMoment.Timestamp, Is.EqualTo(DateTime.Parse("01/20/2023 21:00:00")));

        }

        [Test]
        public void InitializeShouldSortMoments()
        {
            var sut = new Timeline<DutyStatusChangeMoment>();
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/20/2023 21:00:00"), DutyStatus.OnDuty, "1"));
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/20/2023 10:00:00"), DutyStatus.Driving, "1"));
            sut.Add(new DutyStatusChangeMoment(DateTime.MinValue, DutyStatus.OffDuty, "1"));
            sut.Initialize();
            sut.First();
            sut.Next();
            Assert.That(sut.IsBeginningOfTime(), Is.False);
            Assert.That(sut.IsEndOfTime(), Is.False);
            Assert.That(sut.CurrentMoment.CurrentDutyStatus, Is.EqualTo(DutyStatus.Driving));
            sut.Next();
            Assert.That(sut.IsBeginningOfTime(), Is.False);
            Assert.That(sut.IsEndOfTime(), Is.True);
            Assert.That(sut.CurrentMoment.CurrentDutyStatus, Is.EqualTo(DutyStatus.OnDuty));
            sut.Next();

        }

        [Test]
        public void EmptyTimelineShouldntFail()
        {
            var sut = new Timeline<DutyStatusChangeMoment>();
            sut.Initialize();

            sut.First();
            Assert.That(sut.BaseMoment.Timestamp, Is.EqualTo(DateTime.MinValue));

        }
    }
}
