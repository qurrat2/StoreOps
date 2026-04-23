using Microsoft.Extensions.DependencyInjection;
using StoreOps.Services.Categories;

namespace StoreOps.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();

        return services;
    }
}
