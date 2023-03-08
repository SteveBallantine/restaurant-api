using System.Text.Json.Serialization;

namespace Businesses.DataAccess.Data;

/// <summary>
/// Data class containing details about a business
/// </summary>
public class Business 
{
    public string? Id {get;set;}
    public string? Name {get;set;}
    public Location? Location {get;set;}
    [JsonPropertyName("image_url")]
    public string? ImageUrl {get;set;}
    [JsonPropertyName("review_count")]
    public int? ReviewCount {get;set;}
    public float? Rating {get;set;}
    public string? Phone {get;set;}
    public IList<Category>? Categories {get;set;}
}