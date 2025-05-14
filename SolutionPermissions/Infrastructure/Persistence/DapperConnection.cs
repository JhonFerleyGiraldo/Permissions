using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Infrastructure.Persistence
{
    public class DapperConnection : IDapperConnection
    {

        private readonly string _connectionString;

        public DapperConnection(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Dapper");
        }


        public async Task<List<Permission>> GetAllPermissionsAsync()
        {
            List<Permission> response = new();
            const string sql = "SP_GetPermissions"; 

            using IDbConnection db = new SqlConnection(_connectionString);
            var permissions = await db.QueryAsync<Permission>(sql);

            if (permissions.Any())
            {
                response = permissions.ToList();
            }

            return response;
        }

    }
}
