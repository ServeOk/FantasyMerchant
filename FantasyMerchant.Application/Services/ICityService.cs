using FantasyMerchant.Domain.Entities;
using FantasyMerchant.Domain.Records;

namespace FantasyMerchant.Application.Services;

public interface ICityService
{
    Task<City> GetCityByIdAsync(Id id, CancellationToken ct = default);
    Task<List<City>> GetAllCitiesAsync(CancellationToken ct = default);
    Task<City> CreateCityAsync(string name, int x, int y, string? description = null, CancellationToken ct = default);
    Task<City> UpdateCityAsync(Id id, string? name = null, int? x = null, int? y = null, string? description = null, CancellationToken ct = default);
}
