using System;
using System.Threading.Tasks;
using Domain.Core;
using Domain.Services.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


class Program
{
    private static async Task Main(string[] args)
    {
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddFilter("NonHostConsoleApp.Program", LogLevel.Debug)
                .AddConsole();
        });
        ILogger logger = loggerFactory.CreateLogger<Program>();

        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        DbContextOptionsBuilder<TaskManagerDbContext> optionsBuilder =
            new DbContextOptionsBuilder<TaskManagerDbContext>();
        optionsBuilder.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);


        await using TaskManagerDbContext db = new TaskManagerDbContext(optionsBuilder.Options);
        try
        {
            await DbInitializer.DbInitializer.Initialize(configuration, db, new EncryptService(), logger);
        }
        catch (Exception e)
        {
            logger.Log(LogLevel.Error, "Error during DBInitializer Process -{0}", e);
        }
    }
}