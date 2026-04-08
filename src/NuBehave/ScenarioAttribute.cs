using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace NuBehave;

[MeansImplicitUse]
[AttributeUsage(AttributeTargets.Method)]
public class ScenarioAttribute(string name) : NUnitAttribute, ISimpleTestBuilder, IApplyToTest, IImplyFixture
{
    private readonly TestAttribute _inner = new() { Description = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Name must be have a value", nameof(name)) : name };

    public void ApplyToTest(Test test)
    {
        this._inner.ApplyToTest(test);
    }

    public TestMethod BuildFrom(IMethodInfo method, Test? suite)
    {
        var test = this._inner.BuildFrom(method, suite);

        return test;
    }

    #region Other Properties

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>The description.</value>
    [DisallowNull]
    public string? Description
    {
        get => this._inner.Description;
        set => this._inner.Description = value;
    }

    /// <summary>
    /// The author of this test
    /// </summary>
    [DisallowNull]
    public string? Author
    {
        get => this._inner.Author;
        set => this._inner.Author = value;
    }

    /// <summary>
    /// The type that this test is testing
    /// </summary>
    [DisallowNull]
    public Type? TestOf
    {
        get => this._inner.TestOf;
        set => this._inner.TestOf = value;
    }

    #endregion
}
