using Newtonsoft.Json.Serialization;

namespace Businesses.DataAccess.Data;

/// <summary>
/// Used to handle differences between our property names and the ones used by the Yelp API
/// </summary>
public class YelpContractResolver : DefaultContractResolver
{
    public static readonly YelpContractResolver Instance = new YelpContractResolver();

    // A dictionary of Yelp property name to our property name mappings.
    private static readonly Dictionary<string, string> _customMappings = new Dictionary<string, string>() 
    {
        { "Zip", "zip_code" },
        { "ImageUrl", "url" }
    };

    protected override string ResolvePropertyName(string propertyName)
    {
        // If we have a match in our custom mapping table then use it. Otherwise, use the base class logic.
        return _customMappings.TryGetValue(propertyName, out var result) ? result : base.ResolvePropertyName(propertyName);
    }
}