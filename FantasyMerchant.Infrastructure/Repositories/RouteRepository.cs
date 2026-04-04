using FantasyMerchant.Domain.Entities;
using FantasyMerchant.Domain.Repositories;
using FantasyMerchant.Domain.Records;

namespace FantasyMerchant.Infrastructure.Repositories;

public class RouteRepository : IRouteRepository
{
    private readonly List<Route> _routes = new();

    public Task<Route> CreateRouteAsync(Route route, CancellationToken ct = default)
    {
        if (route.Id == null || route.Id.Value == Guid.Empty)
            route.Id = new Id(Guid.NewGuid());
        _routes.Add(route);
        return Task.FromResult(route);
    }

    public Task<Route?> GetRouteByIdAsync(Id id, CancellationToken ct = default)
        => Task.FromResult(_routes.FirstOrDefault(r => r.Id == id));

    public Task<List<Route>> GetRoutesByUserIdAsync(Id userId, CancellationToken ct = default)
        => Task.FromResult(_routes.ToList()); 

    public Task<List<Route>> GetAllRoutesAsync(CancellationToken ct = default)
        => Task.FromResult(_routes.ToList());
}
