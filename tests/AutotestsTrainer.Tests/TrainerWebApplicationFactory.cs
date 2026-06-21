using AutotestsTrainer.Web.Data;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AutotestsTrainer.Tests;

public sealed class TrainerWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _databasePath = Path.Combine(Path.GetTempPath(), $"autotests-{Guid.NewGuid():N}.db");
    private readonly string _dataProtectionPath = Path.Combine(Path.GetTempPath(), $"autotests-keys-{Guid.NewGuid():N}");

    public string DatabasePath => _databasePath;

    public TrainerWebApplicationFactory()
    {
        if (File.Exists(_databasePath))
        {
            File.Delete(_databasePath);
        }
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        builder.ConfigureLogging(logging => logging.ClearProviders());
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            Directory.CreateDirectory(_dataProtectionPath);

            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(_dataProtectionPath));

            services.AddDbContext<AppDbContext>(options => options.UseSqlite($"Data Source={_databasePath}"));
        });
    }
}
