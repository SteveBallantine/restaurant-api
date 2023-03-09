using System.Collections.Generic;
using Businesses.DataAccess.Data;

namespace Businesses.DataAccess;

/// <summary>
/// Represents a service that can return details about businesses.
/// </summary>
public interface IBusinessDataService {
    /// <summary>
    /// Get all businesses that match the specified search parameters.
    /// </summary>
    /// <param name="location">
    /// The name of the location to search in. (e.g. "New York City")
    /// </param>
    /// <param name="businessType">
    /// The business type to search for. (e.g. "Restaurants")
    /// </param>
    /// <param name="term">
    /// The search term to use. (e.g. "Sushi")
    /// </param>
    /// <returns>
    /// A collection of <see cref="Business"> instances that match the supplied parameters.
    /// If there is no match then the enumeration will contain no elements.
    /// </returns>
    public Task<IList<Business>> SearchAsync(string location, string businessType, string term);

    /// <summary>
    /// Get the details for the specified business 
    /// </summary>
    /// <param name="id">
    /// The Id of the business to get details for
    /// </param>
    /// <returns>
    /// The matching <see cref="Business"> instance or null if there is no match.
    /// </returns>
    public Task<Business> GetAsync(string id);
}