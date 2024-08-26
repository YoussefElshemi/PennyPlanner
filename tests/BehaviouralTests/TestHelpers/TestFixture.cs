using System.Data.Common;
using FastEndpoints.Testing;
using Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Presentation.Constants;
using Respawn;

namespace BehaviouralTests.TestHelpers;

public class TestFixture : AppFixture<Presentation.Program>
{
    private DbConnection _dbConnection = default!;
    private Respawner _respawner = default!;

    protected override async Task SetupAsync()
    {
        _dbConnection = new NpgsqlConnection(ConfigurationHelper.GetConnectionString(ConnectionNames.DatabaseName));
        await _dbConnection.OpenAsync();

        _respawner = await RespawnerHelper.CreateRespawnerAsync(_dbConnection);
    }

    protected override void ConfigureApp(IWebHostBuilder a)
    {
        a.UseEnvironment("Development");
    }

    protected override async Task TearDownAsync()
    {
        await _dbConnection.CloseAsync();
    }

    public async Task SeedDatabaseAsync()
    {
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PennyPlannerDbContext>();
        await context.SaveChangesAsync();
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }
}