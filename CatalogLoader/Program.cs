using CatalogLoader.Interfaces;
using CatalogLoader.Messaging;
using CatalogLoader.Services;
using Microsoft.EntityFrameworkCore;
using PriceWatcher.Data;
using PriceWatcher.Data.Interfaces;
using PriceWatcher.Data.Repositories;
using PriceWatcher.Services;

var builder = Host.CreateApplicationBuilder(args);

// ---- DbContext ----
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// ---- Repositories ----
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IPriceHistoryRepository, PriceHistoryRepository>();

// ---- HttpClient ----
builder.Services.AddHttpClient();

// ---- Singleton services ----
builder.Services.AddSingleton<OnlinerClient>();

// ---- Background services ----
builder.Services.AddHostedService<OnlinerBackgroundService>();

builder.Services.AddScoped<IPriceChangeProcessor, PriceChangeProcessor>();
builder.Services.AddHostedService<PriceTrackingService>();

var app = builder.Build();

app.Run();
