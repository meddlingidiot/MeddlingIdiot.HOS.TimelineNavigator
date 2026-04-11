using Automation.HOS.TimelineNavigator.Explorers;
using Automation.HOS.TimelineNavigator.Moments;
using Automation.HOS.TimelineNavigator.Timelines;
using NUnit.Framework;

namespace Automation.HOS.TimelineNavigator.UnitTests.Explorers
{
    [TestFixture]
    public class RestFinderExplorerShould
    {
        private void PopulateTimeline(ITimelineNavigator data)
        {
            data.Add(new DutyStatusChangeMoment(DateTime.Parse("02/14/2023 08:00:00"), DutyStatus.OnDuty));
            //ON DUTY 10 HOURS
            data.Add(new DutyStatusChangeMoment(DateTime.Parse("02/14/2023 18:00:00"), DutyStatus.OffDuty));
            //OFF DUTY 2 HOURS
            data.Add(new DutyStatusChangeMoment(DateTime.Parse("02/14/2023 20:00:00"), DutyStatus.Sleeper));
            //SLEEPER 1 HOUR (TOTAL REST 3 HOURS)
            data.Add(new DutyStatusChangeMoment(DateTime.Parse("02/14/2023 21:00:00"), DutyStatus.OnDuty));
            //ON DUTY 3 HOURS
            data.Add(new DutyStatusChangeMoment(DateTime.Parse("02/15/2023 00:00:00"), DutyStatus.Sleeper));
            //SLEEPER 4 HOURS
            data.Add(new DutyStatusChangeMoment(DateTime.Parse("02/15/2023 04:00:00"), DutyStatus.OnDuty));
            //ON DUTY Rest of day
            data.Add(new DutyStatusChangeMoment(DateTime.Parse("02/16/2023 00:00:00"), DutyStatus.Unknown));
            //UNKNOWN TIL EOT
        }

        [Test]
        public void FindRestGoingForwardMoveToBeginning()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions());
            PopulateTimeline(sut);
            sut.Initialize();

            sut.JumpTo(DateTime.Parse("02/14/2023 09:00:00"));

            var found = sut.FindRest(TimeSpan.FromHours(4), TimelineDirection.Forward, PreferredEndOfRest.Beginning);
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("02/15/2023 00:00:00")));
            Assert.That(found.Timestamp, Is.EqualTo(sut.Start.Timestamp));

            sut.JumpTo(DateTime.Parse("02/14/2023 09:00:00"));

            found = sut.FindRest(TimeSpan.FromHours(2), TimelineDirection.Forward, PreferredEndOfRest.Beginning);
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("02/14/2023 18:00:00")));
            Assert.That(found.Timestamp, Is.EqualTo(sut.Start.Timestamp));
        }

        [Test]
        public void FindRestGoingForwardMoveToEnd()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions());
            PopulateTimeline(sut);
            sut.Initialize();

            sut.JumpTo(DateTime.Parse("02/14/2023 09:00:00"));

            var found = sut.FindRest(TimeSpan.FromHours(4), TimelineDirection.Forward, PreferredEndOfRest.Ending);
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("02/15/2023 00:00:00")));
            Assert.That(found.Timestamp, Is.EqualTo(sut.Start.Timestamp));

            sut.JumpTo(DateTime.Parse("02/14/2023 09:00:00"));

            found = sut.FindRest(TimeSpan.FromHours(2), TimelineDirection.Forward, PreferredEndOfRest.Ending);
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("02/14/2023 20:00:00")));
            Assert.That(found.Timestamp, Is.EqualTo(sut.Start.Timestamp));
        }

        [Test]
        public void FindRestGoingForwardNoMove()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions());
            PopulateTimeline(sut);
            sut.Initialize();

            sut.JumpTo(DateTime.Parse("02/14/2023 09:00:00"));

            var found = sut.FindRest(TimeSpan.FromHours(4), TimelineDirection.Forward, PreferredEndOfRest.Ending, MoveTo.None);
            Assert.That(found.Timestamp, Is.EqualTo(DateTime.Parse("02/15/2023 00:00:00")));
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("02/14/2023 08:00:00")));

            sut.JumpTo(DateTime.Parse("02/14/2023 09:00:00"));

            found = sut.FindRest(TimeSpan.FromHours(2), TimelineDirection.Forward, PreferredEndOfRest.Ending, MoveTo.None);
            Assert.That(found.Timestamp, Is.EqualTo(DateTime.Parse("02/14/2023 20:00:00")));
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("02/14/2023 08:00:00")));
        }

        [Test]
        public void FindRestGoingBackwardMoveToBeginning()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions());
            PopulateTimeline(sut);
            sut.Initialize();

            sut.JumpTo(DateTime.Parse("02/14/2023 22:00:00"));

            var found = sut.FindRest(TimeSpan.FromHours(4), TimelineDirection.Backward, PreferredEndOfRest.Beginning);
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.MinValue));
            Assert.That(found.Timestamp, Is.EqualTo(sut.Start.Timestamp));

            sut.JumpTo(DateTime.Parse("02/14/2023 22:00:00"));

            found = sut.FindRest(TimeSpan.FromHours(2), TimelineDirection.Backward, PreferredEndOfRest.Beginning);
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("02/14/2023 18:00:00")));
            Assert.That(found.Timestamp, Is.EqualTo(sut.Start.Timestamp));
        }

        [Test]
        public void FindRestGoingBackwardNoMove()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions());
            PopulateTimeline(sut);
            sut.Initialize();

            sut.JumpTo(DateTime.Parse("02/14/2023 22:00:00"));

            var found = sut.FindRest(TimeSpan.FromHours(4), TimelineDirection.Backward, PreferredEndOfRest.Ending, MoveTo.None);
            Assert.That(found.Timestamp, Is.EqualTo(DateTime.Parse("02/14/2023 00:00:00")));
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("02/14/2023 21:00:00")));

            sut.JumpTo(DateTime.Parse("02/14/2023 22:00:00"));

            found = sut.FindRest(TimeSpan.FromHours(2), TimelineDirection.Backward, PreferredEndOfRest.Ending, MoveTo.None);
            Assert.That(found.Timestamp, Is.EqualTo(DateTime.Parse("02/14/2023 18:00:00")));
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("02/14/2023 21:00:00")));
        }


    }
}
