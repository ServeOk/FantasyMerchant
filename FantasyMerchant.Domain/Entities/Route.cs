namespace FantasyMerchant.Domain.Entities;

public class Route : IEntity
{
    public Id Id { get; set; }
    public Id StartCityId { get; set; }
    public Id EndCityId { get; set; }
    public RouteStrategy Strategy { get; set; } = RouteStrategy.Merchant;
    public int TotalGold { get; set; }
    public int TotalDanger { get; set; }
    public int TotalSteps { get; set; }
    public virtual City StartCity { get; set; } = null!;
    public virtual City EndCity { get; set; } = null!;
    public virtual ICollection<RouteStep> Steps { get; set; } = new List<RouteStep>();

    public static Route Create(Id startCityId, Id endCityId, RouteStrategy strategy)
    {
        if (startCityId == endCityId)
            throw new ArgumentException("Город старта и назначения должны различаться");

        return new Route
        {
            Id = new Id(Guid.NewGuid()),
            StartCityId = startCityId,
            EndCityId = endCityId,
            Strategy = strategy
        };
    }

    public void SetTotals(int totalGold, int totalDanger, int totalSteps)
    {
        TotalGold = totalGold;
        TotalDanger = totalDanger;
        TotalSteps = totalSteps;
    }
}