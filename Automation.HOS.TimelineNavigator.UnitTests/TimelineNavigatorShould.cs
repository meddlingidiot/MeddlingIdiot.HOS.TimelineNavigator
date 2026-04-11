using Automation.HOS.TimelineNavigator.Moments;
using Automation.HOS.TimelineNavigator.Segments;
using Automation.HOS.TimelineNavigator.Timelines;
using NUnit.Framework;

namespace Automation.HOS.TimelineNavigator.UnitTests
{
    [TestFixture]
    public class TimelineNavigatorShould
    {
        [Test]
        public void Merge_timelines_together_for_MoveOnOrBefore()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions());
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/29/2023 08:00:00"), DutyStatus.Driving)); 
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/27/2023 01:00:00"), DutyStatus.Driving));
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/28/2023 01:00:00"), DutyStatus.OnDuty));
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/27/2023 08:00:00"), DutyStatus.Sleeper));
            sut.Add(new GpsMoment(DateTime.Parse("01/27/2023 23:00:00"), (long)0.0, (long)0.0));
            sut.Add(new EngineBusMoment(DateTime.Parse("01/27/2023 23:00:00"), 1000));
            sut.Add(new GpsMoment(DateTime.Parse("01/27/2023 23:00:00"), (long)0.0, (long)0.0));
            sut.Add(new EngineBusMoment(DateTime.Parse("01/27/2023 23:00:00"), 1000));
            sut.Initialize();

            sut.JumpTo(DateTime.Parse("01/27/2023 00:30:00"));
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 00:00:00")));

