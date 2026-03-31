using FantasyMerchant.Domain.Entities;

namespace FantasyMerchant.Domain.Repositories;

public interface IRouteRepository
{
    Task<Route> CreateRouteAsync(Route route, CancellationToken ct = default);
    Task<Route?> GetRouteByIdAsync(int id, CancellationToken ct = default);
    Task<List<Route>> GetRoutesByUserIdAsync(int userId, CancellationToken ct = default);
    Task<List<Route>> GetAllRoutesAsync(CancellationToken ct = default);
}
