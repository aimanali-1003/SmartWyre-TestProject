using System.Data.SqlClient;

public interface IDbConnectionWrapper
{
    void Open();
    void Close();
    SqlCommand CreateCommand();
}
