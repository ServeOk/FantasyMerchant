namespace FantasyMerchant.Application.Common;

public record ApiResponse<T>(
    bool Success,
    T? Data,
    string? Message = null
);

public record ApiResponse(
    bool Success,
    string? Message = null
);
