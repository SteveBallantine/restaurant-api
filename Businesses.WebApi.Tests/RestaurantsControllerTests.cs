using Businesses.DataAccess;
using Businesses.WebApi.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Businesses.WebApi.Tests;

[TestClass]
public class RestaurantsControllerTests
{
    private Mock<IBusinessDataService>? _businessDataService;
    private RestaurantsController? _controller;

    [TestInitialize]
    public void Init()
    {
        LoggerFactory factory = new LoggerFactory();
        _businessDataService = new Mock<IBusinessDataService>();

        _controller = new RestaurantsController(
            factory.CreateLogger<RestaurantsController>(),
            _businessDataService.Object);
    }

    [TestMethod]
    public void Search()
    {
    }
    
    [TestMethod]
    public void Search_ErrorInDataService()
    {
    }

    [TestMethod]
    public void Get()
    {
    }

    [TestMethod]
    public void Get_ErrorInDataService()
    {
    }
}