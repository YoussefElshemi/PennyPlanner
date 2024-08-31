using System.Data.Common;
using BehaviouralTests.Mocks;
using FastEndpoints.Testing;
using Infrastructure.Interfaces.Services;
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
        a.UseConfiguration(ConfigurationHelper.Config);
    }

    protected override void ConfigureServices(IServiceCollection s)
    {
        s.AddSingleton<ISmtpClient, MockSmtpClient>();
    }

    protected override async Task TearDownAsync()
    {
        await _dbConnection.CloseAsync();
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }
}