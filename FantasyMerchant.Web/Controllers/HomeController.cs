using FantasyMerchant.Application.Interfaces;
using FantasyMerchant.Application.Services;
using FantasyMerchant.Domain.Enums;
using FantasyMerchant.Domain.Records;
using FantasyMerchant.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace FantasyMerchant.Web.Controllers;

public class HomeController : Controller
{
    private readonly IRouteOptimizer _routeOptimizer;
    private readonly IGraphRepository _graphRepository;

    public HomeController(IRouteOptimizer routeOptimizer, IGraphRepository graphRepository)
    {
        _routeOptimizer = routeOptimizer;
        _graphRepository = graphRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var cities = await _graphRepository.GetAllCitiesAsync();
        var viewModel = new FindRouteViewModel
        {
            AvailableCities = cities.Select(c => new CityViewModel
            {
                Id = c.Id.Value.ToString(),
                Name = c.Name
            }).ToList()
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> FindRoute(FindRouteViewModel model)
    {
        var cities = await _graphRepository.GetAllCitiesAsync();
        model.AvailableCities = cities.Select(c => new CityViewModel
        {
            Id = c.Id.Value.ToString(),
            Name = c.Name
        }).ToList();

        if (!ModelState.IsValid)
        {
            return View("Index", model);
        }

        try
        {
            var startCityId = new Id(Guid.Parse(model.StartCityId));
            var endCityId = new Id(Guid.Parse(model.EndCityId));

            if (startCityId == endCityId)
            {
                var errorResponse = new FindRouteResponse(
                    Success: false,
                    Path: new List<Id>(),
                    TotalGold: 0,
                    TotalDanger: 0,
                    TotalSteps: 0,
                    ErrorMessage: "Город старта и назначения должны быть разными"
                );
                return View("Index", errorResponse);
            }

            var strategy = model.Strategy.ToLower() switch
            {
                "merchant" => RouteStrategy.Merchant,
                "rogue" => RouteStrategy.Rogue,
                "balanced" => RouteStrategy.Balanced,
                _ => RouteStrategy.Merchant
            };

            var graph = await _graphRepository.GetGraphStructureAsync();

            var (path, totalGold, totalDanger) = await _routeOptimizer.FindShortestPathAsync(
                graph,
                startCityId,
                endCityId,
                strategy,
                CancellationToken.None
            );

            if (path.Count == 0)
            {
                ModelState.AddModelError("", "Маршрут не найден");
                return View("Index", model);
            }

            var response = new FindRouteResponse(
                Success: true,
                Path: path,
                TotalGold: totalGold,
                TotalDanger: totalDanger,
                TotalSteps: path.Count
            );

            return View("Index", response);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Ошибка: {ex.Message}");
            return View("Index", model);
        }
    }
}