using NUnit.Framework;
using Automation.HOS.TimelineNavigator.Utilities;

namespace Automation.HOS.TimelineNavigator.UnitTests.Utilities;

[TestFixture]
public class LoggerShould
{
    // ── NullLogger ────────────────────────────────────────────────────────────

    [Test]
    public void NullLogger_InitializeWithCategories_DoesNotThrow()
    {
        var logger = new NullLogger();
        Assert.DoesNotThrow(() => logger.Initialize(new List<string> { LoggerCategories.All }));
    }

    [Test]
    public void NullLogger_Initialize_DoesNotThrow()
    {
        var logger = new NullLogger();
        Assert.DoesNotThrow(() => logger.Initialize());
    }

    [Test]
    public void NullLogger_Debug_DoesNotThrow()
    {
        var logger = new NullLogger();
        Assert.DoesNotThrow(() => logger.Debug(LoggerCategories.General, "msg"));
    }

    [Test]
    public async Task NullLogger_SaveToFileAsync_ReturnsCompletedTask()
    {
        var logger = new NullLogger();
        await logger.SaveToFileAsync("ignored.txt");
        Assert.Pass();
    }

    [Test]
    public void NullLogger_GetResults_ReturnsEmptyString()
    {
        var logger = new NullLogger();
        Assert.That(logger.GetResults(), Is.EqualTo(string.Empty));
    }

    // ── InMemoryLogger ────────────────────────────────────────────────────────

    [Test]
    public void InMemoryLogger_GetResults_IsEmptyBeforeAnyDebugCalls()
    {
        var logger = new InMemoryLogger();
        Assert.That(logger.GetResults(), Is.Empty);
    }

    [Test]
    public void InMemoryLogger_Debug_LogsMessage_WhenNoCategoriesSet()
    {
        var logger = new InMemoryLogger();
        logger.Debug(LoggerCategories.General, "hello");
        Assert.That(logger.GetResults(), Does.Contain("hello"));
    }

    [Test]
    public void InMemoryLogger_Debug_LogsMessage_WhenCategoryMatches()
    {
        var logger = new InMemoryLogger();
        logger.Initialize(new List<string> { LoggerCategories.General });
        logger.Debug(LoggerCategories.General, "matched");
        Assert.That(logger.GetResults(), Does.Contain("matched"));
    }

    [Test]
    public void InMemoryLogger_Debug_LogsMessage_WhenAllCategorySet()
    {
        var logger = new InMemoryLogger();
        logger.Initialize(new List<string> { LoggerCategories.All });
        logger.Debug(LoggerCategories.Rule, "all-category-msg");
        Assert.That(logger.GetResults(), Does.Contain("all-category-msg"));
    }

    [Test]
    public void InMemoryLogger_Debug_DoesNotLog_WhenCategoryDoesNotMatch()
    {
        var logger = new InMemoryLogger();
        logger.Initialize(new List<string> { LoggerCategories.General });
        logger.Debug(LoggerCategories.Rule, "filtered-out");
        Assert.That(logger.GetResults(), Does.Not.Contain("filtered-out"));
    }

    [Test]
    public void InMemoryLogger_Debug_TrimsCategory_BeforeMatching()
    {
        var logger = new InMemoryLogger();
        logger.Initialize(new List<string> { $"  {LoggerCategories.General}  " });
        logger.Debug(LoggerCategories.General, "trimmed-match");
        Assert.That(logger.GetResults(), Does.Contain("trimmed-match"));
    }

    [Test]
    public void InMemoryLogger_Debug_IncludesCategoryAndMessageInLog()
    {
        var logger = new InMemoryLogger();
        logger.Debug(LoggerCategories.Violations, "violation-detail");
        var result = logger.GetResults();
        Assert.That(result, Does.Contain(LoggerCategories.Violations));
        Assert.That(result, Does.Contain("violation-detail"));
    }

    [Test]
    public void InMemoryLogger_Initialize_ClearsLog()
    {
        var logger = new InMemoryLogger();
        logger.Debug(LoggerCategories.General, "before-clear");
        logger.Initialize();
        Assert.That(logger.GetResults(), Is.Empty);
    }

    [Test]
    public void InMemoryLogger_Initialize_ClearsCategories_SoDefaultIsAllAgain()
    {
        var logger = new InMemoryLogger();
        logger.Initialize(new List<string> { LoggerCategories.General });
        logger.Initialize(); // clears categories → default is all
        logger.Debug(LoggerCategories.Rule, "after-reset");
        Assert.That(logger.GetResults(), Does.Contain("after-reset"));
    }

    [Test]
    public void InMemoryLogger_InitializeWithCategories_ReplacesExistingCategories()
    {
        var logger = new InMemoryLogger();
        logger.Initialize(new List<string> { LoggerCategories.General });
        logger.Initialize(new List<string> { LoggerCategories.Rule });
        logger.Debug(LoggerCategories.General, "old-category");
        logger.Debug(LoggerCategories.Rule, "new-category");
        var result = logger.GetResults();
        Assert.That(result, Does.Not.Contain("old-category"));
        Assert.That(result, Does.Contain("new-category"));
    }

    [Test]
    public void InMemoryLogger_ToString_MatchesGetResults()
    {
        var logger = new InMemoryLogger();
        logger.Debug(LoggerCategories.General, "tostring-test");
        Assert.That(logger.ToString(), Is.EqualTo(logger.GetResults()));
    }

    [Test]
    public async Task InMemoryLogger_SaveToFileAsync_WritesContentToFile()
    {
        var logger = new InMemoryLogger();
        logger.Debug(LoggerCategories.General, "file-content");
        var path = Path.GetTempFileName();
        try
        {
            await logger.SaveToFileAsync(path);
            var written = await File.ReadAllTextAsync(path);
            Assert.That(written, Does.Contain("file-content"));
        }
        finally
        {
            File.Delete(path);
        }
    }
}
