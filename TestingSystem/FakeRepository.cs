using FantasyMerchant.Domain.Entities;
using FantasyMerchant.Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace FantasyMerchant;

public class FakeGraphRepository : IGraphRepository
{
    private readonly List<City> _cities = new();
    private readonly List<Road> _roads = new();
    private readonly List<Event> _events = new();

    public Task<City?> GetCityByIdAsync(int id, CancellationToken ct = default)
        => Task.FromResult(_cities.FirstOrDefault(c => c.Id == id));

    public Task<List<City>> GetAllCitiesAsync(CancellationToken ct = default)
        => Task.FromResult(_cities.ToList());

    public Task<City> CreateCityAsync(City city, CancellationToken ct = default)
    {
        city.Id = _cities.Count + 1;
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

    public Task<Road?> GetRoadByIdAsync(int id, CancellationToken ct = default)
        => Task.FromResult(_roads.FirstOrDefault(r => r.Id == id));

    public Task<List<Road>> GetAllRoadsAsync(CancellationToken ct = default)
        => Task.FromResult(_roads.ToList());

    public Task<Road> CreateRoadAsync(Road road, CancellationToken ct = default)
    {
        road.Id = _roads.Count + 1;
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

    public Task<Dictionary<int, List<Road>>> GetGraphStructureAsync(CancellationToken ct = default)
    {
        var graph = new Dictionary<int, List<Road>>();
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
        evt.Id = _events.Count + 1;
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

    // Helper метод для тестов — заполнить тестовыми данными
    public void SeedTestData()
    {
        // Города: Цитадель (1), Северный Дозор (2), Восточный Порт (3)
        var city1 = City.Create("Цитадель", 400, 300);
        city1.Id = 1;
        var city2 = City.Create("Северный Дозор", 400, 100);
        city2.Id = 2;
        var city3 = City.Create("Восточный Порт", 600, 300);
        city3.Id = 3;
        _cities.AddRange(new[] { city1, city2, city3 });

        // Дороги
        var road1 = Road.Create(1, 2, 10, 5); // Цитадель → Северный
        road1.Id = 1;
        var road2 = Road.Create(1, 3, 15, 2); // Цитадель → Восточный
        road2.Id = 2;
        var road3 = Road.Create(2, 3, 5, 8);  // Северный → Восточный
        road3.Id = 3;
        _roads.AddRange(new[] { road1, road2, road3 });
    }
}