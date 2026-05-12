using FantasyMerchant.Application.Interfaces;
using FantasyMerchant.Domain.Entities;
using FantasyMerchant.Domain.Enums;
using FantasyMerchant.Domain.Records;

namespace FantasyMerchant.Application.Services;

public class RoadService : IRoadService
{
    private readonly IGraphRepository _repository;

    public RoadService(IGraphRepository repository)
    {
        _repository = repository;
    }

    public async Task<Road> CreateRoadAsync(Id fromCityId, Id toCityId, int goldCost, int dangerLevel, CancellationToken ct = default)
    {
        if (fromCityId == toCityId)
            throw new ArgumentException("Дорога не может соединять город с самим собой");
        if (goldCost < 0 || dangerLevel < 0)
            throw new ArgumentException("Веса не могут быть отрицательными");

        await _repository.GetCityByIdAsync(fromCityId, ct);
        await _repository.GetCityByIdAsync(toCityId, ct);

        var road = new Road
        {
            Id = new Id(Guid.NewGuid()),
            FromCityId = fromCityId,
            ToCityId = toCityId,
            GoldCost = goldCost,
            DangerLevel = dangerLevel,
            LoadMultiplier = 1.0m,
            IsBlocked = false
        };

        return await _repository.CreateRoadAsync(road, ct);
    }

    public async Task<Road> UpdateRoadWeightsAsync(Id id, int? goldCost = null, int? dangerLevel = null, CancellationToken ct = default)
    {
        var road = await _repository.GetRoadByIdAsync(id, ct);
        if (road == null)
            throw new KeyNotFoundException($"Дорога с ID {id} не найдена");

        if (goldCost.HasValue)
        {
            if (goldCost < 0)
                throw new ArgumentException("Золото не может быть отрицательным");
            road.GoldCost = goldCost.Value;
        }

        if (dangerLevel.HasValue)
        {
            if (dangerLevel < 0)
                throw new ArgumentException("Опасность не может быть отрицательной");
            road.DangerLevel = dangerLevel.Value;
        }

        return await _repository.UpdateRoadAsync(road, ct);
    }

    public async Task<Road> BlockRoadAsync(Id id, CancellationToken ct = default)
    {
        var road = await _repository.GetRoadByIdAsync(id, ct);
        if (road == null)
            throw new KeyNotFoundException($"Дорога с ID {id} не найдена");

        road.IsBlocked = true;
        return await _repository.UpdateRoadAsync(road, ct);
    }

    public async Task<Road> UnblockRoadAsync(Id id, CancellationToken ct = default)
    {
        var road = await _repository.GetRoadByIdAsync(id, ct);
        if (road == null)
            throw new KeyNotFoundException($"Дорога с ID {id} не найдена");

        road.IsBlocked = false;
        return await _repository.UpdateRoadAsync(road, ct);
    }

    public Task<decimal> CalculateEffectiveWeightAsync(Id roadId, RouteStrategy strategy, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}