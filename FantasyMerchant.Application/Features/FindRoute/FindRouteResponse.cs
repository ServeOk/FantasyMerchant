using FantasyMerchant.Domain.Records;

namespace FantasyMerchant.Application.Features.FindRoute;

public record FindRouteResponse(
    bool Success,
    List<Id> Path,
    int TotalGold,
    int TotalDanger,
    int TotalSteps,
    string? ErrorMessage = null
);
