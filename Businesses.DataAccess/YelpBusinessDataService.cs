
using Businesses.DataAccess.Configuration;
using Businesses.DataAccess.Data;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Businesses.DataAccess;

/// <summary>
/// Business data service that uses Yelp as a source
/// </summary>
public class YelpBusinessDataService : IBusinessDataService
{
    private readonly ILogger<YelpBusinessDataService> _logger;
    private readonly HttpClient _httpClient;
    private readonly YelpSettings _settings;

    // The Yelp data is in a slightly different format, so use a custom resolver to deserialise it.
    private readonly JsonSerializerSettings _serialiserSettings = new JsonSerializerSettings()
    {
        ContractResolver = new YelpContractResolver()
    };

    public YelpBusinessDataService(ILogger<YelpBusinessDataService> logger,
        HttpClient httpClient, YelpSettings settings) 
    {
        _logger = logger;
        _httpClient = httpClient;
        _settings = settings;
    }

    public async Task<IList<Business>?> SearchAsync(string location, string businessType, string term)
    {
        using(HttpRequestMessage message = new HttpRequestMessage() {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{_settings.BaseUrl}/search?" +
                $"{Constants.YelpParameter_Location}={location}&" +
                $"{Constants.YelpParameter_Term}={businessType}&" + 
                $"{Constants.YelpParameter_Categories}={term}")
        })
        {
            AddAuthHeader(message);
            var response = await _httpClient.SendAsync(message);

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    var collection = await HandleResponseAsync<BusinessCollection>(response);
                    return collection?.Businesses;
                default:
                    _logger.LogError($"Call to Yelp API unsuccessful. Status code {response.StatusCode}");
                    return null;
            }
        }
    }

    /// <summary>
    /// Get the details for the specified business 
    /// </summary>
    /// <param name="id">
    /// The Id of the business to get details for
    /// </param>
    /// <returns>
    /// The matching <see cref="Business"> instance or null if there is no match.
    /// </returns>
    /// <exception cref="HttpRequestException">
    /// Thrown if exceptions occur when contacting the remote server
    /// </exception>
    public async Task<Business?> GetAsync(string id)
    {
        using(HttpRequestMessage message = new HttpRequestMessage() {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{_settings.BaseUrl}/{id}")
        })
        {
            AddAuthHeader(message);
            var response = await _httpClient.SendAsync(message);

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    return await HandleResponseAsync<Business>(response);
                default:
                    _logger.LogError($"Call to Yelp API unsuccessful. Status code {response.StatusCode}");
                    return null;
            }
        }
    }

    /// <summary>
    /// Add the authorisation header that is required to access the Yelp API.
    /// </summary>
    /// <param name="message">
    /// The message to add the header to.
    /// </param>
    protected void AddAuthHeader(HttpRequestMessage message)
    {
        message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", _settings.ApiKey);
    }

    private async Task<T?> HandleResponseAsync<T>(HttpResponseMessage response)
        where T : class?
    {                            
        try
        {
            if(response.Content != null)
            {
                // Parse and return result
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content, _serialiserSettings);
            } 
            else 
            {
                // If response content is null, log an error and return null
                _logger.LogError($"Yelp API response is null.");
                return null;
            }

        }
        catch (JsonException ex) 
        {
            // If there is an error deserialising the response then log an error and return null
            _logger.LogError(ex, $"Error parsing Yelp API response.");
            return null;
        }
    }

}