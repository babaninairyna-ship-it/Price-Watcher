using CatalogLoader.Messaging;
using CatalogLoader.Services;
using Microsoft.EntityFrameworkCore;
using PriceWatcher.Data;
using PriceWatcher.Data.Repositories;

var builder = Host.CreateApplicationBuilder(args);

// ---- DbContext ----
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// ---- Repositories ----
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<PriceHistoryRepository>();

// ---- HttpClient ----
builder.Services.AddHttpClient();

// ---- Singleton services ----
builder.Services.AddSingleton<OnlinerClient>();
builder.Services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();

// ---- Background services ----
builder.Services.AddHostedService<OnlinerBackgroundService>();
builder.Services.AddHostedService<PriceTrackingService>();

var app = builder.Build();

app.Run();
