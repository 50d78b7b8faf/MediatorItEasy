using MediatorItEasy.Engine;
using MediatorItEasy.Extensions;
using MediatorItEasy.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

class Program
{
    private static IHost _host;

    static async Task Main(string[] args)
    {
        BuildHost();
        IMediator mediator = _host.Services.GetService<IMediator>()!;

        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var resultReflection = await mediator.SendWithReflection(new GetUserQuery { Id = 1 });
        stopwatch.Stop();
        Console.WriteLine($"SendWithReflection tomó: {stopwatch.ElapsedMilliseconds} ms");
        stopwatch.Reset();

        stopwatch.Start();
        var resultDictionary = await mediator.SendWithDictionary(new GetUserQuery { Id = 1 });
        stopwatch.Stop();
        Console.WriteLine($"SendWithDictionary tomó: {stopwatch.ElapsedMilliseconds} ms");

        Console.ReadKey();
    }

    private static IConfiguration GetConfiguration()
    {
        return new ConfigurationBuilder().Build();
    }

    private static void BuildHost()
    {
        var configuration = GetConfiguration();
        _host = new HostBuilder()
            .ConfigureServices((hostContext, serviceCollection) => serviceCollection.InstallServices(configuration))
            .UseConsoleLifetime()
            .Build();
    }
}