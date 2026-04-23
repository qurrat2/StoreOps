using Microsoft.Extensions.DependencyInjection;
using StoreOps.Services.Analytics;
using StoreOps.Services.Categories;
using StoreOps.Services.Customers;
using StoreOps.Services.Orders;
using StoreOps.Services.Products;

namespace StoreOps.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IAnalyticsService, AnalyticsService>();

        return services;
    }
}
