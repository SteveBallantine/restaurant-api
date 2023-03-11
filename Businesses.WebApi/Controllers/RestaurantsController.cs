using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Businesses.DataAccess.Data;
using Businesses.DataAccess;
using Businesses.WebApi.Configuration;

namespace Businesses.WebApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class RestaurantsController : ControllerBase
{
    private readonly ILogger<RestaurantsController> _logger;
    private readonly IBusinessDataService _dataService;
    private readonly WebApiSettings _settings;

    public RestaurantsController(
        ILogger<RestaurantsController> logger,
        IBusinessDataService dataService,
        WebApiSettings settings)
    {
        _logger = logger;
        _dataService = dataService;
        _settings = settings;
    }

    /// <summary>
    /// Get a list of up to 1000 restaurants that match the supplied criteria.
    /// Note that the Authorization header must also be supplied and contain the correct key.
    /// </summary>
    /// <param name="location">The name of the location to search in. (e.g. "New York City")</param>
    /// <param name="term">The search term to use. (e.g. "Sushi")</param>
    /// <returns>A list of up to 1000 matching restraunts.</returns>
    /// <response code="200">Returns the results of the search query.</response>
    /// <response code="401">The expected authorization details were not supplied.</response>    
    /// <response code="500">There was an error when performing the search.</response>    
    [HttpGet]
    [Route("")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ResponseCache(CacheProfileName = Constants.ApiCacheProfileName)]
    public async Task<IList<Business>?> SearchAsync(string location, string term)
    {
        // Check for nulls
        Helpers.ThrowIf(_settings.BusinessType == null, nameof(_settings.BusinessType));

        // TODO - Verify Authorize header set correctly. If not - return 401

        var result = await _dataService.SearchAsync(location, _settings.BusinessType, term);
        if(result == null)
        {
            // Result is null. Failure has been logged, so just return a 500.
            Response.StatusCode = StatusCodes.Status500InternalServerError;
        }
        return result;
    }

    /// <summary>
    /// Get a the details for the specified restaurant.
    /// Note that the Authorization header must also be supplied and contain the correct key.
    /// </summary>
    /// <param name="id">The id of the restaurant to retrieve.</param>
    /// <returns>The details for the requested restaurant.</returns>
    /// <response code="200">Returns the results of the search query.</response>
    /// <response code="401">The expected authorization details were not supplied.</response>    
    /// <response code="500">There was an error when performing the search.</response>  
    [HttpGet]
    [Route("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ResponseCache(CacheProfileName = Constants.ApiCacheProfileName)]    
    public async Task<Business?> GetAsync(string id)
    {
        // Check for nulls
        Helpers.ThrowIf(_settings.BusinessType == null, nameof(_settings.BusinessType));

        // TODO - Verify Authorize header set correctly. If not - return 401

        var result = await _dataService.GetAsync(id);
        if(result == null)
        {
            // Result is null. Failure has been logged, so just return a 500.
            Response.StatusCode = StatusCodes.Status500InternalServerError;
        }
        return result;
    }
}
