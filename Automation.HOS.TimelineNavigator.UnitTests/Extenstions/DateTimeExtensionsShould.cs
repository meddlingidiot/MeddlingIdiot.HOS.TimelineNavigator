using NUnit.Framework;
using Automation.HOS.TimelineNavigator.Extensions;

namespace Automation.HOS.TimelineNavigator.UnitTests.Extenstions;

[TestFixture]
public class DateTimeExtensionsShould
{
    [Test]
    public void AbsoluteDifference_ReturnsPositiveSpan_WhenStartIsAfterEnd()
    {
        var start = DateTime.Parse("01/27/2023 10:00:00");
        var end   = DateTime.Parse("01/27/2023 08:00:00");

        var result = start.AbsoluteDifference(end);

        Assert.That(result, Is.EqualTo(TimeSpan.FromHours(2)));
    }

    [Test]
    public void AbsoluteDifference_ReturnsPositiveSpan_WhenEndIsAfterStart()
    {
        var start = DateTime.Parse("01/27/2023 08:00:00");
        var end   = DateTime.Parse("01/27/2023 10:00:00");

        var result = start.AbsoluteDifference(end);

        Assert.That(result, Is.EqualTo(TimeSpan.FromHours(2)));
    }

    [Test]
    public void AbsoluteDifference_ReturnsZero_WhenStartAndEndAreEqual()
    {
        var timestamp = DateTime.Parse("01/27/2023 08:00:00");

        var result = timestamp.AbsoluteDifference(timestamp);

        Assert.That(result, Is.EqualTo(TimeSpan.Zero));
    }
}
