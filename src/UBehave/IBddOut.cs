using System.Diagnostics.CodeAnalysis;

#pragma warning disable IDE0079
#pragma warning disable IDE1006

namespace UBehave;

/// <summary>
/// Result carrier um ein 'out' auch bei asynchronen Methoden nutzen zu können.
/// </summary>
/// <remarks>
/// Das geschieht üner den kleinen Trick, einen ReferenceType mit dem Wert zu nutzen, da dieser erst später gefüllt
/// werden kann.
/// </remarks>
[SuppressMessage("ReSharper", "InconsistentNaming", Justification = "BDD Exclusive")]
public interface IBddOut<out T>
{
    /// <summary>
    /// Resultat des ausgeführten Steps.
    /// </summary>
    T x { get; }
}
