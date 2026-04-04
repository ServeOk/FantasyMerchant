namespace FantasyMerchant.Domain.Entities;

public class Event : IEntity
{
    public Id Id { get; set; }
    public Id RoadId { get; set; }
    public EventType Type { get; set; }
    public decimal WeightModifier { get; set; } 
    public DateTime? StartsAt { get; set; }
    public DateTime? EndsAt { get; set; }
    public bool IsActive { get; set; } = true;

    public virtual Road Road { get; set; } = null!;

    public static Event Create(Id roadId, EventType type, decimal weightModifier)
    {
        if (weightModifier < 0)
            throw new ArgumentException("Модификатор не может быть отрицательным");

        return new Event
        {
            Id = new Id(Guid.NewGuid()),
            RoadId = roadId,
            Type = type,
            WeightModifier = weightModifier,
            StartsAt = DateTime.UtcNow
        };
    }

    public void Deactivate()
    {
        IsActive = false;
        EndsAt = DateTime.UtcNow;
    }

    public bool IsExpired => EndsAt.HasValue && DateTime.UtcNow > EndsAt.Value;
}
