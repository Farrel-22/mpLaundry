using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace mpLaundry.Services.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMpLaundryServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? "Server=(localdb)\\MSSQLLocalDB;Database=LaundryDb;Trusted_Connection=True;TrustServerCertificate=True";

        // Tambahkan DbContext & Service saat scaffold selesai
        // services.AddDbContext<AppDbContext>(o => o.UseSqlServer(connectionString));

        return services;
    }
}
