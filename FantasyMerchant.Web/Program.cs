using Microsoft.EntityFrameworkCore;
using System.Reflection;
using FantasyMerchant.Infrastructure.Persistence;
using FantasyMerchant.Domain.Repositories;
using FantasyMerchant.Domain.Services;
using FantasyMerchant.Infrastructure.Repositories;
using FantasyMerchant.Infrastructure.Services;
using MediatR;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(FantasyMerchant.Application.Features.FindRoute.FindRouteHandler).Assembly));


builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IGraphRepository, JsonGraphRepository>();
builder.Services.AddScoped<IRouteRepository, RouteRepository>();
builder.Services.AddScoped<IRouteOptimizer, RouteOptimizer>();

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
