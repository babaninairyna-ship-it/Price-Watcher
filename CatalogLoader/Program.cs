IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddHttpClient();
        services.AddHostedService<OnlinerBackgroundService>();
    })
    .Build();

await host.RunAsync();
