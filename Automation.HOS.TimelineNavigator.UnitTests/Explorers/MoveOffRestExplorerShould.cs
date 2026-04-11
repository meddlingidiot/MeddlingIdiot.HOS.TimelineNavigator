using System.Diagnostics;
using Automation.HOS.TimelineNavigator.Explorers;
using Automation.HOS.TimelineNavigator.Moments;
using Automation.HOS.TimelineNavigator.Timelines;
using Automation.HOS.TimelineNavigator.Utilities;
using NUnit.Framework;

namespace Automation.HOS.TimelineNavigator.UnitTests.Explorers
{
    [TestFixture]
    public class MoveOffRestExplorerShould
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
        public void MoveOffRestShouldNotMoveIfNotOnRest()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions());
            PopulateTimeline(sut);
            sut.Initialize();

            sut.JumpTo(DateTime.Parse("02/14/2023 09:00:00"));

            sut.MoveOffRest(TimelineDirection.Forward);
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("02/14/2023 08:00:00")));
        }

        [Test]
        public void MoveOffRestShouldMoveOffRestIfBeforeData()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions());
            PopulateTimeline(sut);
            sut.Initialize();

            sut.JumpTo(DateTime.Parse("02/14/2023 07:00:00"));

            sut.MoveOffRest(TimelineDirection.Forward);
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("02/14/2023 08:00:00")));
        }

        [Test]
        public void MoveOffRestShouldMoveToBotIfBeforeDataGoingBackwards()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions());
            PopulateTimeline(sut);
            sut.Initialize();

            sut.JumpTo(DateTime.Parse("02/14/2023 07:00:00"));

            sut.MoveOffRest(TimelineDirection.Backward);
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.MinValue));
        }

        [Test]
        public void MoveOffRestShouldMoveOffRest()
        {
            var logger = new InMemoryLogger();
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions());
            PopulateTimeline(sut);
            sut.Initialize();

            sut.JumpTo(DateTime.Parse("02/14/2023 18:00:00"));

            sut.MoveOffRest(TimelineDirection.Forward, logger);
            Debug.WriteLine(logger.ToString());
            File.WriteAllText("C:\\Temp\\MoveOffRestDebug.txt", logger.ToString());
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("02/14/2023 21:00:00")));
        }

        [Test]
        public void MoveOffRestShouldMoveToEotIfAfterData()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions());
            PopulateTimeline(sut);
            sut.Initialize();

            sut.JumpTo(DateTime.Parse("04/14/2023 18:00:00"));

            sut.MoveOffRest(TimelineDirection.Forward);
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("03/01/2023 00:00:00")));
            Assert.That(sut.Finish.Timestamp, Is.EqualTo(DateTime.MaxValue));
        }

        [Test]
        public void MoveOffRestShouldNotMoveIfNotOnRestBackwards()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions());
            PopulateTimeline(sut);
            sut.Initialize();

            sut.JumpTo(DateTime.Parse("02/14/2023 09:00:00"));

            sut.MoveOffRest(TimelineDirection.Backward);
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("02/14/2023 08:00:00")));
        }

        [Test]
        public void MoveOffRestShouldMoveOffRestIfBeforeDataBackwards()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions());
            PopulateTimeline(sut);
            sut.Initialize();

            sut.JumpTo(DateTime.Parse("02/14/2023 07:00:00"));

            sut.MoveOffRest(TimelineDirection.Backward);
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.MinValue));
        }

        [Test]
        public void MoveOffRestShouldMoveOffRestBackwards()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions());
            PopulateTimeline(sut);
            sut.Initialize();

            sut.JumpTo(DateTime.Parse("02/14/2023 18:00:00"));

            sut.MoveOffRest(TimelineDirection.Backward);
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("02/14/2023 08:00:00")));
        }

        [Test]
        public void MoveOffRestShouldMoveToEotIfAfterDataBackwards()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions());
            PopulateTimeline(sut);
            sut.Initialize();

            sut.JumpTo(DateTime.Parse("04/14/2023 18:00:00"));

            sut.MoveOffRest(TimelineDirection.Forward);
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("03/01/2023 00:00:00")));
            Assert.That(sut.Finish.Timestamp, Is.EqualTo(DateTime.MaxValue));
        }

    }
}
