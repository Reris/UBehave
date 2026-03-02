using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

#pragma warning disable IDE1006
#pragma warning disable IDE0079

namespace NuBehave;

public static partial class BddSteps
{
    private static readonly AsyncLocal<IStepLogger?> AsyncLocalStepLogger = new();

    /// <summary>
    /// Executed an integration test step.
    /// </summary>
    /// <param name="step">Prose-Description of the processed step.</param>
    /// <param name="test">Function which runs the processed step.</param>
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "BDD Exclusive")]
    public static async ValueTask<BddStep> x(this string step, Func<ValueTask> test)
    {
        _ = step ?? throw new ArgumentNullException(nameof(step));
        _ = test ?? throw new ArgumentNullException(nameof(test));

        var logger = BddSteps.GetStepLoggerOrDefault();
        logger.BeginStep(step);
        var timestamp = Stopwatch.GetTimestamp();
        try
        {
            await test();
        }
        catch (Exception e)
        {
            logger.FailedStep(step, BddSteps.Elapsed(timestamp), e);
            throw;
        }

        logger.SucceededStep(step, BddSteps.Elapsed(timestamp));

        return new BddStep(step);
    }

    /// <summary>
    /// Executed an integration test step.
    /// </summary>
    /// <param name="step">Prose-Description of the processed step.</param>
    /// <param name="test">Function which runs the processed step.</param>
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "BDD Exclusive")]
    public static ValueTask<BddStep> x(this string step, Action test)
    {
        _ = step ?? throw new ArgumentNullException(nameof(step));
        _ = test ?? throw new ArgumentNullException(nameof(test));

        var logger = BddSteps.GetStepLoggerOrDefault();
        logger.BeginStep(step);
        var timestamp = Stopwatch.GetTimestamp();
        try
        {
            test();
        }
        catch (Exception e)
        {
            logger.FailedStep(step, BddSteps.Elapsed(timestamp), e);
            throw;
        }

        logger.SucceededStep(step, BddSteps.Elapsed(timestamp));

        return new ValueTask<BddStep>(new BddStep(step));
    }

    /// <summary>
    /// Executed an integration test step.
    /// </summary>
    /// <param name="step">Prose-Description of the processed step.</param>
    /// <param name="test">Function which runs the processed step.</param>
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "BDD Exclusive")]
    public static async ValueTask<BddStep<T>> x<T>(this string step, Func<ValueTask<T>> test)
    {
        _ = step ?? throw new ArgumentNullException(nameof(step));
        _ = test ?? throw new ArgumentNullException(nameof(test));

        var logger = BddSteps.GetStepLoggerOrDefault();
        logger.BeginStep(step);
        var timestamp = Stopwatch.GetTimestamp();
        T result;
        try
        {
            result = await test();
        }
        catch (Exception e)
        {
            logger.FailedStep(step, BddSteps.Elapsed(timestamp), e);
            throw;
        }

        logger.SucceededStep(step, BddSteps.Elapsed(timestamp));

        return new BddStep<T>(step, result);
    }

    /// <summary>
    /// Executed an integration test step.
    /// </summary>
    /// <param name="step">Prose-Description of the processed step.</param>
    /// <param name="test">Function which runs the processed step.</param>
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "BDD Exclusive")]
    public static ValueTask<BddStep<T>> x<T>(this string step, Func<T> test)
    {
        _ = step ?? throw new ArgumentNullException(nameof(step));
        _ = test ?? throw new ArgumentNullException(nameof(test));

        var logger = BddSteps.GetStepLoggerOrDefault();
        logger.BeginStep(step);
        var timestamp = Stopwatch.GetTimestamp();
        T result;
        try
        {
            result = test();
        }
        catch (Exception e)
        {
            logger.FailedStep(step, BddSteps.Elapsed(timestamp), e);
            throw;
        }

        logger.SucceededStep(step, BddSteps.Elapsed(timestamp));

        return new ValueTask<BddStep<T>>(new BddStep<T>(step, result));
    }

    /// <summary>
    /// Gets the returned value from a test step.
    /// </summary>
    public static ValueTask<BddStep<T>> Out<T>(this ValueTask<BddStep<T>> step, out IBddOut<T> value)
    {
        var bddOut = new BddOut<T>();
        value = bddOut;
        return Complete(step, bddOut);

        static async ValueTask<BddStep<T>> Complete(ValueTask<BddStep<T>> task, BddOut<T> bo)
        {
            var result = await task;
            bo.x = result.Result;
            return result;
        }
    }

    /// <summary>
    /// Marks a step as 'To do'.
    /// So it is a process that still needs to be completed before the feature is complete.
    /// </summary>
    /// <param name="step">Prose-Description of the processed step.</param>
    public static ValueTask<BddStep> Todo(this string step)
    {
        _ = step ?? throw new ArgumentNullException(nameof(step));

        var logger = BddSteps.GetStepLoggerOrDefault();
        logger.Todo(step);
        BddSteps.Skip("TODO: " + step);
        return new ValueTask<BddStep>(new BddStep(step));
    }

    /// <summary>
    /// Marks a step as 'Note'.
    /// It is therefore used solely for the correct formulation of the process.
    /// </summary>
    /// <param name="step">Prose-Description of the processed step.</param>
    /// <param name="sidenote">Additional note comment.</param>
    public static ValueTask<BddStep> Note(this string step, string? sidenote = null)
    {
        _ = step ?? throw new ArgumentNullException(nameof(step));

        var logger = BddSteps.GetStepLoggerOrDefault();
        logger.Note(step, sidenote);

        return new ValueTask<BddStep>(new BddStep(step));
    }

    private static IStepLogger GetStepLoggerOrDefault()
    {
        return BddSteps.AsyncLocalStepLogger.Value ??= new DefaultStepLogger(BddSteps.CurrentWriteLine());
    }

    public static void SetStepLogger(IStepLogger logger)
    {
        BddSteps.AsyncLocalStepLogger.Value = logger;
    }


    private static TimeSpan Elapsed(long timestamp)
    {
#if NETSTANDARD2_0
        return new TimeSpan((Stopwatch.GetTimestamp() - timestamp) * Stopwatch.Frequency);
#else
        return Stopwatch.GetElapsedTime(timestamp);
#endif
    }

    private record BddOut<T> : IBddOut<T>
    {
        [SuppressMessage("ReSharper", "MemberHidesStaticFromOuterClass", Justification = "BDD Exclusive")]
        public T x { get; set; } = default!;
    }
}
