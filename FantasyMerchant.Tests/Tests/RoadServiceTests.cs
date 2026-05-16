using FantasyMerchant.Application.Services;
using FantasyMerchant.Domain.Records;
using FantasyMerchant.Tests.Fakes;
using Xunit;

namespace FantasyMerchant.Tests;

public class RoadServiceTests
{
    private readonly RoadService _roadService;
    private readonly FakeGraphRepository _fakeRepository;

    public RoadServiceTests()
    {
        _fakeRepository = new FakeGraphRepository();
        _fakeRepository.SeedTestData();
        _roadService = new RoadService(_fakeRepository);
    }

    [Fact]
    public async Task CreateRoad_ValidData_CreatesRoad()
    {
        var fromId = new Id(Guid.Parse("00000000-0000-0000-0000-000000000001"));
        var toId = new Id(Guid.Parse("00000000-0000-0000-0000-000000000002"));
        var goldCost = 10;
        var dangerLevel = 5;

        var road = await _roadService.CreateRoadAsync(fromId, toId, goldCost, dangerLevel);

        Assert.NotNull(road);
        Assert.Equal(goldCost, road.GoldCost);
        Assert.Equal(dangerLevel, road.DangerLevel);
    }

    [Fact]
    public async Task CreateRoad_SameCity_ThrowsException()
    {
        var cityId = new Id(Guid.Parse("00000000-0000-0000-0000-000000000001"));

        await Assert.ThrowsAsync<ArgumentException>(() =>
            _roadService.CreateRoadAsync(cityId, cityId, 10, 5));
    }

    [Fact]
    public async Task CreateRoad_NegativeGold_ThrowsException()
    {
        var fromId = new Id(Guid.Parse("00000000-0000-0000-0000-000000000001"));
        var toId = new Id(Guid.Parse("00000000-0000-0000-0000-000000000002"));

        await Assert.ThrowsAsync<ArgumentException>(() =>
            _roadService.CreateRoadAsync(fromId, toId, -10, 5));
    }

    [Fact]
    public async Task BlockRoad_ExistingRoad_BlocksRoad()
    {
        var roadId = new Id(Guid.Parse("00000000-0000-0000-0000-000000000005"));

        var blockedRoad = await _roadService.BlockRoadAsync(roadId);

        Assert.True(blockedRoad.IsBlocked);
    }

    [Fact]
    public async Task UnblockRoad_BlockedRoad_UnblocksRoad()
    {
        var roadId = new Id(Guid.Parse("00000000-0000-0000-0000-000000000005"));
        await _roadService.BlockRoadAsync(roadId);

        var unblockedRoad = await _roadService.UnblockRoadAsync(roadId);

        Assert.False(unblockedRoad.IsBlocked);
    }
}
