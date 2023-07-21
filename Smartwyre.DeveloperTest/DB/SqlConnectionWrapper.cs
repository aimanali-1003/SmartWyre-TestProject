using Smartwyre.DeveloperTest;
using System.Data.SqlClient;

public class SqlConnectionWrapper : IDbConnectionWrapper
{
    private readonly SqlConnection sqlConnection;
    public SqlConnectionWrapper()
    {
        string connectionString = Constants.connectionString;
        sqlConnection = new SqlConnection(connectionString);
    }

    public void Open()
    {
        sqlConnection.Open();
    }

    public void Close()
    {
        sqlConnection.Close();
    }

    public SqlCommand CreateCommand()
    {
        return sqlConnection.CreateCommand();
    }
}
