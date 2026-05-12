using FantasyMerchant.Domain.Entities;
using FantasyMerchant.Domain.Enums;
using FantasyMerchant.Domain.Records;

namespace FantasyMerchant.Application.Services;

public interface IRoadService
{
    Task<Road> CreateRoadAsync(Id fromCityId, Id toCityId, int goldCost, int dangerLevel, CancellationToken ct = default);
    Task<Road> UpdateRoadWeightsAsync(Id id, int? goldCost = null, int? dangerLevel = null, CancellationToken ct = default);
    Task<Road> BlockRoadAsync(Id id, CancellationToken ct = default);
    Task<Road> UnblockRoadAsync(Id id, CancellationToken ct = default);
    Task<decimal> CalculateEffectiveWeightAsync(Id roadId, RouteStrategy strategy, CancellationToken ct = default);
}
