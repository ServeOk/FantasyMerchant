using System.ComponentModel.DataAnnotations;

namespace FantasyMerchant.Web.ViewModels;

public class FindRouteViewModel
{
    [Required(ErrorMessage = "Город старта обязателен")]
    [Display(Name = "Город старта")]
    public string StartCityId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Город назначения обязателен")]
    [Display(Name = "Город назначения")]
    public string EndCityId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Стратегия обязательна")]
    [Display(Name = "Стратегия оптимизации")]
    public string Strategy { get; set; } = "merchant";
}
