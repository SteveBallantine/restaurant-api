
using Businesses.DataAccess.Configuration;
using Businesses.DataAccess.Data;

namespace Businesses.DataAccess;

/// <summary>
/// Business data service that uses Yelp as a source
/// </summary>
public class YelpBusinessDataService : IBusinessDataService
{
    private readonly HttpClient _httpClient;
    private readonly YelpSettings _settings;

    public YelpBusinessDataService(HttpClient httpClient, YelpSettings settings) 
    {
        _httpClient = httpClient;
        _settings = settings;
    }

    public Task<IList<Business>> SearchAsync(string location, string businessType, string term)
    {
        throw new NotImplementedException();
    }

    public Task<Business> GetAsync(int id)
    {
        throw new NotImplementedException();
    }
}