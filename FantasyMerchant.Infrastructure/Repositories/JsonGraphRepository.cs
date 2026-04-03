using System.Text.Json;
using FantasyMerchant.Domain.Entities;
using FantasyMerchant.Domain.Repositories;
using FantasyMerchant.Domain.Records;
using FantasyMerchant.Infrastructure.JsonConverters;

namespace FantasyMerchant.Infrastructure.Repositories;

public class JsonGraphRepository : IGraphRepository
{
    private readonly string _filePath = "Data/graph.json";
    private List<City> _cities = new();
    private List<Road> _roads = new();
    private List<Event> _events = new();

    public JsonGraphRepository()
    {
        LoadData();
    }

    private void LoadData()
    {
        if (File.Exists(_filePath))
        {
            var json = File.ReadAllText(_filePath);

            var options = new JsonSerializerOptions
            {
                Converters = { new IdJsonConverter() },
                PropertyNameCaseInsensitive = true
            };

            var data = JsonSerializer.Deserialize<GraphData>(json, options);
            if (data != null)
            {
                _cities = data.Cities ?? new List<City>();
                _roads = data.Roads ?? new List<Road>();
                _events = data.Events ?? new List<Event>();
            }
        }
        else
        {
            InitializeTestData();
        }
    }

    private void SaveData()
    {
        var data = new GraphData
        {
            Cities = _cities,
            Roads = _roads,
            Events = _events
        };

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new IdJsonConverter() }
        };

        var json = JsonSerializer.Serialize(data, options);

        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
        File.WriteAllText(_filePath, json);
    }

    private void InitializeTestData()
    {
        var city1 = City.Create("Цитадель", 400, 300);
        city1.Id = new Id(Guid.Parse("00000000-0000-0000-0000-000000000001"));

        var city2 = City.Create("Северный Дозор", 400, 100);
        city2.Id = new Id(Guid.Parse("00000000-0000-0000-0000-000000000002"));

        var city3 = City.Create("Восточный Порт", 600, 300);
        city3.Id = new Id(Guid.Parse("00000000-0000-0000-0000-000000000003"));

        var city4 = City.Create("Южная Гавань", 400, 500);
        city4.Id = new Id(Guid.Parse("00000000-0000-0000-0000-000000000004"));

        _cities.AddRange(new[] { city1, city2, city3, city4 });

        var road1 = Road.Create(city1.Id, city2.Id, 10, 5);
        road1.Id = new Id(Guid.Parse("00000000-0000-0000-0000-000000000005"));

        var road2 = Road.Create(city1.Id, city3.Id, 15, 2);
        road2.Id = new Id(Guid.Parse("00000000-0000-0000-0000-000000000006"));

        var road3 = Road.Create(city2.Id, city3.Id, 5, 8);
        road3.Id = new Id(Guid.Parse("00000000-0000-0000-0000-000000000007"));

        var road4 = Road.Create(city1.Id, city4.Id, 8, 3);
        road4.Id = new Id(Guid.Parse("00000000-0000-0000-0000-000000000008"));

        _roads.AddRange(new[] { road1, road2, road3, road4 });

        SaveData();
    }

    
    public Task<City?> GetCityByIdAsync(Id id, CancellationToken ct = default)
        => Task.FromResult(_cities.FirstOrDefault(c => c.Id == id));

    public Task<List<City>> GetAllCitiesAsync(CancellationToken ct = default)
        => Task.FromResult(_cities.ToList());

    public Task<City> CreateCityAsync(City city, CancellationToken ct = default)
    {
        if (city.Id == null || city.Id.Value == Guid.Empty)
            city.Id = new Id(Guid.NewGuid());
        _cities.Add(city);
        SaveData();
        return Task.FromResult(city);
    }

    public Task<City> UpdateCityAsync(City city, CancellationToken ct = default)
    {
        var existing = _cities.FirstOrDefault(c => c.Id == city.Id);
        if (existing != null)
        {
            _cities.Remove(existing);
            _cities.Add(city);
            SaveData();
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
            road.Id = new Id(Guid.NewGuid());
        _roads.Add(road);
        SaveData();
        return Task.FromResult(road);
    }

    public Task<Road> UpdateRoadAsync(Road road, CancellationToken ct = default)
    {
        var existing = _roads.FirstOrDefault(r => r.Id == road.Id);
        if (existing != null)
        {
            _roads.Remove(existing);
            _roads.Add(road);
            SaveData();
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
            evt.Id = new Id(Guid.NewGuid());
        _events.Add(evt);
        SaveData();
        return Task.FromResult(evt);
    }

    public Task<Event> UpdateEventAsync(Event evt, CancellationToken ct = default)
    {
        var existing = _events.FirstOrDefault(e => e.Id == evt.Id);
        if (existing != null)
        {
            _events.Remove(existing);
            _events.Add(evt);
            SaveData();
        }
        return Task.FromResult(evt);
    }

    private class GraphData
    {
        public List<City>? Cities { get; set; }
        public List<Road>? Roads { get; set; }
        public List<Event>? Events { get; set; }
    }
}
