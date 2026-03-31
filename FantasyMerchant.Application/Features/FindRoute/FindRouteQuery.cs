using FantasyMerchant.Domain.Records;
using MediatR;

namespace FantasyMerchant.Application.Features.FindRoute;

public class FindRouteQuery : IRequest<FindRouteResponse>
{
    public Id StartCityId { get; }
    public Id EndCityId { get; }
    public string Strategy { get; }

    public FindRouteQuery(Id startCityId, Id endCityId, string strategy)
    {
        StartCityId = startCityId;
        EndCityId = endCityId;
        Strategy = strategy;
    }
}

public record FindRouteResponse(
    List<Id> Path,
    int TotalGold,
    int TotalDanger,
    int TotalSteps
);