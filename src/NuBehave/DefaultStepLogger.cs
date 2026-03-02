namespace NuBehave;

public class DefaultStepLogger(Action<string> writeLine) : IStepLogger
{
    public void Todo(string step)
    {
    }

    public void Note(string step, string? sidenote = null)
    {
        var message = sidenote is null ? $"NOTE: '{step}'" : $"NOTE: '{step}' // {sidenote}";
        writeLine(message);
        this.FinishStep(message);
    }

    public void BeginStep(string step)
    {
        var message = $"STEP START: '{step}'";
        writeLine(message);
    }

    public void SucceededStep(string step, TimeSpan timeSpan)
    {
        var message = $"STEP END: '{step}' took {timeSpan}";
        writeLine(message);
        this.FinishStep(message);
    }

    public void FailedStep(string step, TimeSpan timeSpan, Exception exception)
    {
        var message = $"STEP END: '{step}' took {timeSpan}";
        writeLine(message);
        this.FinishStep(message);
    }

    private void FinishStep(string message)
    {
        writeLine(new string('-', message.Length));
        writeLine("");
    }
}
