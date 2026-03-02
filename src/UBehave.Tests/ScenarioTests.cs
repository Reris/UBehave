using AwesomeAssertions;
using NSubstitute;
using Xunit.Sdk;

namespace UBehave.Tests;

public class ScenarioTests
{
    [Scenario("Just a simple run")]
    public async Task Simple()
    {
        // Given
        var setup = Substitute.For<Action>();
        await "Given a setup"
            .x(setup);
        setup.Received(1).Invoke();

        // When
        await "When I ask for a value"
              .x(() => 1)
              .Out(out var value1Out);

        // Then 
        await "Then I got the value"
            .x(() => value1Out.x.Should().Be(1));

        // When
        await "When I ask for an async value"
              .x(async () =>
              {
                  await Task.Yield();
                  return 2;
              })
              .Out(out var value2Out);

        // Then 
        await "Then I got the async value"
            .x(() => value2Out.x.Should().Be(2));
    }

    [Scenario("Scenario still in Todo")]
    public async Task Todo()
    {
        // Given
        await "Given a system still in development"
            .Note();

        // When
        var skipped = false;
        try
        {
            await "When I'm still in Todo"
                .Todo();
        }
        catch (SkipException)
        {
            skipped = true;
        }

        // Then 
        await "Then the scenario will get skipped"
            .x(() => skipped.Should().BeTrue());
    }

    [Scenario("I want to Customize the Logger")]
    public async Task CustomizeLogger()
    {
        // Given
        var logger = Substitute.For<IStepLogger>();
        BddSteps.SetStepLogger(logger);
        await "Given a system still in development"
            .Note();

        // When
        await "When I give a Note"
            .Note("System state ready");

        // Then 
        await "Then the logger gets the note"
            .x(() => logger.Received(1).Note("When I give a Note", "System state ready"));

        // When
        try
        {
            await "When I have a Todo"
                .Todo();
        }
        catch (SkipException)
        {
        }

        // Then 
        await "Then the logger gets the Todo"
            .x(() => logger.Received(1).Todo("When I have a Todo"));

        // When
        await "When I run a step"
            .x(() => { });

        // Then 
        await "Then the logger gets the step"
            .x(() =>
            {
                logger.Received(1).BeginStep("When I run a step");
                logger.Received(1).SucceededStep("When I run a step", Arg.Is<TimeSpan>(a => a != TimeSpan.Zero));
            });

        // When
        var expectedException = new ArgumentNullException();
        try
        {
            await "When I fail a step"
                .x(() => throw expectedException);
        }
        catch (ArgumentNullException)
        {
        }

        // Then 
        await "Then the logger gets the failed step"
            .x(() =>
            {
                logger.Received(1).BeginStep("When I fail a step");
                logger.Received(1).FailedStep("When I fail a step", Arg.Is<TimeSpan>(a => a != TimeSpan.Zero), expectedException);
            });
    }

    [Scenario("I want to skip a test", Skip = "This test is skipped for demonstration purposes")]
    public Task Ignored()
    {
        Assert.Fail("This test should be ignored and not fail");
        return Task.CompletedTask;
    }

    [Scenario("I want to have explicit tests", Explicit = true)]
    public Task Explicit()
    {
        // Side Note: ReSharper runner seems to ignore explicit calls in xUnit
        return Task.CompletedTask;
    }
}
