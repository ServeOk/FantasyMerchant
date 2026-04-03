using FluentValidation;

namespace FantasyMerchant.Application.Features.FindRoute;

public class FindRouteQueryValidator : AbstractValidator<FindRouteQuery>
{
    public FindRouteQueryValidator()
    {
        RuleFor(x => x.StartCityId)
            .NotNull().WithMessage("Город старта обязателен")
            .Must(id => id.Value != Guid.Empty).WithMessage("Неверный ID города старта");

        RuleFor(x => x.EndCityId)
            .NotNull().WithMessage("Город назначения обязателен")
            .Must(id => id.Value != Guid.Empty).WithMessage("Неверный ID города назначения");

        RuleFor(x => x.Strategy)
            .NotEmpty().WithMessage("Стратегия обязательна")
            .Must(s => s is "merchant" or "rogue" or "balanced")
            .WithMessage("Допустимые стратегии: merchant, rogue, balanced");

        RuleFor(x => x.EndCityId)
            .NotEqual(x => x.StartCityId)
            .WithMessage("Город старта и назначения должны различаться");
    }
}
