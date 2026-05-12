using FantasyMerchant.Application.Interfaces;
using FantasyMerchant.Domain.Entities;
using FantasyMerchant.Domain.Records;

namespace FantasyMerchant.Application.Services;

public class CityService : ICityService  
{
    private readonly IGraphRepository _repository;

    public CityService(IGraphRepository repository)
    {
        _repository = repository;
    }

    public async Task<City> GetCityByIdAsync(Id id, CancellationToken ct = default)
    {
        var city = await _repository.GetCityByIdAsync(id, ct);
        if (city == null)
            throw new KeyNotFoundException($"Город с ID {id} не найден");
        return city;
    }

    public async Task<List<City>> GetAllCitiesAsync(CancellationToken ct = default)
    {
        return await _repository.GetAllCitiesAsync(ct);
    }

    public async Task<City> CreateCityAsync(string name, int x, int y, string? description = null, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Название города обязательно", nameof(name));

        var city = new City
        {
            Id = new Id(Guid.NewGuid()),
            Name = name,
            X = x,
            Y = y,
            Description = description
        };

        return await _repository.CreateCityAsync(city, ct);
    }

    public async Task<City> UpdateCityAsync(Id id, string? name = null, int? x = null, int? y = null, string? description = null, CancellationToken ct = default)
    {
        var city = await GetCityByIdAsync(id, ct);

        if (!string.IsNullOrWhiteSpace(name))
            city.Name = name;
        if (x.HasValue)
            city.X = x.Value;
        if (y.HasValue)
            city.Y = y.Value;
        if (description != null)
            city.Description = description;

        return await _repository.UpdateCityAsync(city, ct);
    }
}
