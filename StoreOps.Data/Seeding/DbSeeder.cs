using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StoreOps.Models;
using StoreOps.Models.Enums;

namespace StoreOps.Data.Seeding;

public static class DbSeeder
{
    public static async Task ResetAndSeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();

        using (var context = await factory.CreateDbContextAsync())
        {
            await context.Orders.ExecuteDeleteAsync();
            await context.Set<InventoryTransaction>().ExecuteDeleteAsync();
            await context.Set<Coupon>().ExecuteDeleteAsync();
            await context.Customers.ExecuteDeleteAsync();
            await context.Products.ExecuteDeleteAsync();
            await context.Categories.ExecuteDeleteAsync();
        }

        await SeedAsync(services);
    }

    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
        using var context = await factory.CreateDbContextAsync();

        if (await context.Categories.AnyAsync()) return;

        var now = DateTime.UtcNow;

        var categories = new List<Category>
        {
            new() { Name = "Electronics",    Description = "Gadgets, devices, and accessories", CreatedAt = now.AddDays(-30) },
            new() { Name = "Books",          Description = "Technical and general reading",      CreatedAt = now.AddDays(-30) },
            new() { Name = "Clothing",       Description = "Apparel and accessories",            CreatedAt = now.AddDays(-30) },
            new() { Name = "Home & Kitchen", Description = "Cookware and home goods",            CreatedAt = now.AddDays(-30) },
            new() { Name = "Toys",           Description = "Games and collectibles",             CreatedAt = now.AddDays(-30) }
        };
        context.Categories.AddRange(categories);
        await context.SaveChangesAsync();

        var products = new List<Product>
        {
            new() { Sku = "ELEC-P1", Name = "Wireless Headphones",                Description = "Noise-cancelling over-ear headphones", Price = 129.99m, CostPrice = 70m,  StockQuantity = 100, ReorderLevel = 10, CategoryId = categories[0].Id, CreatedAt = now.AddDays(-100) },
            new() { Sku = "ELEC-P2", Name = "Smart Watch",                        Description = "Fitness tracking with heart-rate",       Price = 249m,    CostPrice = 140m, StockQuantity = 20,  ReorderLevel = 15, CategoryId = categories[0].Id, CreatedAt = now.AddDays(-100) },
            new() { Sku = "ELEC-P3", Name = "USB-C Hub",                          Description = "7-in-1 multiport adapter",               Price = 39.99m,  CostPrice = 15m,  StockQuantity = 150, ReorderLevel = 20, CategoryId = categories[0].Id, CreatedAt = now.AddDays(-100) },

            new() { Sku = "BOOK-P1", Name = "Clean Architecture",                 Description = "Robert C. Martin",                       Price = 42m,     CostPrice = 18m,  StockQuantity = 50,  ReorderLevel = 10, CategoryId = categories[1].Id, CreatedAt = now.AddDays(-100) },
            new() { Sku = "BOOK-P2", Name = "The Pragmatic Programmer",           Description = "Hunt and Thomas",                        Price = 38m,     CostPrice = 16m,  StockQuantity = 15,  ReorderLevel = 10, CategoryId = categories[1].Id, CreatedAt = now.AddDays(-100) },
            new() { Sku = "BOOK-P3", Name = "Designing Data-Intensive Apps",      Description = "Martin Kleppmann",                       Price = 55m,     CostPrice = 22m,  StockQuantity = 50,  ReorderLevel = 10, CategoryId = categories[1].Id, CreatedAt = now.AddDays(-100) },

            new() { Sku = "CLOT-P1", Name = "Cotton T-Shirt",                     Description = "Unisex, 100% organic cotton",            Price = 24.99m,  CostPrice = 8m,   StockQuantity = 250, ReorderLevel = 50, CategoryId = categories[2].Id, CreatedAt = now.AddDays(-100) },
            new() { Sku = "CLOT-P2", Name = "Denim Jacket",                       Description = "Classic blue denim",                     Price = 89m,     CostPrice = 35m,  StockQuantity = 50,  ReorderLevel = 20, CategoryId = categories[2].Id, CreatedAt = now.AddDays(-100) },
            new() { Sku = "CLOT-P3", Name = "Running Shoes",                      Description = "Lightweight trainers",                   Price = 129m,    CostPrice = 50m,  StockQuantity = 10,  ReorderLevel = 15, CategoryId = categories[2].Id, CreatedAt = now.AddDays(-100) },

            new() { Sku = "HOME-P1", Name = "Non-stick Frying Pan",               Description = "28cm ceramic-coated",                    Price = 45m,     CostPrice = 20m,  StockQuantity = 80,  ReorderLevel = 15, CategoryId = categories[3].Id, CreatedAt = now.AddDays(-100) },
            new() { Sku = "HOME-P2", Name = "Coffee Maker",                       Description = "12-cup programmable",                    Price = 89m,     CostPrice = 40m,  StockQuantity = 40,  ReorderLevel = 10, CategoryId = categories[3].Id, CreatedAt = now.AddDays(-100) },
            new() { Sku = "HOME-P3", Name = "Kitchen Knife Set",                  Description = "8-piece stainless steel",                Price = 149m,    CostPrice = 65m,  StockQuantity = 40,  ReorderLevel = 20, CategoryId = categories[3].Id, CreatedAt = now.AddDays(-100) },

            new() { Sku = "TOYS-P1", Name = "LEGO Classic Box",                   Description = "790 colourful pieces",                   Price = 59.99m,  CostPrice = 28m,  StockQuantity = 70,  ReorderLevel = 15, CategoryId = categories[4].Id, CreatedAt = now.AddDays(-100) },
            new() { Sku = "TOYS-P2", Name = "Board Game: Catan",                  Description = "Classic strategy board game",            Price = 49m,     CostPrice = 22m,  StockQuantity = 25,  ReorderLevel = 15, CategoryId = categories[4].Id, CreatedAt = now.AddDays(-100) },
            new() { Sku = "TOYS-P3", Name = "Remote Control Car",                 Description = "1:12 scale, rechargeable",               Price = 79m,     CostPrice = 35m,  StockQuantity = 15,  ReorderLevel = 10, CategoryId = categories[4].Id, CreatedAt = now.AddDays(-100) }
        };
        context.Products.AddRange(products);
        await context.SaveChangesAsync();

        var customers = new List<Customer>
        {
            new() { FirstName = "Sarah",  LastName = "Johnson", Email = "sarah.johnson@example.com", Phone = "+1-212-555-0101", AddressLine1 = "42 Broadway",       City = "New York",  State = "NY",        PostalCode = "10004",   Country = "USA",      CreatedAt = now.AddDays(-20) },
            new() { FirstName = "Ahmed",  LastName = "Khan",    Email = "ahmed.khan@example.com",    Phone = "+92-300-555-0202", AddressLine1 = "15-B Clifton Block 2", City = "Karachi",   State = "Sindh",     PostalCode = "75600",   Country = "Pakistan", CreatedAt = now.AddDays(-19) },
            new() { FirstName = "Maria",  LastName = "Garcia",  Email = "maria.garcia@example.com",  Phone = "+34-91-555-0303",  AddressLine1 = "Calle de Alcalá 12",   City = "Madrid",    State = "Madrid",    PostalCode = "28014",   Country = "Spain",    CreatedAt = now.AddDays(-18) },
            new() { FirstName = "James",  LastName = "Wilson",  Email = "j.wilson@example.com",      Phone = "+44-20-7946-0404", AddressLine1 = "221B Baker Street",    City = "London",    State = "England",   PostalCode = "NW1 6XE", Country = "UK",       CreatedAt = now.AddDays(-15) },
            new() { FirstName = "Yuki",   LastName = "Tanaka",  Email = "yuki.tanaka@example.com",   Phone = "+81-3-5555-0505",  AddressLine1 = "1-1 Shibuya",          City = "Tokyo",     State = "Tokyo",     PostalCode = "150-0002", Country = "Japan",    CreatedAt = now.AddDays(-12) },
            new() { FirstName = "Priya",  LastName = "Patel",   Email = "priya.patel@example.com",   Phone = "+91-22-5555-0606", AddressLine1 = "Marine Drive 88",      City = "Mumbai",    State = "Maharashtra", PostalCode = "400020",  Country = "India",    CreatedAt = now.AddDays(-10) }
        };
        context.Customers.AddRange(customers);
        await context.SaveChangesAsync();

        var orderSeeds = new List<OrderSeed>
        {
            //          days  customer   status                 payment                 items (productIdx, qty)
            // ~3 months back
            new(85, 0, OrderStatus.Delivered,  PaymentStatus.Paid,    new[] { (0, 1), (3, 2) }),
            new(80, 3, OrderStatus.Delivered,  PaymentStatus.Paid,    new[] { (6, 3) }),
            new(75, 1, OrderStatus.Delivered,  PaymentStatus.Paid,    new[] { (12, 2) }),
            new(70, 2, OrderStatus.Delivered,  PaymentStatus.Paid,    new[] { (9, 1), (10, 1) }),
            new(65, 4, OrderStatus.Delivered,  PaymentStatus.Paid,    new[] { (0, 2), (6, 1) }),
            // ~2 months back
            new(58, 5, OrderStatus.Delivered,  PaymentStatus.Paid,    new[] { (2, 1), (4, 1) }),
            new(52, 0, OrderStatus.Delivered,  PaymentStatus.Paid,    new[] { (4, 1), (3, 1) }),
            new(48, 3, OrderStatus.Delivered,  PaymentStatus.Paid,    new[] { (7, 1), (12, 1) }),
            new(45, 1, OrderStatus.Delivered,  PaymentStatus.Paid,    new[] { (0, 1) }),
            new(42, 2, OrderStatus.Delivered,  PaymentStatus.Paid,    new[] { (6, 2), (13, 1) }),
            // ~1 month back
            new(38, 4, OrderStatus.Delivered,  PaymentStatus.Paid,    new[] { (12, 1), (13, 1) }),
            new(32, 5, OrderStatus.Delivered,  PaymentStatus.Paid,    new[] { (9, 1), (0, 1) }),
            new(28, 0, OrderStatus.Delivered,  PaymentStatus.Paid,    new[] { (3, 1), (4, 2) }),
            new(22, 3, OrderStatus.Delivered,  PaymentStatus.Paid,    new[] { (2, 1), (6, 2) }),
            new(18, 1, OrderStatus.Delivered,  PaymentStatus.Paid,    new[] { (6, 2) }),
            // last 2 weeks (original seeds)
            new(14, 0, OrderStatus.Delivered,  PaymentStatus.Paid,    new[] { (0, 1), (3, 1) }),
            new(12, 1, OrderStatus.Delivered,  PaymentStatus.Paid,    new[] { (6, 2), (12, 1) }),
            new(10, 2, OrderStatus.Delivered,  PaymentStatus.Paid,    new[] { (1, 1), (5, 1) }),
            new( 8, 3, OrderStatus.Shipped,    PaymentStatus.Paid,    new[] { (10, 1), (9, 1) }),
            new( 6, 4, OrderStatus.Shipped,    PaymentStatus.Paid,    new[] { (4, 3) }),
            new( 4, 5, OrderStatus.Processing, PaymentStatus.Paid,    new[] { (7, 1), (8, 1) }),
            new( 3, 0, OrderStatus.Processing, PaymentStatus.Paid,    new[] { (2, 2), (11, 1) }),
            new( 2, 1, OrderStatus.Pending,    PaymentStatus.Pending, new[] { (14, 1), (13, 1) }),
            new( 1, 2, OrderStatus.Pending,    PaymentStatus.Pending, new[] { (0, 1), (6, 1) }),
            new( 0, 3, OrderStatus.Cancelled,  PaymentStatus.Pending, new[] { (1, 1) })
        };

        var sequencePerDay = new Dictionary<DateTime, int>();

        foreach (var seed in orderSeeds)
        {
            var orderDate = now.AddDays(-seed.DaysAgo);
            var dayKey = orderDate.Date;
            sequencePerDay.TryGetValue(dayKey, out var seq);
            sequencePerDay[dayKey] = ++seq;

            var items = seed.Items.Select(x =>
            {
                var product = products[x.productIdx];
                if (seed.Status != OrderStatus.Cancelled)
                {
                    product.StockQuantity -= x.qty;
                    product.UpdatedAt = orderDate;
                }
                return new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = x.qty,
                    UnitPrice = product.Price,
                    LineTotal = product.Price * x.qty,
                    CreatedAt = orderDate
                };
            }).ToList();

            var subtotal = items.Sum(i => i.LineTotal);
            const decimal shipping = 9.99m;
            var total = subtotal + shipping;

            context.Orders.Add(new Order
            {
                OrderNumber = $"ORD-{dayKey:yyyyMMdd}-{seq:D3}",
                OrderDate = orderDate,
                CustomerId = customers[seed.CustomerIdx].Id,
                Status = seed.Status,
                PaymentStatus = seed.PaymentStatus,
                Subtotal = subtotal,
                ShippingAmount = shipping,
                DiscountAmount = 0m,
                TotalAmount = total,
                OrderItems = items,
                CreatedAt = orderDate,
                UpdatedAt = seed.Status is OrderStatus.Delivered or OrderStatus.Shipped or OrderStatus.Cancelled ? orderDate.AddHours(2) : null
            });
        }

        await context.SaveChangesAsync();
    }

    private record OrderSeed(int DaysAgo, int CustomerIdx, OrderStatus Status, PaymentStatus PaymentStatus, (int productIdx, int qty)[] Items);
}
