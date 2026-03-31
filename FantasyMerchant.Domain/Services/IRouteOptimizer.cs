using FantasyMerchant.Domain.Entities;
using FantasyMerchant.Domain.Records;

namespace FantasyMerchant.Domain.Services;

public interface IRouteOptimizer
{
    /// <summary>
    /// Найти оптимальный путь между двумя городами
    /// </summary>
    /// <param name="graph">Граф в виде словаря смежности (Id города -> список дорог)</param>
    /// <param name="startCityId">Город старта</param>
    /// <param name="endCityId">Город назначения</param>
    /// <param name="strategy">Стратегия оптимизации</param>
    /// <returns>Кортеж: (список Id городов в пути, общее золото, общая опасность)</returns>
    Task<(List<Id> path, int totalGold, int totalDanger)> FindShortestPathAsync(
        Dictionary<Id, List<Road>> graph,
        Id startCityId,
        Id endCityId,
        RouteStrategy strategy,
        CancellationToken ct = default);
}