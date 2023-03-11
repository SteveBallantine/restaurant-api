using Businesses.DataAccess;
using Businesses.WebApi.Controllers;
using Businesses.WebApi.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Businesses.WebApi.Tests;

[TestClass]
public class RestaurantsControllerTests
{
    private Mock<ILogger<RestaurantsController>>? _mockLogger;
    private Mock<IBusinessDataService>? _businessDataService;
    private readonly WebApiSettings _settings = new WebApiSettings() {
        ApiKey = "TestKey"
    };

    private RestaurantsController? _controller;

    [TestInitialize]
    public void Init()
    {
        _businessDataService = new Mock<IBusinessDataService>();
        _mockLogger = new Mock<ILogger<RestaurantsController>>();

        _controller = new RestaurantsController(
            _mockLogger.Object,
            _businessDataService.Object,
            _settings);
    }

    [TestMethod]
    public void Search_VerifyResultFormat()
    {
    }
    
    [TestMethod]
    public void Search_ErrorInDataService()
    {
    }

    [TestMethod]
    public void Search_NotAuthorised()
    {
    }

    [TestMethod]
    public void Get_VerifyResultFormat()
    {
    }

    [TestMethod]
    public void Get_ErrorInDataService()
    {
    }

    [TestMethod]
    public void Get_NotAuthorised()
    {
    }
}