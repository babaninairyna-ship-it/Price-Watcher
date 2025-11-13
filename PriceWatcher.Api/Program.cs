using Microsoft.EntityFrameworkCore;
using PriceWatcher.Api.Hubs;
using PriceWatcher.Api.Messaging;
using PriceWatcher.Api.Services;
using PriceWatcher.Data;
using PriceWatcher.Data.Repositories;

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

// Scoped services
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<PriceHistoryRepository>();
builder.Services.AddScoped<PriceChangedHandler>();

// Hosted service 
builder.Services.AddHostedService<PriceChangeConsumerService>();

// RabbitMQ subscriber
builder.Services.AddSingleton<IMessageSubscriber, RabbitMqSubscriber>();

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
