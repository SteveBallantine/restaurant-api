
using Businesses.DataAccess.Data;

namespace Businesses.DataAccess;

/// <summary>
/// Business data service that uses Yelp as a source
/// </summary>
public class YelpBusinessDataService : IBusinessDataService
{
    private readonly HttpClient _httpClient;

    public YelpBusinessDataService(HttpClient httpClient) 
    {
        _httpClient = httpClient;
    }

    public Task<Business> GetAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Business>> SearchAsync(string location, string businessType, string term)
    {
        throw new NotImplementedException();
    }
}