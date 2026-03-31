using MediatR;
using FantasyMerchant.Domain.Repositories;
using FantasyMerchant.Domain.Services;
using FantasyMerchant.Domain.Enums;
using FantasyMerchant.Domain.Records;

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
        var graph = await _graphRepository.GetGraphStructureAsync(cancellationToken);

        // Преобразуем строку в enum
        var strategy = request.Strategy.ToLower() switch
        {
            "merchant" => RouteStrategy.Merchant,
            "rogue" => RouteStrategy.Rogue,
            "balanced" => RouteStrategy.Balanced,
            _ => RouteStrategy.Merchant
        };

        // Вызываем оптимизатор
        var (path, totalGold, totalDanger) = await _routeOptimizer.FindShortestPathAsync(
            graph,
            request.StartCityId,
            request.EndCityId,
            strategy,  // ✅ Передаем enum RouteStrategy
            cancellationToken
        );

        return new FindRouteResponse(
            Path: path,
            TotalGold: totalGold,
            TotalDanger: totalDanger,
            TotalSteps: path.Count
        );
    }
}