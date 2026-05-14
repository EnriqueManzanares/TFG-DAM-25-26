using MySql.Data.MySqlClient;

public abstract class RepositoryBase
{
    private readonly string _connectionString;

    public RepositoryBase()
    {
       
        _connectionString = "Server=localhost;Port=3306;Database=inamanager;Uid=root;Pwd=root;";
    }

    protected MySqlConnection GetConnection()
    {
        return new MySqlConnection(_connectionString);
    }
}
