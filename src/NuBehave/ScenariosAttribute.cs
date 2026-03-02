using NUnit.Framework;

namespace NuBehave;

[AttributeUsage(AttributeTargets.Method)]
public class ScenariosAttribute(string name) : NUnitAttribute
{
    public string DisplayName { get; set; } = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Name must be have a value", nameof(name)) : name;
}
