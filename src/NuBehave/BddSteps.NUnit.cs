using NUnit.Framework;

namespace NuBehave;

public static partial class BddSteps
{
    public static Action<string> CurrentWriteLine()
    {
        var logger = TestContext.Out ?? throw new LoggerNotInitializedException();
        return logger.WriteLine;
    }

    private static void Skip(string reason)
    {
        Assert.Ignore(reason);
    }

    public class LoggerNotInitializedException() : Exception($"The {nameof(BddSteps)}.{nameof(TestContext)}.{nameof(TestContext.Out)} is not available.");
}
