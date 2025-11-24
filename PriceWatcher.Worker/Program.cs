using Microsoft.EntityFrameworkCore;
using PriceWatcher.Data;
using PriceWatcher.Data.API.Onliner;
using PriceWatcher.Data.Repositories;
using PriceWatcher.Domain;
using PriceWatcher.Services;
using PriceWatcher.Services.Interfaces;
using PriceWatcher.Worker.Services;

var builder = Host.CreateApplicationBuilder(args);

// ---- DbContext ----
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ---- HttpClient ----
builder.Services.AddHttpClient();

// ---- Singleton services ----
builder.Services.AddSingleton<ICatalogClient, OnlinerClient>();
builder.Services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();

// ---- Repositories ----
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IPriceHistoryRepository, PriceHistoryRepository>();
builder.Services.AddScoped<IPriceChangeProcessor, PriceChangeProcessor>();

// ---- Background services ----
builder.Services.AddHostedService<OnlinerBackgroundService>();
builder.Services.AddHostedService<PriceTrackingService>();

var app = builder.Build();

app.Run();
