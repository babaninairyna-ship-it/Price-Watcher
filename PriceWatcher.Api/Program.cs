using CatalogLoader.Interfaces;
using CatalogLoader.Messaging;
using CatalogLoader.Services;
using Microsoft.EntityFrameworkCore;
using PriceWatcher.Api.Hubs;
using PriceWatcher.Api.Messaging;
using PriceWatcher.Api.Services;
using PriceWatcher.Data;
using PriceWatcher.Data.Interfaces;
using PriceWatcher.Data.Repositories;
using PriceWatcher.Services;
using PriceWatcher.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ---- Controllers ----
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ---- DbContext ----
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ---- HttpClient ----
builder.Services.AddHttpClient();

// Repositories
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<PriceHistoryRepository>();

// Services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IPriceChangeProcessor, PriceChangeProcessor>();

// RabbitMQ subscriber
builder.Services.AddSingleton<OnlinerClient>();
builder.Services.AddSingleton<IMessageSubscriber, RabbitMqSubscriber>();
builder.Services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();


// Background Services
builder.Services.AddHostedService<PriceChangeConsumerService>();
builder.Services.AddHostedService<PriceTrackingService>();

builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseStaticFiles();

app.MapHub<PriceChangeHub>("/priceChangeHub");

app.MapControllers();

app.Run();
