using Microsoft.AspNetCore.Mvc;
using MediatR;
using FantasyMerchant.Application.Features.FindRoute;
using FantasyMerchant.Domain.Records;
using FantasyMerchant.Web.ViewModels;

namespace FantasyMerchant.Web.Controllers;

public class HomeController : Controller
{
    private readonly IMediator _mediator;

    public HomeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> FindRoute(FindRouteViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Index");
        }

        var startCityId = new Id(Guid.Parse(model.StartCityId));
        var endCityId = new Id(Guid.Parse(model.EndCityId));

        var query = new FindRouteQuery(startCityId, endCityId, model.Strategy);

        var result = await _mediator.Send(query);

        if (!result.Success)
        {
            ModelState.AddModelError("", result.ErrorMessage);
            return View("Index");
        }

        return View("Index", result);
    }
}
