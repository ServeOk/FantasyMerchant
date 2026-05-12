using FantasyMerchant.Application.Interfaces;
using FantasyMerchant.Application.Services;
using FantasyMerchant.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);




builder.Services.AddScoped<IRouteOptimizer, RouteOptimizer>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<IRoadService, RoadService>();


builder.Services.AddScoped<IGraphRepository, JsonGraphRepository>();
builder.Services.AddScoped<IRouteRepository, RouteRepository>();


builder.Services.AddControllersWithViews();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
