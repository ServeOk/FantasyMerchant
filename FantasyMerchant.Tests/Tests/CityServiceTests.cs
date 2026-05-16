using FantasyMerchant.Application.Services;
using FantasyMerchant.Domain.Records;
using FantasyMerchant.Tests.Fakes;
using Xunit;

namespace FantasyMerchant.Tests;

public class CityServiceTests
{
    private readonly CityService _cityService;
    private readonly FakeGraphRepository _fakeRepository;

    public CityServiceTests()
    {
        _fakeRepository = new FakeGraphRepository();
        _fakeRepository.SeedTestData();
        _cityService = new CityService(_fakeRepository);
    }

    public async Task GetAllCities_ReturnsAllCities()
    {
        var cities = await _cityService.GetAllCitiesAsync();

        Assert.NotEmpty(cities);
        Assert.True(cities.Count >= 4); // У нас 4 тестовых города
    }

    [Fact]
    public async Task GetCityById_ExistingCity_ReturnsCity()
    {
        var cityId = new Id(Guid.Parse("00000000-0000-0000-0000-000000000001"));

        var city = await _cityService.GetCityByIdAsync(cityId);

        Assert.NotNull(city);
        Assert.Equal(cityId, city.Id);
        Assert.Equal("Цитадель", city.Name);
    }

    [Fact]
    public async Task CreateCity_ValidData_CreatesCity()
    {
        var cityName = "Новый Город";
        var x = 100;
        var y = 200;

        var city = await _cityService.CreateCityAsync(cityName, x, y);

        Assert.NotNull(city);
        Assert.Equal(cityName, city.Name);
        Assert.Equal(x, city.X);
        Assert.Equal(y, city.Y);
    }

    [Fact]
    public async Task CreateCity_EmptyName_ThrowsException()
    {
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _cityService.CreateCityAsync("", 100, 200));
    }

    [Fact]
    public async Task UpdateCity_ValidData_UpdatesCity()
    {
        var cityId = new Id(Guid.Parse("00000000-0000-0000-0000-000000000001"));
        var newName = "Обновлённая Цитадель";

        var updatedCity = await _cityService.UpdateCityAsync(cityId, name: newName);

        Assert.Equal(newName, updatedCity.Name);
    }
}
