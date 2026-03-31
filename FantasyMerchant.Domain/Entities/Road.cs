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

    public static Road Create(Id fromCityId, Id toCityId, int goldCost, int dangerLevel)
    {
        if (fromCityId == toCityId)
            throw new ArgumentException("Дорога не может соединять город с самим собой");
        if (goldCost < 0 || dangerLevel < 0)
            throw new ArgumentException("Веса не могут быть отрицательными");

        return new Road
        {
            Id = new Id(Guid.NewGuid()),
            FromCityId = fromCityId,
            ToCityId = toCityId,
            GoldCost = goldCost,
            DangerLevel = dangerLevel,
            LoadMultiplier = 1.0m
        };
    }

    public void UpdateWeights(int goldCost, int dangerLevel)
    {
        if (goldCost < 0 || dangerLevel < 0)
            throw new ArgumentException("Веса не могут быть отрицательными");
        GoldCost = goldCost;
        DangerLevel = dangerLevel;
    }

    public void ApplyLoadMultiplier(decimal multiplier)
    {
        if (multiplier < 0)
            throw new ArgumentException("Множитель не может быть отрицательным");
        LoadMultiplier = multiplier;
    }

    public void Block() => IsBlocked = true;
    public void Unblock() => IsBlocked = false;

    /// <summary>
    /// Расчёт итогового веса для выбранной стратегии оптимизации
    /// </summary>
    public int GetEffectiveWeight(RouteStrategy strategy)
    {
        if (IsBlocked) return int.MaxValue;

        int baseWeight = strategy switch
        {
            RouteStrategy.Merchant => GoldCost,
            RouteStrategy.Rogue => DangerLevel,
            RouteStrategy.Balanced => GoldCost + DangerLevel,
            _ => GoldCost
        };

        return (int)(baseWeight * LoadMultiplier);
    }
}
