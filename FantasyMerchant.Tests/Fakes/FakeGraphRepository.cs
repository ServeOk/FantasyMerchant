using FantasyMerchant.Domain.Entities;
using FantasyMerchant.Domain.Records;
using FantasyMerchant.Application.Interfaces;

namespace FantasyMerchant.Tests.Fakes;

public class FakeGraphRepository : IGraphRepository
{
    private readonly List<City> _cities = new();
    private readonly List<Road> _roads = new();
    private readonly List<Event> _events = new();

    public Task<City?> GetCityByIdAsync(Id id, CancellationToken ct = default)
        => Task.FromResult(_cities.FirstOrDefault(c => c.Id == id));

    public Task<List<City>> GetAllCitiesAsync(CancellationToken ct = default)
        => Task.FromResult(_cities.ToList());

    public Task<City> CreateCityAsync(City city, CancellationToken ct = default)
    {
        
        if (city.Id == null || city.Id.Value == Guid.Empty)
        {
            city.Id = new Id(Guid.NewGuid());
        }
        _cities.Add(city);
        return Task.FromResult(city);
    }

    public Task<City> UpdateCityAsync(City city, CancellationToken ct = default)
    {
        var existing = _cities.FirstOrDefault(c => c.Id == city.Id);
        if (existing != null)
        {
            _cities.Remove(existing);
            _cities.Add(city);
        }
        return Task.FromResult(city);
    }

    public Task<Road?> GetRoadByIdAsync(Id id, CancellationToken ct = default)
        => Task.FromResult(_roads.FirstOrDefault(r => r.Id == id));

    public Task<List<Road>> GetAllRoadsAsync(CancellationToken ct = default)
        => Task.FromResult(_roads.ToList());

    public Task<Road> CreateRoadAsync(Road road, CancellationToken ct = default)
    {
        if (road.Id == null || road.Id.Value == Guid.Empty)
        {
            road.Id = new Id(Guid.NewGuid());
        }
        _roads.Add(road);
        return Task.FromResult(road);
    }

    public Task<Road> UpdateRoadAsync(Road road, CancellationToken ct = default)
    {
        var existing = _roads.FirstOrDefault(r => r.Id == road.Id);
        if (existing != null)
        {
            _roads.Remove(existing);
            _roads.Add(road);
        }
        return Task.FromResult(road);
    }

    public Task<Dictionary<Id, List<Road>>> GetGraphStructureAsync(CancellationToken ct = default)
    {
        var graph = new Dictionary<Id, List<Road>>();
        foreach (var road in _roads)
        {
            if (!graph.ContainsKey(road.FromCityId))
                graph[road.FromCityId] = new List<Road>();
            graph[road.FromCityId].Add(road);
        }
        return Task.FromResult(graph);
    }

    public Task<Event> CreateEventAsync(Event evt, CancellationToken ct = default)
    {
        if (evt.Id == null || evt.Id.Value == Guid.Empty)
        {
            evt.Id = new Id(Guid.NewGuid());
        }
        _events.Add(evt);
        return Task.FromResult(evt);
    }

    public Task<Event> UpdateEventAsync(Event evt, CancellationToken ct = default)
    {
        var existing = _events.FirstOrDefault(e => e.Id == evt.Id);
        if (existing != null)
        {
            _events.Remove(existing);
            _events.Add(evt);
        }
        return Task.FromResult(evt);
    }


    public void SeedTestData()
    {
        var city1 = new City
        {
            Id = new Id(Guid.Parse("00000000-0000-0000-0000-000000000001")),
            Name = "Цитадель",
            X = 400,
            Y = 300,
            Description = "Главный торговый центр"
        };

        var city2 = new City
        {
            Id = new Id(Guid.Parse("00000000-0000-0000-0000-000000000002")),
            Name = "Северный Дозор",
            X = 400,
            Y = 100,
            Description = "Военный пост"
        };

        var city3 = new City
        {
            Id = new Id(Guid.Parse("00000000-0000-0000-0000-000000000003")),
            Name = "Восточный Порт",
            X = 600,
            Y = 300,
            Description = "Морской порт"
        };

        var city4 = new City
        {
            Id = new Id(Guid.Parse("00000000-0000-0000-0000-000000000004")),
            Name = "Южная Гавань",
            X = 400,
            Y = 500,
            Description = "Рыбацкая деревня"
        };

        _cities.AddRange(new[] { city1, city2, city3, city4 });

        var road1 = new Road
        {
            Id = new Id(Guid.Parse("00000000-0000-0000-0000-000000000005")),
            FromCityId = city1.Id,
            ToCityId = city2.Id,
            GoldCost = 10,
            DangerLevel = 5,
            LoadMultiplier = 1.0m,
            IsBlocked = false
        };

        var road2 = new Road
        {
            Id = new Id(Guid.Parse("00000000-0000-0000-0000-000000000006")),
            FromCityId = city1.Id,
            ToCityId = city3.Id,
            GoldCost = 15,
            DangerLevel = 2,
            LoadMultiplier = 1.0m,
            IsBlocked = false
        };

        var road3 = new Road
        {
            Id = new Id(Guid.Parse("00000000-0000-0000-0000-000000000007")),
            FromCityId = city2.Id,
            ToCityId = city3.Id,
            GoldCost = 5,
            DangerLevel = 8,
            LoadMultiplier = 1.0m,
            IsBlocked = false
        };

        var road4 = new Road
        {
            Id = new Id(Guid.Parse("00000000-0000-0000-0000-000000000008")),
            FromCityId = city1.Id,
            ToCityId = city4.Id,
            GoldCost = 8,
            DangerLevel = 3,
            LoadMultiplier = 1.0m,
            IsBlocked = false
        };

        _roads.AddRange(new[] { road1, road2, road3, road4 });
    }
}