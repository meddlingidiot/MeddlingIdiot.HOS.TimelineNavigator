using Automation.HOS.TimelineNavigator.Moments;
using Automation.HOS.TimelineNavigator.Segments;
using Automation.HOS.TimelineNavigator.Timelines;
using Automation.HOS.TimelineNavigator.Utilities;
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

        [Test]
        public void Add_RestMoment_WithNormalTimestamp_AppearsInTimeline()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions(null, false));
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/27/2023 08:00:00"), DutyStatus.OffDuty));
            sut.Add(new RestMoment(DateTime.Parse("01/27/2023 08:00:00"), TimeSpan.FromHours(10)));
            sut.Initialize();

            sut.JumpTo(DateTime.Parse("01/27/2023 08:00:00"));
            Assert.That(sut.CurrentRestMoment.Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 08:00:00")));
        }

        [Test]
        public void Add_RestMoment_WithMinValueTimestamp_RoutesToUpsert()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions(null, false));
            sut.Add(new RestMoment(DateTime.MinValue, TimeSpan.FromHours(10)));
            sut.Initialize();

            sut.JumpTo(DateTime.MinValue);
            Assert.That(sut.CurrentRestMoment.Timestamp, Is.EqualTo(DateTime.MinValue));
            Assert.That(sut.CurrentRestMoment.Duration, Is.EqualTo(TimeSpan.FromHours(10)));
        }

        [Test]
        public void Add_EventMoment_WithMinValueTimestamp_RoutesToUpsert()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions(null, false));
            sut.Add(new EventMoment(DateTime.MinValue, eventCode: 99));
            sut.Initialize();

            sut.JumpTo(DateTime.MinValue);
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.MinValue));
        }

        [Test]
        public void DefaultConstructor_CreatesNavigatorWithMidnightStartOfDay()
        {
            var sut = new TimelineNavigator();
            sut.Initialize();
            Assert.That(sut.IsBeginningOfTime(), Is.True);
        }

        [Test]
        public void Upsert_AnchorMoment_AppearsInTimeline()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions(null, false));
            sut.Upsert(new AnchorMoment(DateTime.Parse("01/27/2023 08:00:00"), "audit"));
            sut.Initialize();

            sut.JumpTo(DateTime.Parse("01/27/2023 08:00:00"));
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 08:00:00")));
        }

        [Test]
        public void Upsert_DutyStatusChangeMoment_AppearsInTimeline()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions(null, false));
            sut.Upsert(new DutyStatusChangeMoment(DateTime.Parse("01/27/2023 08:00:00"), DutyStatus.Driving));
            sut.Initialize();

            sut.JumpTo(DateTime.Parse("01/27/2023 08:00:00"));
            Assert.That(sut.DutyStatus, Is.EqualTo(DutyStatus.Driving));
        }

        [Test]
        public void Upsert_GpsMoment_ExposesLatitudeAndLongitude()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions(null, false));
            sut.Upsert(new GpsMoment(DateTime.Parse("01/27/2023 08:00:00"), 45.5, -93.2));
            sut.Initialize();

            sut.JumpTo(DateTime.Parse("01/27/2023 08:00:00"));
            Assert.That(sut.Latitude, Is.EqualTo(45.5));
            Assert.That(sut.Longitude, Is.EqualTo(-93.2));
            Assert.That(sut.CurrentGpsMoment.Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 08:00:00")));
        }

        [Test]
        public void Upsert_EngineBusMoment_ExposesOdometerAndCurrentMoment()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions(null, false));
            sut.Upsert(new EngineBusMoment(DateTime.Parse("01/27/2023 08:00:00"), 5000));
            sut.Initialize();

            sut.JumpTo(DateTime.Parse("01/27/2023 08:00:00"));
            Assert.That(sut.Odometer, Is.EqualTo(5000));
            Assert.That(sut.CurrentEngineBusMoment.Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 08:00:00")));
        }

        [Test]
        public void Upsert_ShiftExtensionSegment_ExposesIsShiftExtended()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions(null, false));
            sut.Upsert(new ShiftExtensionSegment(
                DateTime.Parse("01/27/2023 08:00:00"),
                DateTime.Parse("01/27/2023 16:00:00")));
            sut.Initialize();

            sut.JumpTo(DateTime.Parse("01/27/2023 08:00:00"));
            Assert.That(sut.IsShiftExtended, Is.True);

            sut.JumpTo(DateTime.Parse("01/27/2023 16:00:00"));
            Assert.That(sut.IsShiftExtended, Is.False);
        }

        [Test]
        public void IsEndOfSleeperSplits_ReturnsFalse_WhenRestMomentsExistAhead()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions(null, false));
            sut.Add(new RestMoment(DateTime.Parse("01/27/2023 08:00:00"), TimeSpan.FromHours(10)));
            sut.Initialize();

            sut.JumpTo(DateTime.MinValue);
            Assert.That(sut.IsEndOfSleeperSplits(), Is.False);
        }

        [Test]
        public void IsEndOfShiftExtensions_ReturnsFalse_WhenShiftExtensionsExistAhead()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions(null, false));
            sut.Upsert(new ShiftExtensionSegment(
                DateTime.Parse("01/27/2023 08:00:00"),
                DateTime.Parse("01/27/2023 16:00:00")));
            sut.Initialize();

            sut.JumpTo(DateTime.MinValue);
            Assert.That(sut.IsEndOfShiftExtensions(), Is.False);
        }

        [Test]
        public void StartOfDay_ReturnsCorrectMidnightForTimestamp()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions());
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/27/2023 08:00:00"), DutyStatus.Driving));
            sut.Initialize();

            var result = sut.StartOfDay(DateTime.Parse("01/27/2023 14:00:00"));
            Assert.That(result, Is.EqualTo(DateTime.Parse("01/27/2023 00:00:00")));
        }

        [Test]
        public void CurrentDay_ReturnsStartOfDayForCurrentPosition()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions());
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/27/2023 08:00:00"), DutyStatus.Driving));
            sut.Initialize();

            sut.JumpTo(DateTime.Parse("01/27/2023 10:00:00"));
            Assert.That(sut.CurrentDay, Is.EqualTo(DateTime.Parse("01/27/2023 00:00:00")));
        }

        [Test]
        public void StartAndFinishTimestamps_AreExposedAsDateTimeProperties()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions(null, false));
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/27/2023 08:00:00"), DutyStatus.Driving));
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/27/2023 12:00:00"), DutyStatus.OnDuty));
            sut.Initialize();

            sut.JumpTo(DateTime.Parse("01/27/2023 08:00:00"));
            Assert.That(sut.StartTimestamp, Is.EqualTo(DateTime.Parse("01/27/2023 08:00:00")));
            Assert.That(sut.FinishTimestamp, Is.EqualTo(DateTime.Parse("01/27/2023 12:00:00")));
        }

        [Test]
        public void DriverIdNumber_And_TruckNumber_ArePopulatedFromMoment()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions(null, false));
            sut.Upsert(new DutyStatusChangeMoment(DateTime.Parse("01/27/2023 08:00:00"), DutyStatus.Driving,
                driverIdNumber: "D123", truckNumber: "T456"));
            sut.Initialize();

            sut.JumpTo(DateTime.Parse("01/27/2023 08:00:00"));
            Assert.That(sut.DriverIdNumber, Is.EqualTo("D123"));
            Assert.That(sut.TruckNumber, Is.EqualTo("T456"));
        }

        [Test]
        public void DumpSplitRestTimeline_LogsEachRestMoment()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions(null, false));
            sut.Add(new RestMoment(DateTime.Parse("01/27/2023 08:00:00"), TimeSpan.FromHours(10)));
            sut.Add(new RestMoment(DateTime.Parse("01/27/2023 20:00:00"), TimeSpan.FromHours(2)));
            sut.Initialize();

            var logger = new InMemoryLogger();
            logger.Initialize();
            sut.DumpSplitRestTimeline(logger);

            var log = logger.GetResults();
            Assert.That(log, Does.Contain("SplitRestMoment"));
        }

        [Test]
        public void NonGenericGetEnumerator_IteratesAllMoments()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions(null, false));
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/27/2023 08:00:00"), DutyStatus.Driving));
            sut.Initialize();

            var moments = new List<object>();
            System.Collections.IEnumerable nonGeneric = sut;
            foreach (var moment in nonGeneric)
            {
                moments.Add(moment);
            }

            Assert.That(moments, Is.Not.Empty);
        }

        [Test]
        public void JumpToPriorRest_WithNullPaired_MovesToPriorRest()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions(null, false));
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/27/2023 08:00:00"), DutyStatus.OffDuty));
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/27/2023 20:00:00"), DutyStatus.Sleeper));
            sut.Add(new RestMoment(DateTime.Parse("01/27/2023 08:00:00"), TimeSpan.FromHours(10)));
            sut.Add(new RestMoment(DateTime.Parse("01/27/2023 20:00:00"), TimeSpan.FromHours(2)));
            sut.Initialize();

            sut.JumpToNextRest();
            sut.JumpToNextRest();
            sut.JumpToPriorRest();
            Assert.That(sut.CurrentRestMoment.Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 08:00:00")));
        }

        [Test]
        public void JumpToPriorRest_WithPairedFilter_MovesToMatchingRest()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions(null, false));
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/27/2023 08:00:00"), DutyStatus.OffDuty));
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/27/2023 20:00:00"), DutyStatus.Sleeper));
            sut.Add(new RestMoment(DateTime.Parse("01/27/2023 08:00:00"), TimeSpan.FromHours(10), isPaired: false));
            sut.Add(new RestMoment(DateTime.Parse("01/27/2023 20:00:00"), TimeSpan.FromHours(2), isPaired: true));
            sut.Initialize();

            sut.JumpToNextRest();
            sut.JumpToNextRest();
            sut.JumpToPriorRest(paired: false);
            Assert.That(sut.CurrentRestMoment.IsPaired, Is.False);
        }

        [Test]
        public void JumpToNextRest_WithNullPaired_MovesToNextRest()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions(null, false));
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/27/2023 08:00:00"), DutyStatus.OffDuty));
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/27/2023 20:00:00"), DutyStatus.Sleeper));
            sut.Add(new RestMoment(DateTime.Parse("01/27/2023 08:00:00"), TimeSpan.FromHours(10)));
            sut.Add(new RestMoment(DateTime.Parse("01/27/2023 20:00:00"), TimeSpan.FromHours(2)));
            sut.Initialize();

            sut.JumpToNextRest();
            Assert.That(sut.CurrentRestMoment.Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 08:00:00")));

            sut.JumpToNextRest();
            Assert.That(sut.CurrentRestMoment.Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 20:00:00")));
        }

        [Test]
        public void JumpToNextRest_WithPairedFilter_MovesToMatchingRest()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions(null, false));
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/27/2023 08:00:00"), DutyStatus.OffDuty));
            sut.Add(new DutyStatusChangeMoment(DateTime.Parse("01/27/2023 20:00:00"), DutyStatus.Sleeper));
            sut.Add(new RestMoment(DateTime.Parse("01/27/2023 08:00:00"), TimeSpan.FromHours(10), isPaired: false));
            sut.Add(new RestMoment(DateTime.Parse("01/27/2023 20:00:00"), TimeSpan.FromHours(2), isPaired: true));
            sut.Initialize();

            sut.JumpToNextRest(paired: true);
            Assert.That(sut.CurrentRestMoment.IsPaired, Is.True);
        }

        [Test]
        public void JumpToPriorShiftExtension_WithNullFilter_MovesToPriorExtension()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions(null, false));
            sut.Upsert(new ShiftExtensionSegment(
                DateTime.Parse("01/27/2023 08:00:00"),
                DateTime.Parse("01/27/2023 16:00:00")));
            sut.Initialize();

            sut.JumpToNextShiftExtension();
            sut.JumpToNextShiftExtension();
            sut.JumpToPriorShiftExtension();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 08:00:00")));
        }

        [Test]
        public void JumpToPriorShiftExtension_WithExtendedFilter_MovesToMatchingExtension()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions(null, false));
            sut.Upsert(new ShiftExtensionSegment(
                DateTime.Parse("01/27/2023 08:00:00"),
                DateTime.Parse("01/27/2023 16:00:00"),
                isExtended: true));
            sut.Initialize();

            sut.JumpToNextShiftExtension();
            sut.JumpToNextShiftExtension();
            sut.JumpToPriorShiftExtension(isExtended: true);
            Assert.That(sut.IsShiftExtended, Is.True);
        }

        [Test]
        public void JumpToNextShiftExtension_WithNullFilter_MovesToNextExtension()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions(null, false));
            sut.Upsert(new ShiftExtensionSegment(
                DateTime.Parse("01/27/2023 08:00:00"),
                DateTime.Parse("01/27/2023 16:00:00")));
            sut.Initialize();

            sut.JumpToNextShiftExtension();
            Assert.That(sut.Start.Timestamp, Is.EqualTo(DateTime.Parse("01/27/2023 08:00:00")));
        }

        [Test]
        public void JumpToNextShiftExtension_WithExtendedFilter_MovesToMatchingExtension()
        {
            var sut = new TimelineNavigator(new StartOfDayTimelineOptions(null, false));
            sut.Upsert(new ShiftExtensionSegment(
                DateTime.Parse("01/27/2023 08:00:00"),
                DateTime.Parse("01/27/2023 16:00:00"),
                isExtended: true));
            sut.Initialize();

            sut.JumpToNextShiftExtension(isExtended: true);
            Assert.That(sut.IsShiftExtended, Is.True);
        }
    }
}
