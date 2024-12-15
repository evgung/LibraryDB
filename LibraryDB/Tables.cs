using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDB;

public class Tables
{
    public List<string> TablesNames { get; } = new List<string>();
    public List<string> ViewsNames { get; } = new List<string>();

    public Tables(Database database)
    {
        DefineTablesNames(database);
        DefineViewsNames(database);
    }

    private void DefineTablesNames(Database database)
    {
        var query = @"
            SELECT table_name
            FROM information_schema.tables
            WHERE table_schema = 'public'
                AND table_type = 'BASE TABLE'
            ORDER BY table_name;
        ";

        using (var reader = DatabaseQueryExecutor.GetReader(database, query))
        {
            while (reader.Read())
            {
                TablesNames.Add(reader.GetString(0));
            }
        }
    }

    private void DefineViewsNames(Database database)
    {
        var query = @"
            SELECT table_name
            FROM information_schema.tables
            WHERE table_schema = 'public'
                AND table_type = 'VIEW'
            ORDER BY table_name;
        ";

        using (var reader = DatabaseQueryExecutor.GetReader(database, query))
        {
            while (reader.Read())
            {
                ViewsNames.Add(reader.GetString(0));
            }
        }
    }
}
