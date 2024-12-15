using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDB;

public static class DatabaseQueryExecutor
{
    public static NpgsqlDataReader GetReader(Database database, string query)
    {
        using (var command = new NpgsqlCommand(query, database.SqlConnection))
        {
            var reader = command.ExecuteReader();
            return reader;
        }
    }

    public static object? GetScalar(Database database, string query)
    {
        using (var command = new NpgsqlCommand(query, database.SqlConnection))
        {
            return command.ExecuteScalar();
        }
    }

    public static DataTable GetDataTable(Database database, string query)
    {
        using (var reader = GetReader(database, query))
        {
            var dataTable = new DataTable();
            dataTable.Load(reader);
            return dataTable;
        }
    }
}
