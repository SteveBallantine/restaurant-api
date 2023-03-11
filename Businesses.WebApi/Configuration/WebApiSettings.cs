namespace Businesses.WebApi.Configuration;

public class WebApiSettings
{
    public string? ApiKey {get;set;}
    public string? BusinessType {get;set;}
    // Default response cache time to 5 minutes.
    public int ResponseCacheSeconds {get;set;} = 300;
}