using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RodizioOrganistas.Application.Interfaces;
using RodizioOrganistas.Application.Services;
using RodizioOrganistas.Domain.Interfaces;
using RodizioOrganistas.Infrastructure.Data;
using RodizioOrganistas.Infrastructure.Repositories;

namespace RodizioOrganistas.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? "server=mysql;port=3306;database=rodizio_organistas;user=root;password=root";

        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        services.AddScoped<IChurchRepository, ChurchRepository>();
        services.AddScoped<IOrganistRepository, OrganistRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IScheduleRepository, ScheduleRepository>();
        services.AddScoped<IScheduleService, ScheduleService>();

        return services;
    }
}