            sut.JumpTo(DateTime.Parse("01/28/2023 00:00:00"));
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("01/28/2023 00:00:00")));

            sut.JumpTo(DateTime.Parse("01/27/2023 23:00:01"));
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 23:00:00")));
        }

        [Test]
        public void Merge_timelines_together_for_Next()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions());
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/27/2023 01:00:00"), DutyStatus.Driving));
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/27/2023 08:00:00"), DutyStatus.Sleeper));
            sut.Add(new AnchorMoment(DateTime.Parse("01/27/2023 22:00:00"), "StartAudit"));
            sut.Add(new GpsMoment(DateTime.Parse("01/27/2023 23:00:00"), (long)0.0, (long)0.0));
            sut.Add(new EngineBusMoment(DateTime.Parse("01/27/2023 23:00:00"), 1000));
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/28/2023 01:00:00"), DutyStatus.OnDuty));
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/29/2023 08:00:00"), DutyStatus.Driving));
            sut.Initialize();

            sut.JumpTo(DateTime.Parse("01/27/2023 00:30:00"));
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 00:00:00")));
            Assert.That(sut.Finish.Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 01:00:00")));
            Assert.That(sut.Length, Is.EqualTo(TimeSpan.FromHours(1)));
            Assert.That(sut.IsStartOfDay, Is.True);
            sut.Next();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 01:00:00")));
            Assert.That(sut.Finish.Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 08:00:00")));
            Assert.That(sut.CurrentDutyStatusChangeMoment.CurrentDutyStatus, Is.EqualTo(DutyStatus.Driving));
            Assert.That(sut.DutyStatus, Is.EqualTo(DutyStatus.Driving));
            Assert.That(sut.Length, Is.EqualTo(TimeSpan.FromHours(7))); 
            Assert.That(sut.IsStartOfDay, Is.False);
            sut.Next();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 08:00:00")));
            sut.Next();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 22:00:00")));
            sut.Next();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 23:00:00")));
            Assert.That(sut.Odometer, Is.EqualTo(1000));
            sut.Next();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("01/28/2023 00:00:00")));
            sut.Next();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("01/28/2023 01:00:00")));
            sut.Next();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("01/29/2023 00:00:00")));
            sut.Next();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("01/29/2023 08:00:00")));
            sut.Next();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("01/30/2023 00:00:00")));
            sut.Next();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("01/31/2023 00:00:00")));
            sut.Next();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("02/01/2023 00:00:00")));
            sut.Next();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("02/02/2023 00:00:00")));
            sut.Next();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("02/03/2023 00:00:00")));
            sut.Next();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("02/04/2023 00:00:00")));
            sut.Next();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("02/05/2023 00:00:00")));
            sut.Next();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("02/06/2023 00:00:00")));
            sut.Next();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("02/07/2023 00:00:00")));
            sut.Next();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("02/08/2023 00:00:00")));
            sut.Next();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("02/09/2023 00:00:00")));
            sut.Next();
            Assert.That(sut.IsEndOfTime(), Is.False);
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("02/10/2023 00:00:00")));
            sut.Next();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("02/11/2023 00:00:00")));
            sut.Next();
            Assert.That(sut.IsEndOfTime(), Is.True);
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("02/11/2023 00:00:00")));
            Assert.That(sut.Finish.Timestamp, Is.EqualTo(DateTime.MaxValue));
            sut.Next();
        }

        [Test]
        public void Merge_timelines_together_for_Prior()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions());
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/27/2023 01:00:00"), DutyStatus.Driving));
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/27/2023 08:00:00"), DutyStatus.Sleeper));
            sut.Add(new GpsMoment(DateTime.Parse("01/27/2023 23:00:00"), (long)0.0, (long)0.0));
            sut.Add(new EngineBusMoment(DateTime.Parse("01/27/2023 23:00:00"), 1000));
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/28/2023 01:00:00"), DutyStatus.OnDuty));
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/29/2023 08:00:00"), DutyStatus.Driving));
            sut.Initialize();

            sut.JumpTo(DateTime.Parse("02/13/2023 00:00:00"));
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("02/11/2023 00:00:00")));
            Assert.That(sut.Finish.Timestamp, Is.EqualTo(DateTime.MaxValue));
            Assert.That(sut.Length, Is.GreaterThan(TimeSpan.FromDays(999)));
            Assert.That(sut.IsEndOfTime(), Is.True);
            Assert.That(sut.IsStartOfDay, Is.True);
            sut.Prior();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("02/10/2023 00:00:00")));
            Assert.That(sut.Finish.Timestamp, Is.EqualTo(DateTime.Parse("02/11/2023 00:00:00")));
            Assert.That(sut.Length, Is.EqualTo(TimeSpan.FromHours(24)));
            Assert.That(sut.IsEndOfTime(), Is.False);
            sut.Prior();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("02/09/2023 00:00:00")));
            sut.Prior();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("02/08/2023 00:00:00")));
            sut.Prior();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("02/07/2023 00:00:00")));
            sut.Prior();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("02/06/2023 00:00:00")));
            sut.Prior();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("02/05/2023 00:00:00")));
            sut.Prior();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("02/04/2023 00:00:00")));
            sut.Prior();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("02/03/2023 00:00:00")));
            sut.Prior();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("02/02/2023 00:00:00")));
            sut.Prior();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("02/01/2023 00:00:00")));
            sut.Prior();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("01/31/2023 00:00:00")));
            sut.Prior();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("01/30/2023 00:00:00")));
            sut.Prior();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("01/29/2023 08:00:00")));
            sut.Prior();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("01/29/2023 00:00:00")));
            sut.Prior();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("01/28/2023 01:00:00")));
            sut.Prior();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("01/28/2023 00:00:00")));
            sut.Prior();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 23:00:00")));
            sut.Prior();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 08:00:00")));
            sut.Prior();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 01:00:00")));
            sut.Prior();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 00:00:00")));
            sut.Prior();
            Assert.That(sut.IsBeginningOfTime(), Is.True);
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.MinValue));
            sut.Prior();
            Assert.That(sut.IsBeginningOfTime(), Is.True);
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.MinValue));
        }

        [Test]
        public void Enumerate_through_all_moments_using_foreach()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions());
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/27/2023 01:00:00"), DutyStatus.Driving));
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/27/2023 08:00:00"), DutyStatus.Sleeper));
            sut.Add(new AnchorMoment(DateTime.Parse("01/27/2023 22:00:00"), "StartAudit"));
            sut.Add(new GpsMoment(DateTime.Parse("01/27/2023 23:00:00"), (long)0.0, (long)0.0));
            sut.Add(new EngineBusMoment(DateTime.Parse("01/27/2023 23:00:00"), 1000));
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/28/2023 01:00:00"), DutyStatus.OnDuty));
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/29/2023 08:00:00"), DutyStatus.Driving));
            sut.Initialize();

            var moments = new List<Moment>();
            foreach (var moment in sut)
            {
                moments.Add(moment);
            }

            Assert.That(moments.Count, Is.EqualTo(23));
            Assert.That(moments[0].Timestamp, Is.EqualTo(DateTime.MinValue));
            Assert.That(moments[1].Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 00:00:00")));
            Assert.That(moments[2].Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 01:00:00")));
            Assert.That(moments[3].Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 08:00:00")));
            Assert.That(moments[4].Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 22:00:00")));
            Assert.That(moments[5].Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 23:00:00")));
            Assert.That(moments[6].Timestamp, Is.EqualTo(DateTime.Parse("01/28/2023 00:00:00")));
            Assert.That(moments[7].Timestamp, Is.EqualTo(DateTime.Parse("01/28/2023 01:00:00")));
            Assert.That(moments[20].Timestamp, Is.EqualTo(DateTime.Parse("02/09/2023 00:00:00")));
            Assert.That(moments[22].Timestamp, Is.EqualTo(DateTime.Parse("02/11/2023 00:00:00")));
        }

        [Test]
        public void Enumerate_through_empty_timeline()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions(null, false));
            sut.Initialize();

            var moments = new List<Moment>();
            foreach (var moment in sut)
            {
                moments.Add(moment);
            }

            Assert.That(moments.Count, Is.EqualTo(1));
            Assert.That(moments[0].Timestamp, Is.EqualTo(DateTime.MinValue)); 
        }

        [Test]
        public void Track_agricultural_exception_enabled_state_using_segment()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions());
            sut.Upsert(new AgriculturalExceptionSegment(
                DateTime.Parse("01/27/2023 08:00:00"),
                DateTime.Parse("01/27/2023 16:00:00")));
            sut.Initialize();

            sut.JumpTo(DateTime.Parse("01/27/2023 07:59:59"));
            Assert.That(sut.IsAgriculturalExceptionEnabled, Is.False);

            sut.JumpTo(DateTime.Parse("01/27/2023 08:00:00"));
            Assert.That(sut.IsAgriculturalExceptionEnabled, Is.True);

            sut.JumpTo(DateTime.Parse("01/27/2023 12:00:00"));
            Assert.That(sut.IsAgriculturalExceptionEnabled, Is.True);

            sut.JumpTo(DateTime.Parse("01/27/2023 16:00:00"));
            Assert.That(sut.IsAgriculturalExceptionEnabled, Is.False);
        }

        [Test]
        public void Track_adverse_conditions_enabled_state_using_segment()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions());
            sut.Upsert(new AdverseConditionsSegment(
                DateTime.Parse("01/27/2023 10:00:00"),
                DateTime.Parse("01/27/2023 18:00:00")));
            sut.Initialize();

            sut.JumpTo(DateTime.Parse("01/27/2023 09:59:59"));
            Assert.That(sut.IsAdverseConditionsEnabled, Is.False);

            sut.JumpTo(DateTime.Parse("01/27/2023 10:00:00"));
            Assert.That(sut.IsAdverseConditionsEnabled, Is.True);

            sut.JumpTo(DateTime.Parse("01/27/2023 14:00:00"));
            Assert.That(sut.IsAdverseConditionsEnabled, Is.True);

            sut.JumpTo(DateTime.Parse("01/27/2023 18:00:00"));
            Assert.That(sut.IsAdverseConditionsEnabled, Is.False);
        }

        [Test]
        public void Track_event_moment_in_timeline_navigation()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions(null, false));
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/27/2023 08:00:00"), DutyStatus.Driving));
            sut.Add(new EventMoment(DateTime.Parse("01/27/2023 10:00:00"), eventCode: 42, comment: "test event"));
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/27/2023 12:00:00"), DutyStatus.OnDuty));
            sut.Initialize();

            sut.JumpTo(DateTime.MinValue);
            sut.Next();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 08:00:00")));
            sut.Next();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 10:00:00")));
            sut.Next();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 12:00:00")));
        }

        [Test]
        public void Enumerate_and_access_moment_properties()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions());
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/27/2023 01:00:00"), DutyStatus.Driving));
            sut.Add(new EngineBusMoment(DateTime.Parse("01/27/2023 02:00:00"), 1500));
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/27/2023 08:00:00"), DutyStatus.Sleeper));
            sut.Initialize();

            var dutyStatusMoments = new List<DutyStatusChangeMoment>();
            foreach (var moment in sut)
            {
                if (moment is DutyStatusChangeMoment dscMoment && dscMoment.Timestamp != DateTime.MinValue)
                {
                    dutyStatusMoments.Add(dscMoment);
                }
            }

            Assert.That(dutyStatusMoments.Count, Is.EqualTo(2));
            Assert.That(dutyStatusMoments[0].CurrentDutyStatus, Is.EqualTo(DutyStatus.Driving));
            Assert.That(dutyStatusMoments[1].CurrentDutyStatus, Is.EqualTo(DutyStatus.Sleeper));
        }
    }
}
