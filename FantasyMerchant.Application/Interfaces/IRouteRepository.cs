using FantasyMerchant.Domain.Entities;
using FantasyMerchant.Domain.Records;

namespace FantasyMerchant.Application.Interfaces;

public interface IRouteRepository
{
    Task<Route> CreateRouteAsync(Route route, CancellationToken ct = default);
    Task<Route?> GetRouteByIdAsync(Id id, CancellationToken ct = default);

    Task<List<Route>> GetRoutesByUserIdAsync(Id userId, CancellationToken ct = default);

    Task<List<Route>> GetAllRoutesAsync(CancellationToken ct = default);
}
