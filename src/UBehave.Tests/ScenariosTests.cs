using AwesomeAssertions;
using NSubstitute;
using Xunit.Sdk;

namespace UBehave.Tests;

public class ScenariosTests
{
    [Scenarios("Just a simple run")]
    [InlineData(1, 2)]
    [InlineData(42, 1337)]
    public async Task Simple(int expected1, int expected2)
    {
        // Given
        var setup = Substitute.For<Action>();
        await "Given a setup"
            .x(setup);
        setup.Received(1).Invoke();

        // When
        await "When I ask for a value"
              .x(() => expected1)
              .Out(out var value1Out);

        // Then 
        await "Then I got the value"
            .x(() => value1Out.x.Should().Be(expected1));

        // When
        await "When I ask for an async value"
              .x(async () =>
              {
                  await Task.Yield();
                  return expected2;
              })
              .Out(out var value2Out);

        // Then 
        await "Then I got the async value"
            .x(() => value2Out.x.Should().Be(expected2));
    }

    [Scenarios("Scenario still in Todo")]
    [InlineData(1)]
    public async Task Todo(int _)
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
}
