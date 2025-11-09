using CatalogLoader.Messaging;
using CatalogLoader.Services;
using Microsoft.EntityFrameworkCore;
using PriceWatcher.Data;
using PriceWatcher.Data.Repositories;


var builder = WebApplication.CreateBuilder(args);

// ---- DbContext ----
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ---- Repositories ----
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<PriceHistoryRepository>();

// ---- HttpClient ----
builder.Services.AddHttpClient();

builder.Services.AddSingleton<OnlinerClient>();

// ---- Background Service ----
builder.Services.AddHostedService<OnlinerBackgroundService>();
builder.Services.AddHostedService<PriceTrackingService>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<OnlinerBackgroundService>());

// ---- Controllers ----
builder.Services.AddControllers();

// ---- Swagger ----
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
