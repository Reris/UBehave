using System;
using Xunit;

namespace UBehave;

[AttributeUsage(AttributeTargets.Method)]
public class ScenariosAttribute : TheoryAttribute
{
    public ScenariosAttribute(string name)
    {
        this.DisplayName = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Name must be have a value", nameof(name)) : name;
    }
}
