using FantasyMerchant.Domain.Records;

namespace FantasyMerchant.Domain.Interfaces;

public interface IEntity
{
    Id Id { get; set; }
}
