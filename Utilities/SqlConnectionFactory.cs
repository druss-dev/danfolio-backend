using Microsoft.Data.SqlClient;

namespace DanfolioBackend.Utilities;

// Currently not used since we don't have an active database
public class SqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SqlConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}
