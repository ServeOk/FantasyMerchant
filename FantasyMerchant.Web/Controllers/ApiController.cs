using Microsoft.AspNetCore.Mvc;
using FantasyMerchant.Domain.Repositories;

namespace FantasyMerchant.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GraphController : ControllerBase
{
    private readonly IGraphRepository _graphRepository;

    public GraphController(IGraphRepository graphRepository)
    {
        _graphRepository = graphRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetGraph()
    {
        var cities = await _graphRepository.GetAllCitiesAsync();
        var roads = await _graphRepository.GetAllRoadsAsync();

        return Ok(new
        {
            cities = cities.Select(c => new
            {
                id = c.Id.Value,
                name = c.Name,
                x = c.X,
                y = c.Y,
                description = c.Description
            }),
            roads = roads.Select(r => new
            {
                id = r.Id.Value,
                fromCityId = r.FromCityId.Value,
                toCityId = r.ToCityId.Value,
                goldCost = r.GoldCost,
                dangerLevel = r.DangerLevel,
                loadMultiplier = r.LoadMultiplier,
                isBlocked = r.IsBlocked
            })
        });
    }
}
