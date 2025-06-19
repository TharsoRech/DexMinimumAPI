using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DexMinimumApi.Context;
public class DapperContext : IDapperContext
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
       public DapperContext(IConfiguration configuration)
        {
          _configuration = configuration;
          _connectionString = _configuration.GetConnectionString("SqlConnection"); ;
        }

        public async Task<int> ExecuteAsync(string sql, object entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.ExecuteAsync(sql, entity);
            }
        }

        public async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QuerySingleOrDefaultAsync(sql, entity);
            }
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object entity = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<T>(sql, entity);
            }
        }

        public async Task<T> QueryFirstAsync<T>(string sql, object entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryFirstAsync(sql, entity);
            }
        }

       public async Task<int> ExecuteStoreProcedure(string sql, object entity)
       {
          using (var connection = new SqlConnection(_connectionString))
          {
            return await connection.ExecuteScalarAsync<int>(sql, entity, commandType: CommandType.StoredProcedure);
          }
       }


}