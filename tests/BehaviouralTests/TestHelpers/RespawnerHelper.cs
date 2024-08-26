using System.Data.Common;
using Infrastructure.Configurations;
using Respawn;

namespace BehaviouralTests.TestHelpers;

public static class RespawnerHelper
{
    public static async Task<Respawner> CreateRespawnerAsync(DbConnection dbConnection)
    {
        return await Respawner.CreateAsync(dbConnection,
            new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = ["public"],
                TablesToIgnore = [
                    UserRoleEntityConfiguration.TableName
                ]
            });
    }
}