using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using mpLaundry.DAL.ScaffoldContainer;
using mpLaundry.Services.Interfaces;

namespace mpLaundry.Services.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMpLaundryServices(this IServiceCollection services, IConfiguration configuration)
    {
        // ── Database ──
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? "Host=localhost;Port=5432;Database=laundrydb;Username=postgres;Password=admin";

        services.AddDbContext<LaundryDbContext>(options =>
            options.UseNpgsql(connectionString));

        // ── Service Layer ──
        services.AddScoped<ILayananService, LayananService>();
        services.AddScoped<IPelangganService, PelangganService>();

        return services;
    }
}
