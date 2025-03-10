using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace LibraryDB;

public class Database
{
    private readonly string connectionString =
        "Server=localhost; Port=5432; Database=Library; User Id=postgres; Password=postgres";

    public NpgsqlConnection SqlConnection { get; }

    public Database()
    {
        SqlConnection = new NpgsqlConnection(connectionString);
    }

    public void OpenConnection()
    {
        if (SqlConnection.State == System.Data.ConnectionState.Closed)
        {
            SqlConnection.Open();
        }
    }

    public void CloseConnection()
    {
        if (SqlConnection.State == System.Data.ConnectionState.Open)
        {
            SqlConnection.Close();
        }
    }
}
