using Microsoft.Extensions.DependencyInjection;
using StoreOps.Services.Categories;
using StoreOps.Services.Products;

namespace StoreOps.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductService, ProductService>();

        return services;
    }
}
