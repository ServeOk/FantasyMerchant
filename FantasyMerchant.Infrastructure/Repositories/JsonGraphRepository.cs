using System.Text.Json;
using FantasyMerchant.Application.Interfaces;
using FantasyMerchant.Domain.Entities;
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
        var cities = new[]
        {
        new City
        {
            Id = new Id(Guid.Parse("00000000-0000-0000-0000-000000000001")),
            Name = "Цитадель",
            X = 400,
            Y = 300,
            Description = "Главный торговый центр"
        },
        new City
        {
            Id = new Id(Guid.Parse("00000000-0000-0000-0000-000000000002")),
            Name = "Северный Дозор",
            X = 400,
            Y = 100,
            Description = "Военный пост"
        },
        new City
        {
            Id = new Id(Guid.Parse("00000000-0000-0000-0000-000000000003")),
            Name = "Восточный Порт",
            X = 600,
            Y = 300,
            Description = "Морской порт"
        },
        new City
        {
            Id = new Id(Guid.Parse("00000000-0000-0000-0000-000000000004")),
            Name = "Южная Гавань",
            X = 400,
            Y = 500,
            Description = "Рыбацкая деревня"
        },
        new City
        {
            Id = new Id(Guid.Parse("00000000-0000-0000-0000-000000000005")),
            Name = "Западный Форт",
            X = 200,
            Y = 300,
            Description = "Пограничная крепость"
        },
        new City
        {
            Id = new Id(Guid.Parse("00000000-0000-0000-0000-000000000006")),
            Name = "Горная Твердыня",
            X = 300,
            Y = 150,
            Description = "Шахтёрский город"
        },
        new City
        {
            Id = new Id(Guid.Parse("00000000-0000-0000-0000-000000000007")),
            Name = "Лесная Чаща",
            X = 550,
            Y = 200,
            Description = "Эльфийское поселение"
        },
        new City
        {
            Id = new Id(Guid.Parse("00000000-0000-0000-0000-000000000008")),
            Name = "Пустошь",
            X = 500,
            Y = 450,
            Description = "Заброшенные земли"
        }
    };

        _cities.AddRange(cities);

        var roads = new[]
        {
        new Road { Id = new Id(Guid.Parse("10000000-0000-0000-0000-000000000001")), FromCityId = cities[0].Id, ToCityId = cities[1].Id, GoldCost = 10, DangerLevel = 5, LoadMultiplier = 1.0m, IsBlocked = false },
        new Road { Id = new Id(Guid.Parse("10000000-0000-0000-0000-000000000002")), FromCityId = cities[0].Id, ToCityId = cities[2].Id, GoldCost = 15, DangerLevel = 2, LoadMultiplier = 1.0m, IsBlocked = false },
        new Road { Id = new Id(Guid.Parse("10000000-0000-0000-0000-000000000003")), FromCityId = cities[0].Id, ToCityId = cities[3].Id, GoldCost = 8, DangerLevel = 3, LoadMultiplier = 1.0m, IsBlocked = false },
        new Road { Id = new Id(Guid.Parse("10000000-0000-0000-0000-000000000004")), FromCityId = cities[0].Id, ToCityId = cities[4].Id, GoldCost = 12, DangerLevel = 4, LoadMultiplier = 1.0m, IsBlocked = false },
        
        new Road { Id = new Id(Guid.Parse("10000000-0000-0000-0000-000000000005")), FromCityId = cities[1].Id, ToCityId = cities[2].Id, GoldCost = 5, DangerLevel = 8, LoadMultiplier = 1.0m, IsBlocked = false },
        new Road { Id = new Id(Guid.Parse("10000000-0000-0000-0000-000000000006")), FromCityId = cities[1].Id, ToCityId = cities[5].Id, GoldCost = 7, DangerLevel = 3, LoadMultiplier = 1.0m, IsBlocked = false },
        new Road { Id = new Id(Guid.Parse("10000000-0000-0000-0000-000000000007")), FromCityId = cities[1].Id, ToCityId = cities[6].Id, GoldCost = 20, DangerLevel = 6, LoadMultiplier = 1.0m, IsBlocked = false },
        
        new Road { Id = new Id(Guid.Parse("10000000-0000-0000-0000-000000000008")), FromCityId = cities[2].Id, ToCityId = cities[6].Id, GoldCost = 6, DangerLevel = 2, LoadMultiplier = 1.0m, IsBlocked = false },
        new Road { Id = new Id(Guid.Parse("10000000-0000-0000-0000-000000000009")), FromCityId = cities[2].Id, ToCityId = cities[7].Id, GoldCost = 18, DangerLevel = 7, LoadMultiplier = 1.0m, IsBlocked = false },
        
        new Road { Id = new Id(Guid.Parse("10000000-0000-0000-0000-000000000010")), FromCityId = cities[3].Id, ToCityId = cities[7].Id, GoldCost = 9, DangerLevel = 4, LoadMultiplier = 1.0m, IsBlocked = false },
        new Road { Id = new Id(Guid.Parse("10000000-0000-0000-0000-000000000011")), FromCityId = cities[3].Id, ToCityId = cities[4].Id, GoldCost = 11, DangerLevel = 5, LoadMultiplier = 1.0m, IsBlocked = false },
        
        new Road { Id = new Id(Guid.Parse("10000000-0000-0000-0000-000000000012")), FromCityId = cities[4].Id, ToCityId = cities[5].Id, GoldCost = 14, DangerLevel = 6, LoadMultiplier = 1.0m, IsBlocked = false },
        new Road { Id = new Id(Guid.Parse("10000000-0000-0000-0000-000000000013")), FromCityId = cities[4].Id, ToCityId = cities[3].Id, GoldCost = 13, DangerLevel = 4, LoadMultiplier = 1.0m, IsBlocked = false },
        
        new Road { Id = new Id(Guid.Parse("10000000-0000-0000-0000-000000000014")), FromCityId = cities[5].Id, ToCityId = cities[6].Id, GoldCost = 16, DangerLevel = 9, LoadMultiplier = 1.0m, IsBlocked = true }, // Заблокирована!
        
        new Road { Id = new Id(Guid.Parse("10000000-0000-0000-0000-000000000015")), FromCityId = cities[6].Id, ToCityId = cities[7].Id, GoldCost = 8, DangerLevel = 3, LoadMultiplier = 1.0m, IsBlocked = false }
    };

        _roads.AddRange(roads);
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
