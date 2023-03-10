
using Businesses.DataAccess.Configuration;
using Businesses.DataAccess.Data;
using Microsoft.Extensions.Logging;

namespace Businesses.DataAccess;

/// <summary>
/// Business data service that uses Yelp as a source
/// </summary>
public class YelpBusinessDataService : IBusinessDataService
{
    private readonly ILogger<YelpBusinessDataService> _logger;
    private readonly HttpClient _httpClient;
    private readonly YelpSettings _settings;

    public YelpBusinessDataService(ILogger<YelpBusinessDataService> logger,
        HttpClient httpClient, YelpSettings settings) 
    {
        _logger = logger;
        _httpClient = httpClient;
        _settings = settings;
    }

    public Task<IList<Business>> SearchAsync(string location, string businessType, string term)
    {
        throw new NotImplementedException();
    }

    public Task<Business> GetAsync(string id)
    {
        throw new NotImplementedException();
    }
}