using System.Diagnostics.CodeAnalysis;

namespace Businesses.DataAccess;

/// <summary>
/// Class containing static helper methods
/// </summary>
public static class Helpers
{

    /// <summary>
    /// Helper method used to verify that various things are not null before dereferencing.
    /// </summary>
    public static void ThrowIf([DoesNotReturnIf(true)] bool isNull, string itemName)
    {
        if(isNull) 
        { 
            throw new NullReferenceException($"{itemName} must not be null."); 
        }
    }
}