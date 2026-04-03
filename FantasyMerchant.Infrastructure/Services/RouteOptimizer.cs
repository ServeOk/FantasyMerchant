using FantasyMerchant.Domain.Entities;
using FantasyMerchant.Domain.Enums;
using FantasyMerchant.Domain.Records;
using FantasyMerchant.Domain.Services;

namespace FantasyMerchant.Infrastructure.Services;

public class RouteOptimizer : IRouteOptimizer
{
    public Task<(List<Id> path, int totalGold, int totalDanger)> FindShortestPathAsync(
        Dictionary<Id, List<Road>> graph,
        Id startCityId,
        Id endCityId,
        RouteStrategy strategy,
        CancellationToken ct = default)
    {
        // Реализация алгоритма Дейкстры
        var distances = new Dictionary<Id, int>();
        var previous = new Dictionary<Id, Id?>();
        var unvisited = new HashSet<Id>();
        var totalGold = 0;
        var totalDanger = 0;

        // Инициализация
        foreach (var city in graph.Keys)
        {
            distances[city] = int.MaxValue;
            unvisited.Add(city);
        }
        distances[startCityId] = 0;

        while (unvisited.Count > 0)
        {
            // Находим город с минимальным расстоянием
            var current = unvisited.OrderBy(c => distances[c]).First();

            if (current == endCityId)
                break;

            unvisited.Remove(current);

            // Обновляем расстояния до соседей
            if (graph.TryGetValue(current, out var roads))
            {
                foreach (var road in roads)
                {
                    if (!unvisited.Contains(road.ToCityId))
                        continue;

                    var weight = road.GetEffectiveWeight(strategy);
                    var altDistance = distances[current] + weight;

                    if (altDistance < distances[road.ToCityId])
                    {
                        distances[road.ToCityId] = altDistance;
                        previous[road.ToCityId] = current;
                    }
                }
            }
        }

        // Восстанавливаем путь
        var path = new List<Id>();
        var currentCity = endCityId;

        while (currentCity != null && currentCity != startCityId)
        {
            path.Insert(0, currentCity);
            currentCity = previous.TryGetValue(currentCity, out var prev) ? prev : null;
        }
        path.Insert(0, startCityId);

        // Считаем общую стоимость
        for (int i = 0; i < path.Count - 1; i++)
        {
            var from = path[i];
            var to = path[i + 1];

            if (graph.TryGetValue(from, out var roads))
            {
                var road = roads.FirstOrDefault(r => r.ToCityId == to);
                if (road != null)
                {
                    totalGold += road.GoldCost;
                    totalDanger += road.DangerLevel;
                }
            }
        }

        return Task.FromResult((path, totalGold, totalDanger));
    }
}
