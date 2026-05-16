using FantasyMerchant.Application.Services;
using FantasyMerchant.Domain.Entities;
using FantasyMerchant.Domain.Enums;
using FantasyMerchant.Domain.Records;
using FantasyMerchant.Tests.Fakes;
using Xunit;

namespace FantasyMerchant.Tests;

public class RouteOptimizerTests
{
    private readonly RouteOptimizer _optimizer;
    private readonly FakeGraphRepository _fakeRepository;

    public RouteOptimizerTests()
    {
        _fakeRepository = new FakeGraphRepository();
        _fakeRepository.SeedTestData();
        _optimizer = new RouteOptimizer();
    }

    [Fact]
    public async Task FindShortestPath_MerchantStrategy_ReturnsCorrectPath()
    {
        var graph = await _fakeRepository.GetGraphStructureAsync();
        var startId = new Id(Guid.Parse("00000000-0000-0000-0000-000000000001")); // Цитадель
        var endId = new Id(Guid.Parse("00000000-0000-0000-0000-000000000003")); // Восточный Порт

        var (path, totalGold, totalDanger) = await _optimizer.FindShortestPathAsync(
            graph, startId, endId, RouteStrategy.Merchant);

        Assert.NotEmpty(path);
        Assert.Equal(startId, path.First());
        Assert.Equal(endId, path.Last());
        Assert.True(totalGold >= 0);
    }

    [Fact]
    public async Task FindShortestPath_RogueStrategy_ReturnsCorrectPath()
    {
        var graph = await _fakeRepository.GetGraphStructureAsync();
        var startId = new Id(Guid.Parse("00000000-0000-0000-0000-000000000001"));
        var endId = new Id(Guid.Parse("00000000-0000-0000-0000-000000000003"));

        
        var (path, totalGold, totalDanger) = await _optimizer.FindShortestPathAsync(
            graph, startId, endId, RouteStrategy.Rogue);

       
        Assert.NotEmpty(path);
        Assert.True(totalDanger >= 0);
    }

    [Fact]
    public async Task FindShortestPath_SameCity_ReturnsSingleCity()
    {
        var graph = await _fakeRepository.GetGraphStructureAsync();
        var cityId = new Id(Guid.Parse("00000000-0000-0000-0000-000000000001"));

        var (path, totalGold, totalDanger) = await _optimizer.FindShortestPathAsync(
            graph, cityId, cityId, RouteStrategy.Merchant);

        Assert.Single(path);
        Assert.Equal(cityId, path[0]);
        Assert.Equal(0, totalGold);
        Assert.Equal(0, totalDanger);
    }

    [Fact]
    public async Task FindShortestPath_BlockedRoad_AvoidsBlockedRoad()
    {
        var graph = await _fakeRepository.GetGraphStructureAsync();
        var startId = new Id(Guid.Parse("00000000-0000-0000-0000-000000000001"));
        var endId = new Id(Guid.Parse("00000000-0000-0000-0000-000000000002"));

        var (path, totalGold, totalDanger) = await _optimizer.FindShortestPathAsync(
            graph, startId, endId, RouteStrategy.Merchant);

        Assert.NotEmpty(path);
    }
}
