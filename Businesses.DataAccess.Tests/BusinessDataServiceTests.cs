using Businesses.DataAccess.Configuration;
using Businesses.DataAccess.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Businesses.DataAccess.Tests;

[TestClass]
public class BusinessDataServiceTests
{
    private Mock<HttpMessageHandler>? _mockHttpHandler;
    private YelpBusinessDataService? _service;
    private readonly YelpSettings _settings = new YelpSettings() 
    {
        ClientId = "abc",
        ApiKey = "xyz"
    };

    [TestInitialize]
    public void Init()
    {
        _mockHttpHandler = new Mock<HttpMessageHandler>();
        _service = new YelpBusinessDataService(new HttpClient(_mockHttpHandler.Object), _settings);
    }

    /// <summary>
    /// Check that the call to the Yelp API contains the expected parameters and authorization details.
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task Search_VerifyYelpRequestContent()
    {
        // Ensure nullable values are not null
        Helpers.ThrowIf(_service == null, nameof(_service));
        Helpers.ThrowIf(_settings.ApiKey == null, nameof(_settings.ApiKey));

        // Configure mock http handler
        var protectedMock = _mockHttpHandler.Protected();
        protectedMock.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).Verifiable();

        // Call the service
        var result = await _service.SearchAsync("searchLocation", "searchType", "searchTerm");
        
        // Verify that the Yelp API was called with the expected parameters and auth header value.
        protectedMock.Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(r => 
                r.Method == HttpMethod.Get &&
                r.Headers.Contains("Authorization") &&
                r.Headers.GetValues("Authorization").Single().Contains(_settings.ApiKey) &&
                r.RequestUri != null &&
                r.RequestUri.Query.Contains("term=searchType") &&
                r.RequestUri.Query.Contains("categories=searchTerm") &&
                r.RequestUri.Query.Contains("location=searchLocation")));
    }

    /// <summary>
    /// Check that the service is correctly mapping fields from the Yelp response to our data objects.
    /// </summary>
    [DataTestMethod]
    public async Task Search_VerifyYelpResponseMapping()
    {        
        // Ensure nullable values are not null
        Helpers.ThrowIf(_service == null, nameof(_service));

        var expectedId = "G0AB4-VN3v_Qd-icr8BfEg";
        var expectedName = "Flint Hills Saloon & Eatery";
        var expectedImageUrl = "https://s3-media3.fl.yelpcdn.com/bphoto/9H35VBXO4DoRT0rJ_COKkA/o.jpg";
        var expectedReviewCount = 4;
        var expectedRating = 5;
        var expectedPhone = "+16207676242";

        // Sample response taken from a real query
        var yelpResponse = $@"{{
  ""businesses"": [
    {{
      ""id"": ""{expectedId}"",
      ""alias"": ""flint-hills-saloon-and-eatery-council-grove"",
      ""name"": ""{expectedName}"",
      ""image_url"": ""{expectedImageUrl}"",
      ""is_closed"": false,
      ""url"": ""https://www.yelp.com/biz/flint-hills-saloon-and-eatery-council-grove?adjust_creative=w2587UeBsR7s-jEaUXF8WA&utm_campaign=yelp_api_v3&utm_medium=api_v3_business_search&utm_source=w2587UeBsR7s-jEaUXF8WA"",
      ""review_count"": {expectedReviewCount},
      ""categories"": [
        {{
          ""alias"": ""comfortfood"",
          ""title"": ""Comfort Food""
        }},
        {{
          ""alias"": ""steak"",
          ""title"": ""Steakhouses""
        }},
        {{
          ""alias"": ""burgers"",
          ""title"": ""Burgers""
        }}
      ],
      ""rating"": {expectedRating},
      ""coordinates"": {{
        ""latitude"": 38.66175,
        ""longitude"": -96.4819
      }},
      ""transactions"": [],
      ""location"": {{
        ""address1"": ""410 E Main St"",
        ""address2"": null,
        ""address3"": null,
        ""city"": ""Council Grove"",
        ""zip_code"": ""66846"",
        ""country"": ""US"",
        ""state"": ""KS"",
        ""display_address"": [
          ""410 E Main St"",
          ""Council Grove, KS 66846""
        ]
      }},
      ""phone"": ""{expectedPhone}"",
      ""display_phone"": ""(620) 767-6242"",
      ""distance"": 48321.743920680514
    }}
  ],
  ""total"": 1,
  ""region"": {{
    ""center"": {{
      ""longitude"": -95.95664978027344,
      ""latitude"": 38.51895913571998
    }}
  }}
}}";

        // Configure mock http handler
        var protectedMock = _mockHttpHandler.Protected();
        protectedMock.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(yelpResponse) });

        // Call the service
        var result = await _service.SearchAsync("searchLocation", "searchType", "searchTerm");

        // Verify result
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(expectedId, result[0].Id);
        Assert.AreEqual(expectedImageUrl, result[0].ImageUrl);
        Assert.AreEqual(expectedName, result[0].Name);
        Assert.AreEqual(expectedPhone, result[0].Phone);
        Assert.AreEqual(expectedRating, result[0].Rating);
        Assert.AreEqual(expectedReviewCount, result[0].ReviewCount);
    }

    /// <summary>
    /// Check that the service is handling various responses from the Yelp API as intended
    /// See https://docs.developer.yelp.com/reference/v3_business_search for possible response codes.
    /// </summary>
    /// <param name="yelpResponseCode">The response code to be returned by the Yelp API</param>
    [DataTestMethod]
    [DataRow(HttpStatusCode.BadRequest)]
    [DataRow(HttpStatusCode.Unauthorized)]
    [DataRow(HttpStatusCode.Forbidden)]
    [DataRow(HttpStatusCode.NotFound)]
    [DataRow(HttpStatusCode.RequestEntityTooLarge)]
    [DataRow(HttpStatusCode.TooManyRequests)]
    [DataRow(HttpStatusCode.InternalServerError)]
    [DataRow(HttpStatusCode.ServiceUnavailable)]
    [ExpectedException(typeof(DataAccessException))]
    public async Task Search_VerifyFailResponseBehavior(HttpStatusCode yelpResponseCode)
    {        
        // Ensure nullable values are not null
        Helpers.ThrowIf(_service == null, nameof(_service));

        // Configure mock http handler
        var protectedMock = _mockHttpHandler.Protected();
        protectedMock.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(yelpResponseCode) { Content = new StringContent("") });

        // Call the service
        var result = await _service.SearchAsync("searchLocation", "searchType", "searchTerm");
    }

    [TestMethod]
    public void Get()
    {
    }
    
    [TestMethod]
    public void Get_NoResult()
    {
    }


}