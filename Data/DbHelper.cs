using Microsoft.Data.SqlClient;
using System.Data;

namespace MvcWithApi.Data
{
    public class DbHelper
    {
        private readonly string _connectionString;
        public DbHelper(string connectionString)
        {
            _connectionString = connectionString;
        }


        // Open Connection (caller must dispose)
        public async Task<SqlConnection> GetConnectionAsync()
        {
            var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            return conn;
        }

        // Create Command for stored procedure
        public SqlCommand CreateCommand(SqlConnection conn, string procName)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = procName;
            cmd.CommandType = CommandType.StoredProcedure;
            return cmd;
        }

        //Add Parameters safely
        public void AddParameter(SqlCommand cmd, string name, SqlDbType type, object? value, int size = 0)
        {
            var param = cmd.Parameters.Add(name, type);
            if (size > 0) param.Size = size;
            param.Value = value ?? DBNull.Value;
        }

        //Execoute StoreProcedure that returns DataTable
        public async  Task<DataTable> ExecuteDataTableAsync(SqlCommand cmd)
        {
            using var reader = await cmd.ExecuteReaderAsync();
            var table = new DataTable();
            table.Load(reader);
            return table;
        }

        //Execute StoreProc that Modify Insert/Update/Delete (Select)
        public async Task<int> ExecuteNonQueryAsync(SqlCommand cmd)
        {
            return await cmd.ExecuteNonQueryAsync();
        }

        //Execute StoreProc that return single value
        public async Task<object>? ExecuteScalarAsync(SqlCommand cmd)
        {
            return await cmd.ExecuteScalarAsync();
        }
    }
}

