# StoreOps

Lightweight e-commerce admin panel for small-to-mid-sized stores. Built as a hands-on study of production-grade .NET: layered architecture, EF Core, and Blazor Server — from domain model all the way to UI.

## Stack

| Layer | Tech |
|-------|------|
| UI | Blazor Server (.NET 10), interactive server render mode, Bootstrap 5 |
| Services | C# class library, `Result<T>` pattern, DTOs, manual mapping |
| Data | EF Core 10, SQL Server LocalDB, `IEntityTypeConfiguration<T>`, `IDbContextFactory` |
| Models | Plain POCOs — zero EF dependency (Fluent API only) |

## Architecture

Four projects with strict one-way dependency flow:

```
StoreOps (Web) ──▶ StoreOps.Services ──▶ StoreOps.Data ──▶ StoreOps.Models
```

- **StoreOps.Models** — domain entities, enums, `BaseEntity` (Id, CreatedAt, UpdatedAt, IsActive). No EF attributes — the domain knows nothing about how it's persisted.
- **StoreOps.Data** — `AppDbContext`, one `IEntityTypeConfiguration<T>` per entity, generic + specific repositories, `IDbContextFactory<AppDbContext>` for Blazor circuit safety.
- **StoreOps.Services** — business rules, validation, DTOs (read/create/update), `Result<T>` pattern for predictable failures without exceptions.
- **StoreOps** — Blazor Server UI, DI wire-up, connection-string config.

## Domain

Core entities (see `StoreOps.Models`):
`Category`, `Product`, `Customer`, `Coupon`, `Order`, `OrderItem`, `Payment`, `InventoryTransaction`.

Design choices:
- Soft delete via `IsActive` on every entity.
- Money stored as `decimal` with `HasPrecision(18, 2)` — never `double`/`float`.
- Aggregate roots: `Order` owns `OrderItem` + `Payment` (Cascade). `Product` / `Category` deletes are **Restricted** to preserve historical order data.
- Snapshot pricing: `OrderItem.UnitPrice` is copied at order time so past orders don't mutate when product prices change.

## Patterns used

- **Clean / layered architecture** with a DI extension per layer (`AddDataServices`, `AddApplicationServices`)
- **Repository pattern** — generic `IRepository<T>` + specific interfaces where named queries are needed
- **DTO boundary** — UI never touches entities
- **`Result<T>`** — business failures are data, not exceptions
- **`IDbContextFactory<T>`** — short-lived contexts per operation, avoids the Blazor long-lived-context trap (tracker bloat, stale data)
- **Fluent API** via `IEntityTypeConfiguration<T>` + `ApplyConfigurationsFromAssembly` — one config file per entity
- **Layered validation** — DataAnnotations on DTOs for client-side UX; service re-validates server-side for correctness

## Running locally

Prerequisites:
- .NET 10 SDK
- SQL Server LocalDB

```bash
# one-time: create and start a LocalDB instance named "Projects"
sqllocaldb create Projects
sqllocaldb start Projects

# copy the dev settings template (it is gitignored)
cp StoreOps/appsettings.Development.json.example StoreOps/appsettings.Development.json

# restore, apply migrations, run
dotnet restore
dotnet ef database update --project StoreOps.Data --startup-project StoreOps
dotnet run --project StoreOps
```

Default connection string points at `Server=(localdb)\Projects;Database=StoreOps;Trusted_Connection=True`.

## Status

- [x] Domain model (8 entities, 5 enums, all EF configurations)
- [x] Data layer (generic + specific repositories, DbContext, migrations)
- [x] Service layer template (`Result<T>`, DTOs, mapping) — Category module complete
- [x] Category CRUD UI (list, create, edit, soft-delete)
- [ ] Product module
- [ ] Customer, Coupon, Order, OrderItem, Payment, InventoryTransaction
- [ ] Dashboard / reports
- [ ] ASP.NET Identity + role-based authorization
- [ ] UI polish (MudBlazor or Radzen)

## Branching

- `main` — known-good, always runnable
- `dev` — integration branch; features merge here first
- `feature/<name>` — per-module work (e.g. `feature/product-module`)

## License

Portfolio / learning project.
