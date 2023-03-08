namespace Businesses.DataAccess.Data;

/// <summary>
/// Data class containing details about categories to which <see cref="Business"/> instances can be linked
/// </summary>
public class Category 
{
    public string? Alias {get;set;}
    public string? Title {get;set;}    
}