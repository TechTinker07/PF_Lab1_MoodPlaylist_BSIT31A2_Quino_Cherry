using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShoeShop.Repository;
using ShoeShop.Repository.Entities;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<ShoeShopDbContext>(options =>
            options.UseSqlite("Data Source=shoeshop.db"));
    })
    .Build();

using var scope = host.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<ShoeShopDbContext>();

Console.WriteLine("Applying migrations...");
db.Database.Migrate();
Console.WriteLine("Database ready!\n");

// --- 1. List all Shoes & Color Variations ---
Console.WriteLine("=== Shoes & Color Variations ===");
var shoes = db.Shoes
    .Include(s => s.ColorVariations)
    .ToList();

foreach (var s in shoes)
{
    Console.WriteLine($"{s.Id}. {s.Name} ({s.Brand}) - ₱{s.Price}");
    foreach (var cv in s.ColorVariations)
    {
        Console.WriteLine($"\tColor: {cv.ColorName} | Stock: {cv.StockQuantity}");
    }
}
Console.WriteLine();

// --- 2. List all Suppliers ---
Console.WriteLine("=== Suppliers ===");
var suppliers = db.Suppliers.ToList();
foreach (var sup in suppliers)
{
    Console.WriteLine($"{sup.Id}. {sup.Name}");
}
Console.WriteLine();

// --- 3. List Purchase Orders & Items ---
Console.WriteLine("=== Purchase Orders ===");
var orders = db.PurchaseOrders
    .Include(po => po.Items)
        .ThenInclude(i => i.Shoe)
    .Include(po => po.Supplier)
    .ToList();

foreach (var po in orders)
{
    Console.WriteLine($"PO #{po.Id} - Supplier: {po.Supplier.Name} | Status: {po.Status} | Date: {po.OrderDate.ToShortDateString()}");
    foreach (var item in po.Items)
    {
        Console.WriteLine($"\tShoe: {item.Shoe.Name} | Qty: {item.Quantity} | UnitCost: ₱{item.UnitCost}");
    }
}
Console.WriteLine();

// --- 4. List Stock Pull-Outs ---
Console.WriteLine("=== Stock Pull-Outs ===");
var pullOuts = db.StockPullOuts
    .Include(sp => sp.Shoe)
    .ToList();

foreach (var sp in pullOuts)
{
    Console.WriteLine($"{sp.Id}. {sp.Shoe.Name} | Qty: {sp.Quantity} | Reason: {sp.Reason} | Date: {sp.DatePulledOut.ToShortDateString()}");
}

Console.WriteLine("\nAll data displayed successfully!");
