namespace NuBehave;

public interface IStepLogger
{
    /// <summary>
    /// Logs a step as 'To do'.
    /// </summary>
    /// <param name="step">Prose-Description of the processed step.</param>
    void Todo(string step);

    /// <summary>
    /// Logs a step as 'Note'.
    /// </summary>
    /// <param name="step">Prose-Description of the processed step.</param>
    /// <param name="sidenote">Additional note comment.</param>
    void Note(string step, string? sidenote = null);

    /// <summary>
    /// Logs a step as beginning
    /// </summary>
    /// <param name="step">Prose-Description of the processing step.</param>
    void BeginStep(string step);

    /// <summary>
    /// Logs a step as ended successfully.
    /// </summary>
    /// <param name="step">Prose-Description of the ended step.</param>
    /// <param name="timeSpan">Time it took to run the step.</param>
    void SucceededStep(string step, TimeSpan timeSpan);

    /// <summary>
    /// Logs a step as ended failing.
    /// </summary>
    /// <param name="step">Prose-Description of the ended step.</param>
    /// <param name="timeSpan">Time it took to run the step.</param>
    /// <param name="exception">The reason of the failed test.</param>
    void FailedStep(string step, TimeSpan timeSpan, Exception exception);
}
