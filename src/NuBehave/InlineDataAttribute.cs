using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace NuBehave;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class InlineDataAttribute(params object?[]? arguments) : NUnitAttribute, ITestBuilder, ITestCaseData, IImplyFixture
{
    private readonly TestCaseAttribute _inner = new(arguments);

    public IEnumerable<TestMethod> BuildFrom(IMethodInfo method, Test? suite)
    {
        var scenarios = method.GetCustomAttributes<ScenariosAttribute>(true).FirstOrDefault();
        if (scenarios is not null)
        {
            this.Description ??= scenarios.DisplayName;
        }

        var tests = this._inner.BuildFrom(method, suite);
        foreach (var test in tests)
        {
            yield return test;
        }
    }

    public string? TestName => this._inner.TestName;
    public RunState RunState => this._inner.RunState;
    public object?[] Arguments => this._inner.Arguments;
    public IPropertyBag Properties => this._inner.Properties;
    public object? ExpectedResult => this._inner.ExpectedResult;
    public bool HasExpectedResult => this._inner.HasExpectedResult;

    private class ScenariosMethodInfo(ScenariosAttribute scenario, IMethodInfo method) : IMethodInfo
    {
        public T[] GetCustomAttributes<T>(bool inherit)
            where T : class
        {
            return method.GetCustomAttributes<T>(inherit);
        }

        public bool IsDefined<T>(bool inherit)
            where T : class
        {
            return method.IsDefined<T>(inherit);
        }

        public IParameterInfo[] GetParameters()
        {
            return method.GetParameters();
        }

        public Type[] GetGenericArguments()
        {
            return method.GetGenericArguments();
        }

        public IMethodInfo MakeGenericMethod(params Type[] typeArguments)
        {
            return method.MakeGenericMethod(typeArguments);
        }

        public object? Invoke(object? fixture, params object?[]? args)
        {
            return method.Invoke(fixture, args);
        }

        public ITypeInfo TypeInfo => method.TypeInfo;
        public MethodInfo MethodInfo => method.MethodInfo;
        public string Name => scenario.DisplayName;
        public bool IsAbstract => method.IsAbstract;
        public bool IsPublic => method.IsPublic;
        public bool IsStatic => method.IsStatic;
        public bool ContainsGenericParameters => method.ContainsGenericParameters;
        public bool IsGenericMethod => method.IsGenericMethod;
        public bool IsGenericMethodDefinition => method.IsGenericMethodDefinition;
        public ITypeInfo ReturnType => method.ReturnType;
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

    /// <summary>
    /// Gets or sets the reason for ignoring the test
    /// </summary>
    [DisallowNull]
    public string? Ignore
    {
        get => this._inner.Ignore;
        set => this._inner.Ignore = value;
    }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="NUnit.Framework.TestCaseAttribute" /> is explicit.
    /// </summary>
    /// <value>
    /// <see langword="true" /> if explicit; otherwise, <see langword="false" />.
    /// </value>
    public bool Explicit
    {
        get => this._inner.Explicit;
        set => this._inner.Explicit = value;
    }

    /// <summary>
    /// Gets or sets the reason for not running the test.
    /// </summary>
    /// <value>The reason.</value>
    [DisallowNull]
    public string? Reason
    {
        get => this._inner.Reason;
        set => this._inner.Reason = value;
    }

    /// <summary>
    /// Gets or sets the ignore reason. When set to a non-null
    /// non-empty value, the test is marked as ignored.
    /// </summary>
    /// <value>The ignore reason.</value>
    [DisallowNull]
    public string? IgnoreReason
    {
        get => this._inner.IgnoreReason;
        set => this._inner.IgnoreReason = value;
    }

    /// <summary>
    /// Comma-delimited list of platforms to run the test for
    /// </summary>
    public string? IncludePlatform
    {
        get => this._inner.IncludePlatform;
        set => this._inner.IncludePlatform = value;
    }

    /// <summary>
    /// Comma-delimited list of platforms to not run the test for
    /// </summary>
    public string? ExcludePlatform
    {
        get => this._inner.ExcludePlatform;
        set => this._inner.ExcludePlatform = value;
    }

    /// <summary>
    /// Get or set the type arguments for a generic test method.
    /// If not set explicitly, the generic types will be inferred
    /// based on the test case parameters.
    /// </summary>
    public Type[]? TypeArgs
    {
        get => this._inner.TypeArgs;
        set => this._inner.TypeArgs = value;
    }

    /// <summary>
    /// Gets and sets the category for this test case.
    /// May be a comma-separated list of categories.
    /// </summary>
    [DisallowNull]
    public string? Category
    {
        get => this._inner.Category;
        set => this._inner.Category = value;
    }

    /// <summary>
    /// Gets and sets the ignore until date for this test case.
    /// </summary>
    [StringSyntax(StringSyntaxAttribute.DateTimeFormat)]
    [DisallowNull]
    public string? Until
    {
        get => this._inner.Until;
        set => this._inner.Until = value;
    }

    #endregion
}
