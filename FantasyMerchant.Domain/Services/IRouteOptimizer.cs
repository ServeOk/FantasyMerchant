using FantasyMerchant.Domain.Entities;
using FantasyMerchant.Domain.Records;

namespace FantasyMerchant.Domain.Services;

public interface IRouteOptimizer
{
    
    Task<(List<Id> path, int totalGold, int totalDanger)> FindShortestPathAsync(
        Dictionary<Id, List<Road>> graph,
        Id startCityId,
        Id endCityId,
        RouteStrategy strategy,
        CancellationToken ct = default);
}