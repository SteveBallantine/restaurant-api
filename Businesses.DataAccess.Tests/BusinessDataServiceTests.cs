using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net.Http;

namespace Businesses.DataAccess.Tests;

[TestClass]
public class BusinessDataServiceTests
{
    private Mock<HttpMessageHandler>? _mockHttpHandler;
    private YelpBusinessDataService? _service;

    [TestInitialize]
    public void Init()
    {
        _mockHttpHandler = new Mock<HttpMessageHandler>();
        _service = new YelpBusinessDataService(new HttpClient(_mockHttpHandler.Object));
    }

    [TestMethod]
    public void Search()
    {
    }

    [TestMethod]
    public void Search_NoResults()
    {
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