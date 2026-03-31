using MediatR;

namespace FantasyMerchant.Application.Common;

public abstract class UseCase<TRequest, TResponse> : IRequest<TResponse>
    where TRequest : UseCase<TRequest, TResponse>
{
}

public abstract class UseCase<TRequest> : IRequest
    where TRequest : UseCase<TRequest>
{
}