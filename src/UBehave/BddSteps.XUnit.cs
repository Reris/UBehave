using System;
using Xunit;

namespace UBehave;

public static partial class BddSteps
{
    public static Action<string> CurrentWriteLine()
    {
        var logger = TestContext.Current.TestOutputHelper ?? throw new LoggerNotInitializedException();
        return logger.WriteLine;
    }

    private static void Skip(string reason)
    {
        Assert.Skip(reason);
    }

    public class LoggerNotInitializedException() : Exception($"The {nameof(BddSteps)}.{nameof(TestContext.Current.TestOutputHelper)} is not available.");
}
