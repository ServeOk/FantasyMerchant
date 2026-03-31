using FantasyMerchant.Domain.Entities;
using FantasyMerchant.Domain.Records;

namespace FantasyMerchant.Domain.Repositories;

public interface IGraphRepository
{
    
    Task<City?> GetCityByIdAsync(Id id, CancellationToken ct = default);
    Task<List<City>> GetAllCitiesAsync(CancellationToken ct = default);
    Task<City> CreateCityAsync(City city, CancellationToken ct = default);
    Task<City> UpdateCityAsync(City city, CancellationToken ct = default);

    
    Task<Road?> GetRoadByIdAsync(Id id, CancellationToken ct = default);
    Task<List<Road>> GetAllRoadsAsync(CancellationToken ct = default);
    Task<Road> CreateRoadAsync(Road road, CancellationToken ct = default);
    Task<Road> UpdateRoadAsync(Road road, CancellationToken ct = default);

    
    Task<Dictionary<Id, List<Road>>> GetGraphStructureAsync(CancellationToken ct = default);

   
    Task<Event> CreateEventAsync(Event evt, CancellationToken ct = default);
    Task<Event> UpdateEventAsync(Event evt, CancellationToken ct = default);
}
