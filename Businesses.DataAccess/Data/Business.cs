using Newtonsoft.Json;

namespace Businesses.DataAccess.Data;

/// <summary>
/// Data class containing details about a business
/// </summary>
public class Business 
{
    public string? Id {get;set;}
    public string? Name {get;set;}
    public Location? Location {get;set;}
    [JsonProperty("image_url")]
    public string? ImageUrl {get;set;}
    [JsonProperty("review_count")]
    public int? ReviewCount {get;set;}
    public float? Rating {get;set;}
    public string? Phone {get;set;}
    public IList<Category>? Categories {get;set;}
}