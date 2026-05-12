namespace FantasyMerchant.Domain.Entities;

public class Road : IEntity
{
    public Id Id { get; set; }
    public Id FromCityId { get; set; }
    public Id ToCityId { get; set; }
    public int GoldCost { get; set; }
    public int DangerLevel { get; set; }
    public decimal LoadMultiplier { get; set; } = 1.0m;
    public bool IsBlocked { get; set; } = false;

    public virtual City FromCity { get; set; } = null!;
    public virtual City ToCity { get; set; } = null!;
    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
