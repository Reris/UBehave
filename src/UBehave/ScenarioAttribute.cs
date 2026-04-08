using System;
using JetBrains.Annotations;
using Xunit;

namespace UBehave;

[MeansImplicitUse]
[AttributeUsage(AttributeTargets.Method)]
public class ScenarioAttribute : FactAttribute
{
    public ScenarioAttribute(string name)
    {
        this.DisplayName = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Name must be have a value", nameof(name)) : name;
    }
}
