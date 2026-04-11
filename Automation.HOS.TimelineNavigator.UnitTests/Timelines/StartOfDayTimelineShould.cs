using Automation.HOS.TimelineNavigator.Explorers;
using Automation.HOS.TimelineNavigator.Moments;
using Automation.HOS.TimelineNavigator.Timelines;
using NUnit.Framework;

namespace Automation.HOS.TimelineNavigator.UnitTests.Timelines
{
    [TestFixture]
    public class StartOfDayTimelineShould
    {
        [Test]
        public void JumpByOneDayNextHappyPath()
        {
            var sut = new StartOfDayTimeline<StartOfDayMoment>(new StartOfDayTimelineOptions(DateTime.MinValue));

            sut.Add(new StartOfDayMoment(DateTime.Parse("01/29/2023 00:00:00")));
            sut.Add(new StartOfDayMoment(DateTime.Parse("01/27/2023 00:00:00")));
            sut.MoveOnOrBefore(DateTime.Parse("01/26/2023 03:32:31 PM"));
            Assert.That(sut.IsBeginningOfTime(), Is.True);
            Assert.That(sut.IsEndOfTime(), Is.False);
            Assert.That(sut.CurrentMoment.Timestamp, Is.EqualTo(DateTime.MinValue));
            sut.Next();
            Assert.That(sut.CurrentMoment.Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 00:00:00")));
            sut.Next();
            Assert.That(sut.CurrentMoment.Timestamp, Is.EqualTo(DateTime.Parse("01/28/2023 00:00:00")));
            sut.Next();
            Assert.That(sut.CurrentMoment.Timestamp, Is.EqualTo(DateTime.Parse("01/29/2023 00:00:00")));
            sut.Next();
            sut.Next();
            sut.Next();
            sut.Next();
            sut.Next();
            sut.Next();
            sut.Next();
            sut.Next();
            sut.Next();
            sut.Next();
            sut.Next();
            sut.Next();
            sut.Next();
            Assert.That(sut.CurrentMoment.Timestamp, Is.EqualTo(DateTime.Parse("02/11/2023 00:00:00")));
            sut.Next();
            Assert.That(sut.IsBeginningOfTime(), Is.False);
            Assert.That(sut.IsEndOfTime(), Is.True);
            Assert.That(sut.CurrentMoment.Timestamp, Is.EqualTo(DateTime.Parse("02/11/2023 00:00:00")));
            sut.Next();
            Assert.That(sut.IsBeginningOfTime(), Is.False);
            Assert.That(sut.IsEndOfTime(), Is.True);
            Assert.That(sut.CurrentMoment.Timestamp, Is.EqualTo(DateTime.Parse("02/11/2023 00:00:00")));
            sut.Next();
            Assert.That(sut.IsBeginningOfTime(), Is.False);
            Assert.That(sut.IsEndOfTime(), Is.True);
            Assert.That(sut.CurrentMoment.Timestamp, Is.EqualTo(DateTime.Parse("02/11/2023 00:00:00")));
            sut.Next();
            Assert.That(sut.IsBeginningOfTime(), Is.False);
            Assert.That(sut.IsEndOfTime(), Is.True);
            Assert.That(sut.CurrentMoment.Timestamp, Is.EqualTo(DateTime.Parse("02/11/2023 00:00:00")));
            sut.Prior();
            Assert.That(sut.CurrentMoment.Timestamp, Is.EqualTo(DateTime.Parse("02/10/2023 00:00:00")));
            Assert.That(sut.IsBeginningOfTime(), Is.False);
            Assert.That(sut.IsEndOfTime(), Is.False);
        }

        [Test]
        public void JumpByOneDayPriorHappyPath()
        {
            var sut = new StartOfDayTimeline<StartOfDayMoment>(new StartOfDayTimelineOptions (DateTime.MinValue));

            sut.Add(new StartOfDayMoment(DateTime.Parse("01/29/2023 00:00:00")));
            sut.Add(new StartOfDayMoment(DateTime.Parse("01/27/2023 00:00:00")));
            sut.MoveOnOrBefore(DateTime.Parse("01/29/2023 03:32:31 PM"));
            Assert.That(sut.IsBeginningOfTime(), Is.False);
            Assert.That(sut.IsEndOfTime(), Is.False);
            Assert.That(sut.CurrentMoment.Timestamp, Is.EqualTo(DateTime.Parse("01/29/2023 00:00:00")));
            sut.Prior();
            Assert.That(sut.IsBeginningOfTime(), Is.False);
            Assert.That(sut.IsEndOfTime(), Is.False);
            Assert.That(sut.CurrentMoment.Timestamp, Is.EqualTo(DateTime.Parse("01/28/2023 00:00:00")));
            sut.Prior();
            Assert.That(sut.CurrentMoment.Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 00:00:00")));
            sut.Prior();
            Assert.That(sut.IsBeginningOfTime(), Is.True);
            Assert.That(sut.IsEndOfTime(), Is.False);
            Assert.That(sut.CurrentMoment.Timestamp, Is.EqualTo(DateTime.MinValue));
        }

        [Test]
        public void JumpAroundBeforeBeginningOfTime()
        {
            var sut = new StartOfDayTimeline<StartOfDayMoment>(new StartOfDayTimelineOptions(DateTime.MinValue));

            sut.Add(new StartOfDayMoment(DateTime.Parse("01/29/2023 00:00:00")));
            sut.Add(new StartOfDayMoment(DateTime.Parse("01/27/2023 00:00:00")));
            sut.MoveOnOrBefore(DateTime.Parse("01/27/2023 03:32:31 PM"));
            Assert.That(sut.IsBeginningOfTime(), Is.False);
            Assert.That(sut.IsEndOfTime(), Is.False);
            Assert.That(sut.CurrentMoment.Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 00:00:00")));
            sut.Prior();
            Assert.That(sut.IsBeginningOfTime(), Is.True);
            Assert.That(sut.IsEndOfTime(), Is.False);
            Assert.That(sut.CurrentMoment.Timestamp, Is.EqualTo(DateTime.MinValue));
            sut.Prior();
            Assert.That(sut.IsBeginningOfTime(), Is.True);
            Assert.That(sut.IsEndOfTime(), Is.False);
            Assert.That(sut.CurrentMoment.Timestamp, Is.EqualTo(DateTime.MinValue));
            sut.Next();
            Assert.That(sut.IsBeginningOfTime(), Is.False);
            Assert.That(sut.IsEndOfTime(), Is.False);
            Assert.That(sut.CurrentMoment.Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 00:00:00")));

        }

        [Test]
        public void TestMoveOnOrBefore()
        {
            var sut = new StartOfDayTimeline<StartOfDayMoment>(new StartOfDayTimelineOptions( DateTime.MinValue ));

            sut.Add(new StartOfDayMoment(DateTime.Parse("01/29/2023 00:00:00")));
            sut.Add(new StartOfDayMoment(DateTime.Parse("01/27/2023 00:00:00")));

            //Between points
            sut.MoveOnOrBefore(DateTime.Parse("01/27/2023 03:32:31 PM"));
            Assert.That(sut.IsBeginningOfTime(), Is.False);
            Assert.That(sut.IsEndOfTime(), Is.False);
            Assert.That(sut.CurrentMoment.Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 00:00:00")));
            //On a point
            sut.MoveOnOrBefore(DateTime.Parse("01/27/2023"));
            Assert.That(sut.IsBeginningOfTime(), Is.False);
            Assert.That(sut.IsEndOfTime(), Is.False);
            Assert.That(sut.CurrentMoment.Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 00:00:00")));
            //Before First  point
            sut.MoveOnOrBefore(DateTime.Parse("01/26/2023"));
            Assert.That(sut.IsBeginningOfTime(), Is.True);
            Assert.That(sut.IsEndOfTime(), Is.False);
            Assert.That(sut.CurrentMoment.Timestamp, Is.EqualTo(DateTime.MinValue));
            //After last point
            sut.MoveOnOrBefore(DateTime.Parse("02/12/2023 13:00:00"));
            Assert.That(sut.IsBeginningOfTime(), Is.False);
            Assert.That(sut.IsEndOfTime(), Is.True);
            Assert.That(sut.CurrentMoment.Timestamp, Is.EqualTo(DateTime.Parse("02/11/2023 00:00:00")));

        }

        [Test]
        public void TestMoveOnOrAfter()
        {
            var sut = new StartOfDayTimeline<StartOfDayMoment>(new StartOfDayTimelineOptions( DateTime.MinValue ));

            sut.Add(new StartOfDayMoment(DateTime.Parse("01/29/2023 00:00:00")));
            sut.Add(new StartOfDayMoment(DateTime.Parse("01/27/2023 00:00:00")));

            //Between points
            sut.MoveOnOrAfter(DateTime.Parse("01/27/2023 03:32:31 PM"));
            Assert.That(sut.IsBeginningOfTime(), Is.False);
            Assert.That(sut.IsEndOfTime(), Is.False);
            Assert.That(sut.CurrentMoment.Timestamp, Is.EqualTo(DateTime.Parse("01/28/2023 00:00:00")));
            //On a point
            sut.MoveOnOrAfter(DateTime.Parse("01/27/2023"));
            Assert.That(sut.IsBeginningOfTime(), Is.False);
            Assert.That(sut.IsEndOfTime(), Is.False);
            Assert.That(sut.CurrentMoment.Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 00:00:00")));
            //Before First  point
            sut.MoveOnOrAfter(DateTime.Parse("01/26/2023"));
            Assert.That(sut.IsBeginningOfTime(), Is.False);
            Assert.That(sut.IsEndOfTime(), Is.False);
            Assert.That(sut.CurrentMoment.Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 00:00:00")));
            //After last point
            sut.MoveOnOrAfter(DateTime.Parse("02/12/2023 13:00:00"));
            Assert.That(sut.IsBeginningOfTime(), Is.False);
            Assert.That(sut.IsEndOfTime(), Is.True);
            Assert.That(sut.CurrentMoment.Timestamp, Is.EqualTo(DateTime.MaxValue));

        }

        [Test]
        public void BugAtEndOfTimeProof()
        {
            var navigator = new TimelineNavigator(new(null, true, 13));
            navigator.Add(new DutyStatusChangeMoment(DateTime.Parse("8/24/2023"), DutyStatus.Driving));
            navigator.Add(new DutyStatusChangeMoment(DateTime.Parse("8/24/2023 08:00:00"), DutyStatus.Sleeper));
            navigator.Add(new DutyStatusChangeMoment(DateTime.Parse("8/24/2023 16:00:00"), DutyStatus.OnDuty));
            navigator.Add(new DutyStatusChangeMoment(DateTime.Parse("8/24/2023 18:00:00"), DutyStatus.OffDuty));
            navigator.Add(new DutyStatusChangeMoment(DateTime.Parse("8/24/2023 21:00:00"), DutyStatus.Driving));
            navigator.Add(new DutyStatusChangeMoment(DateTime.Parse("8/25/2023 11:00:00"), DutyStatus.OffDuty));

            navigator.JumpTo(DateTime.Parse("8/24/2023 12:23 PM"));
            var endOfAuditWindow = navigator.FindRest(
                TimeSpan.FromHours(34),
                TimelineDirection.Forward,
                PreferredEndOfRest.Ending,
                MoveTo.None);
            var startOfAuditWindow = navigator.FindRest(
                TimeSpan.FromHours(34),
                TimelineDirection.Backward,
                PreferredEndOfRest.Beginning,
                MoveTo.NewLocation);

            Assert.That(endOfAuditWindow.Timestamp, Is.EqualTo(DateTime.Parse("09/07/2023")));
        }
    }
}
