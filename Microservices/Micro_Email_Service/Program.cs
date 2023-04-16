using Micro_Email_Service;
using Micro_Email_Service.Interfaces;
using Micro_Email_Service.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

public class Program
{
    static void Main(string[] args)
    {
        var host = AppStartup();

        var consumerService = ActivatorUtilities.CreateInstance<MessageConsumer>(host.Services);

        consumerService.Consume();
    }

    static void ConfigSetup(IConfigurationBuilder builder)
    {
        builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
    }

    static IHost AppStartup()
    {
        var builder = new ConfigurationBuilder();
        ConfigSetup(builder);

        // defining serilog configs
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Build())
            .Enrich.FromLogContext()
            .CreateLogger();

        // Initiated the dependancy injection container
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<IMessageConsumer, MessageConsumer>();
                services.AddSingleton(context.Configuration);
                services.AddTransient<IEmailSendService, EmailSendService>();
            })
            .UseSerilog()
            .Build();

        return host;
    }
}