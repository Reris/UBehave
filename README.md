# UBehave

A micro-library for writing **BDD-style tests in C#**. Inspired by [xBehave.net](https://github.com/adamralph/xbehave.net).

Nuget packages:

* [UBehave](https://www.nuget.org/packages/UBehave/) for xUnit
* [NuBehave](https://www.nuget.org/packages/NuBehave/) for NUnit.

The goal it to write behavior tests first, and then implement the code to make the tests pass. Which is mainly useful for integration testing, but you're free to use in anywhere.

## Quickstart

In your xUnit or NUnit-Project, add a reference to `UBehave` or `NuBehave` and write your first test:

```c#

public class Calculator
{
    public int Add(int x, int y) => x + y;
}

public class CalculatorFeature
{
    [Scenario("A user wants to calculate an addition")]
    public async Task Addition()
    {
        // Given
        var calculator = new Calculator();
        var x = 1;
        var y = 2;

        // When
        await $"When I add the numbers {x} + {y} together"
            .x(() => calculator.Add(x, y))
            .Out(out var result);

        // Then
        await "Then the answer is 3"
            .x(() => Assert.Equal(3, result.x));
    }
}
```

## Step by step

In BDD-Style, you write your tests first from a user perspective, and then implement the code step by step to make the tests pass. So you start with simple describing test method:

```c#
    [Scenario("A user wants to request the weather")]
    public async Task Addition()
    {
        // Given
        await "Given a running ASP IntegrationTesting Server"
            .Todo();        

        // When
        await "When I ask the weather for tomorrow"
            .Todo();

        // Then
        await "Then the answer is sunny, 20°C"
            .Todo();
    }
}
```

From here on you can implement the code step by step, and make the tests pass.

Even though not neccessary, UBehave ❤️ to be combinated with [ASP System under test](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests) and [Test Containers](https://github.com/testcontainers) 🎉<br>
And it doesn't matter if you call it 'You Behave' or 'Micro-Behave/µBehave'. It's a bit of both.
