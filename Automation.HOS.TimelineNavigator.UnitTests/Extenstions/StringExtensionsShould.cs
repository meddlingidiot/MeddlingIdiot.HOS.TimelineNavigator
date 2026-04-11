using NUnit.Framework;
using Automation.HOS.TimelineNavigator.Extensions;

namespace Automation.HOS.TimelineNavigator.UnitTests.Extenstions;

[TestFixture]
public class StringExtensionsShould
{
    // ── ToDateTimeOffsetOrNull ────────────────────────────────────────────────

    [Test]
    public void ToDateTimeOffsetOrNull_ValidString_ReturnsParsedValue()
    {
        var result = "2023-01-27T08:00:00+00:00".ToDateTimeOffsetOrNull();
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Value.Year, Is.EqualTo(2023));
    }

    [Test]
    public void ToDateTimeOffsetOrNull_InvalidString_ReturnsNull()
    {
        var result = "not-a-date".ToDateTimeOffsetOrNull();
        Assert.That(result, Is.Null);
    }

    // ── ToDateTimeOrNull ──────────────────────────────────────────────────────

    [Test]
    public void ToDateTimeOrNull_ValidString_ReturnsParsedValue()
    {
        var result = "01/27/2023 08:00:00".ToDateTimeOrNull();
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Value, Is.EqualTo(DateTime.Parse("01/27/2023 08:00:00")));
    }

    [Test]
    public void ToDateTimeOrNull_InvalidString_ReturnsNull()
    {
        var result = "not-a-date".ToDateTimeOrNull();
        Assert.That(result, Is.Null);
    }

    // ── ToDoubleOrNull ────────────────────────────────────────────────────────

    [Test]
    public void ToDoubleOrNull_ValidString_ReturnsParsedValue()
    {
        var result = "3.14".ToDoubleOrNull();
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Value, Is.EqualTo(3.14).Within(0.0001));
    }

    [Test]
    public void ToDoubleOrNull_InvalidString_ReturnsNull()
    {
        var result = "not-a-number".ToDoubleOrNull();
        Assert.That(result, Is.Null);
    }

    // ── ToIntOrNull ───────────────────────────────────────────────────────────

    [Test]
    public void ToIntOrNull_ValidString_ReturnsParsedValue()
    {
        var result = "42".ToIntOrNull();
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Value, Is.EqualTo(42));
    }

    [Test]
    public void ToIntOrNull_InvalidString_ReturnsNull()
    {
        var result = "not-an-int".ToIntOrNull();
        Assert.That(result, Is.Null);
    }

    // ── ToIntOrDefault ────────────────────────────────────────────────────────

    [Test]
    public void ToIntOrDefault_ValidString_ReturnsParsedValue()
    {
        var result = "7".ToIntOrDefault(defaultValue: 99);
        Assert.That(result, Is.EqualTo(7));
    }

    [Test]
    public void ToIntOrDefault_InvalidString_ReturnsDefault()
    {
        var result = "not-an-int".ToIntOrDefault(defaultValue: 99);
        Assert.That(result, Is.EqualTo(99));
    }

    // ── StripLineEndChars ─────────────────────────────────────────────────────

    [Test]
    public void StripLineEndChars_Null_ReturnsNull()
    {
        var result = ((string?)null).StripLineEndChars();
        Assert.That(result, Is.Null);
    }

    [Test]
    public void StripLineEndChars_StringWithNewlines_RemovesNewlines()
    {
        var result = "line1\nline2\r\nline3".StripLineEndChars();
        Assert.That(result, Is.EqualTo("line1line2line3"));
    }

    [Test]
    public void StripLineEndChars_StringWithTabs_RemovesTabs()
    {
        var result = "col1\tcol2".StripLineEndChars();
        Assert.That(result, Is.EqualTo("col1col2"));
    }

    [Test]
    public void StripLineEndChars_StringWithNoSpecialChars_ReturnsUnchanged()
    {
        var result = "hello world".StripLineEndChars();
        Assert.That(result, Is.EqualTo("hello world"));
    }
}
