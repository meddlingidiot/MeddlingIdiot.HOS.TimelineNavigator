using Automation.HOS.TimelineNavigator.Moments;
using Automation.HOS.TimelineNavigator.Timelines;
using NUnit.Framework;
using System;

namespace Automation.HOS.TimelineNavigator.UnitTests
{
    [TestFixture]
    public class GetClosestAfterReproTest
    {
        [Test]
        public void GetClosestAfter_ShouldNotReturnMinValue_WhenNoMomentsAfterSearchTimestamp()
        {
            // Arrange
            var options = new StartOfDayTimelineOptions(stopAtStartOfDay: false);
            var navigator = new TimelineNavigator(options);
            var searchTime = new DateTime(2024, 1, 1, 12, 0, 0);
            
            // Add a moment in the past
            navigator.Add(new DutyStatusChangeMoment(searchTime.AddHours(-1), DutyStatus.OffDuty));
            navigator.Initialize();
            
            // We want to jump to the moment we added
            navigator.JumpTo(searchTime.AddHours(-1));
            
            // At this point Start should be the moment we added (11:00).
            // GetClosestAfter(11:00) should find nothing after it.
            
            // Assert
            Assert.That(navigator.Finish.Timestamp, Is.EqualTo(DateTime.MaxValue), "Finish should be MaxValue when no moments are after Start");
        }

        [Test]
        public void GetClosestAfter_ShouldOnlyReturnMomentsAfterSearchTimestamp()
        {
            // Arrange
            var options = new StartOfDayTimelineOptions(stopAtStartOfDay: false);
            var navigator = new TimelineNavigator(options);
            var baseTime = new DateTime(2024, 1, 1, 12, 0, 0);
            
            // All timelines have DateTime.MinValue by default.
            // We add a moment at baseTime.
            navigator.Add(new DutyStatusChangeMoment(baseTime, DutyStatus.OnDuty));
            navigator.Initialize();
            
            // Jump to baseTime
            navigator.JumpTo(baseTime);
            
            // Start is now baseTime.
            // GetClosestAfter(baseTime) is called to set Finish.
            // It should NOT return DateTime.MinValue.
            
            Assert.That(navigator.Finish.Timestamp, Is.EqualTo(DateTime.MaxValue), "Finish should be MaxValue if there are no moments after baseTime");
        }
    }
}
