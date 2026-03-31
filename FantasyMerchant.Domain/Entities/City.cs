namespace FantasyMerchant.Domain.Entities;

public class City : IEntity
{
    public Id Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int X { get; set; }      
    public int Y { get; set; }      
    public string? Description { get; set; }

    public virtual ICollection<Road> OutgoingRoads { get; set; } = new List<Road>();
    public virtual ICollection<Road> IncomingRoads { get; set; } = new List<Road>();
    public virtual ICollection<RouteStep> RouteSteps { get; set; } = new List<RouteStep>();

    public static City Create(string name, int x, int y, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Название города обязательно", nameof(name));

        return new City
        {
            Id = new Id(Guid.NewGuid()),
            Name = name,
            X = x,
            Y = y,
            Description = description
        };
    }

    public void UpdateCoordinates(int x, int y)
    {
        X = x;
        Y = y;
    }
}