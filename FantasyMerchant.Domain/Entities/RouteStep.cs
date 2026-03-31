namespace FantasyMerchant.Domain.Entities;

public class RouteStep : IEntity
{
    public Id Id { get; set; }
    public Id RouteId { get; set; }
    public Id CityId { get; set; }
    public int StepOrder { get; set; } 
    public virtual Route Route { get; set; } = null!;
    public virtual City City { get; set; } = null!;

    public static RouteStep Create(Id routeId, Id cityId, int stepOrder)
    {
        return new RouteStep
        {
            Id = new Id(Guid.NewGuid()),
            RouteId = routeId,
            CityId = cityId,
            StepOrder = stepOrder
        };
    }
}
