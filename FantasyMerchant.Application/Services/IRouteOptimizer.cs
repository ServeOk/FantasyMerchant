using FantasyMerchant.Domain.Entities;
using FantasyMerchant.Domain.Enums;
using FantasyMerchant.Domain.Records;

namespace FantasyMerchant.Application.Services;

public interface IRouteOptimizer
{
    Task<(List<Id> path, int totalGold, int totalDanger)> FindShortestPathAsync(
        Dictionary<Id, List<Road>> graph,
        Id startCityId,
        Id endCityId,
        RouteStrategy strategy,
        CancellationToken ct = default);
}