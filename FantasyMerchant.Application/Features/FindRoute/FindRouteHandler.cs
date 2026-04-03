using MediatR;
using FantasyMerchant.Domain.Repositories;
using FantasyMerchant.Domain.Services;
using FantasyMerchant.Domain.Enums;
using FantasyMerchant.Domain.Records;
using System.Collections.Generic;

namespace FantasyMerchant.Application.Features.FindRoute;

public class FindRouteHandler : IRequestHandler<FindRouteQuery, FindRouteResponse>
{
    private readonly IGraphRepository _graphRepository;
    private readonly IRouteOptimizer _routeOptimizer;

    public FindRouteHandler(IGraphRepository graphRepository, IRouteOptimizer routeOptimizer)
    {
        _graphRepository = graphRepository;
        _routeOptimizer = routeOptimizer;
    }

    public async Task<FindRouteResponse> Handle(FindRouteQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var graph = await _graphRepository.GetGraphStructureAsync(cancellationToken);

            var startCity = await _graphRepository.GetCityByIdAsync(request.StartCityId, cancellationToken);
            var endCity = await _graphRepository.GetCityByIdAsync(request.EndCityId, cancellationToken);

            if (startCity == null)
            {
                return new FindRouteResponse(
                    false,
                    new List<Id>(),
                    0,
                    0,
                    0,
                    $"Город старта не найден: {request.StartCityId.Value}"
                );
            }

            if (endCity == null)
            {
                return new FindRouteResponse(
                    false,
                    new List<Id>(),
                    0,
                    0,
                    0,
                    $"Город назначения не найден: {request.EndCityId.Value}"
                );
            }

            var strategy = request.Strategy.ToLower() switch
            {
                "merchant" => RouteStrategy.Merchant,
                "rogue" => RouteStrategy.Rogue,
                "balanced" => RouteStrategy.Balanced,
                _ => RouteStrategy.Merchant
            };

            var (path, totalGold, totalDanger) = await _routeOptimizer.FindShortestPathAsync(
                graph,
                request.StartCityId,
                request.EndCityId,
                strategy,
                cancellationToken
            );

            if (path.Count == 0)
            {
                return new FindRouteResponse(
                    false,
                    new List<Id>(),
                    0,
                    0,
                    0,
                    "Маршрут не найден. Проверьте, что города связаны дорогами."
                );
            }

            return new FindRouteResponse(
                true,
                path,
                totalGold,
                totalDanger,
                path.Count,
                null
            );
        }
        catch (Exception ex)
        {
            return new FindRouteResponse(
                false,
                new List<Id>(),
                0,
                0,
                0,
                $"Ошибка при поиске маршрута: {ex.Message}"
            );
        }
    }
}