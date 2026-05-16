using FantasyMerchant.Application.Interfaces;
using FantasyMerchant.Application.Services;
using FantasyMerchant.Web.Controllers;
using FantasyMerchant.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FantasyMerchant.Tests;

public class HomeControllerTests
{
    private readonly HomeController _controller;
    private readonly IRouteOptimizer _routeOptimizer;
    private readonly IGraphRepository _graphRepository;

    public HomeControllerTests()
    {
        var fakeRepo = new Fakes.FakeGraphRepository();
        fakeRepo.SeedTestData();

        _graphRepository = fakeRepo;
        _routeOptimizer = new RouteOptimizer();
        _controller = new HomeController(_routeOptimizer, _graphRepository);
    }

    [Fact]
    public async Task Index_ReturnsViewWithCities()
    {
        var result = await _controller.Index();

        var viewResult = Assert.IsType<ViewResult>(result);
        var viewModel = Assert.IsType<FindRouteViewModel>(viewResult.Model);
        Assert.NotNull(viewModel.AvailableCities);
        Assert.NotEmpty(viewModel.AvailableCities);
    }

    [Fact]
    public async Task FindRoute_ValidData_ReturnsRoute()
    {
        var model = new FindRouteViewModel
        {
            StartCityId = "00000000-0000-0000-0000-000000000001",
            EndCityId = "00000000-0000-0000-0000-000000000003",
            Strategy = "merchant",
            AvailableCities = new List<CityViewModel>()
        };

        var result = await _controller.FindRoute(model);

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.IsType<FindRouteResponse>(viewResult.Model);
    }

    [Fact]
    public async Task FindRoute_SameCity_ReturnsError()
    {
        var model = new FindRouteViewModel
        {
            StartCityId = "00000000-0000-0000-0000-000000000001",
            EndCityId = "00000000-0000-0000-0000-000000000001",
            Strategy = "merchant",
            AvailableCities = new List<CityViewModel>()
        };

        var result = await _controller.FindRoute(model);

        var viewResult = Assert.IsType<ViewResult>(result);
        var response = Assert.IsType<FindRouteResponse>(viewResult.Model);
        Assert.False(response.Success);
    }
}