using FantasyMerchant.Domain.Entities;
using FantasyMerchant.Domain.Enums;
using FantasyMerchant.Domain.Records;

namespace FantasyMerchant.Application.Services;

public class RouteOptimizer : IRouteOptimizer
{
    public async Task<(List<Id> path, int totalGold, int totalDanger)> FindShortestPathAsync(
        Dictionary<Id, List<Road>> graph,
        Id startCityId,
        Id endCityId,
        RouteStrategy strategy,
        CancellationToken ct = default)
    {
        var distances = new Dictionary<Id, int>();
        var previous = new Dictionary<Id, Id?>();
        var unvisited = new HashSet<Id>(graph.Keys);

        foreach (var cityId in graph.Keys)
        {
            distances[cityId] = int.MaxValue;
            previous[cityId] = null;
        }
        distances[startCityId] = 0;

        while (unvisited.Count > 0)
        {
            var current = unvisited.OrderBy(c => distances[c]).FirstOrDefault();

            if (current == null || distances[current] == int.MaxValue)
                break;

            if (current == endCityId)
                break;

            unvisited.Remove(current);

            if (graph.TryGetValue(current, out var roads))
            {
                foreach (var road in roads)
                {
                    if (!unvisited.Contains(road.ToCityId))
                        continue;

                    var weight = CalculateRoadWeight(road, strategy);
                    var altDistance = distances[current] + weight;

                    if (altDistance < distances[road.ToCityId])
                    {
                        distances[road.ToCityId] = altDistance;
                        previous[road.ToCityId] = current;
                    }
                }
            }
        }

        var path = new List<Id>();
        var currentCity = endCityId;

        while (currentCity != null && currentCity != startCityId)
        {
            path.Insert(0, currentCity);
            currentCity = previous.TryGetValue(currentCity, out var prev) ? prev : null;
        }

        if (path.Count == 0 && startCityId != endCityId)
        {
            return (new List<Id>(), 0, 0);
        }

        path.Insert(0, startCityId);

        var totalGold = 0;
        var totalDanger = 0;

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

        return (path, totalGold, totalDanger);
    }

    private static int CalculateRoadWeight(Road road, RouteStrategy strategy)
    {
        if (road.IsBlocked) return int.MaxValue;

        return strategy switch
        {
            RouteStrategy.Merchant => (int)(road.GoldCost * road.LoadMultiplier),
            RouteStrategy.Rogue => (int)(road.DangerLevel * road.LoadMultiplier),
            RouteStrategy.Balanced => (int)((road.GoldCost + road.DangerLevel) * road.LoadMultiplier),
            _ => (int)(road.GoldCost * road.LoadMultiplier)
        };
    }
}
