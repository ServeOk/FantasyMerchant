namespace FantasyMerchant.Domain.Entities;

public class City : IEntity
{
    public Id Id { get; set; }
    public string Name { get; set; } = string.Empty;  // ← public set
    public int X { get; set; }
    public int Y { get; set; }
    public string? Description { get; set; }

    public virtual ICollection<Road> OutgoingRoads { get; set; } = new List<Road>();
    public virtual ICollection<Road> IncomingRoads { get; set; } = new List<Road>();
    public virtual ICollection<RouteStep> RouteSteps { get; set; } = new List<RouteStep>();
}