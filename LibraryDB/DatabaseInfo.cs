using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDB;

public static class DatabaseInfo
{
    public static DataTable GetIdColumnsInfo(Database database, string tableName)
    {
        var query = $@"
            SELECT column_name
            FROM information_schema.columns
            WHERE table_name = '{tableName}'
                AND column_default LIKE 'nextval%'
            ORDER BY ordinal_position
        ";

        return DatabaseQueryExecutor.GetDataTable(database, query);
    }

    public static DataTable GetNonIdColumnsInfo(Database database, string tableName)
    {
        var query = $@"
            SELECT column_name, data_type, character_maximum_length
            FROM information_schema.columns
            WHERE table_name = '{tableName}'
            AND (column_default IS NULL OR 
                 column_default NOT LIKE 'nextval%')
            ORDER BY ordinal_position
        ";

        return DatabaseQueryExecutor.GetDataTable(database, query);
    }

    public static DataTable GetAllColumnsInfo(Database database, string tableName)
    {
        var query = $@"
            SELECT column_name, data_type, character_maximum_length
            FROM information_schema.columns
            WHERE table_name = '{tableName}'
            ORDER BY ordinal_position
        ";

        return DatabaseQueryExecutor.GetDataTable(database, query);
    }

    public static DataTable GetPrimaryKeysInfo(Database database, string tableName)
    {
        var query = $@"
            SELECT kcu.column_name, c.data_type, c.character_maximum_length
                FROM information_schema.table_constraints tc
                JOIN information_schema.key_column_usage kcu
                    ON tc.constraint_name = kcu.constraint_name
                    AND tc.table_schema = kcu.table_schema
                    AND tc.table_name = kcu.table_name
                JOIN information_schema.columns c
                    ON kcu.column_name = c.column_name
    	            AND kcu.table_schema = c.table_schema
   		            AND kcu.table_name = c.table_name
                WHERE tc.constraint_type = 'PRIMARY KEY'
                AND tc.table_name = '{tableName}';
        ";

        return DatabaseQueryExecutor.GetDataTable(database, query);
    }

    public static DataTable GetForeignKeysInfo(Database database, string tableName)
    {
        var query = $@"
            SELECT
                kcu.column_name AS foreign_key_column,
                ccu.table_name AS referenced_table,
                ccu.column_name AS referenced_column
            FROM
                information_schema.table_constraints tc
            JOIN
                information_schema.key_column_usage kcu
                ON tc.constraint_name = kcu.constraint_name
            JOIN
                information_schema.referential_constraints rc
                ON tc.constraint_name = rc.constraint_name
            JOIN
                information_schema.constraint_column_usage ccu
                ON rc.unique_constraint_name = ccu.constraint_name
            WHERE
                tc.constraint_type = 'FOREIGN KEY'
                AND tc.table_name = '{tableName}';
        ";

        return DatabaseQueryExecutor.GetDataTable(database, query);
    }

    public static Dictionary<string, string> GetDefaultValues(Database database, string tableName)
    {
        var defaultColumns = new Dictionary<string, string>();

        string query = $@"
            SELECT
                column_name,
                column_default
            FROM
                information_schema.columns
            WHERE
                table_name = '{tableName}'
                AND column_default IS NOT NULL;
        ";

        var defaultTable = DatabaseQueryExecutor.GetDataTable(database, query);
        foreach (DataRow row in defaultTable.Rows)
        {
            var columnName = row["column_name"].ToString();
            var defaultValue = ProcessDefaultValue(database, row["column_default"].ToString());
            defaultColumns[columnName] = defaultValue;
        }

        return defaultColumns;
    }

    private static string ProcessDefaultValue(Database database, string defaultValue)
    {
        foreach (var typeName in NpgsqlTypeMatcher.GetSqlTypeNames())
        {
            if (defaultValue.EndsWith("::" + typeName))
            {
                defaultValue = defaultValue.Substring(0, defaultValue.Length - (typeName.Length + 2));
            }
        }

        if (defaultValue.StartsWith('\'') && defaultValue.EndsWith('\''))
        {
            return defaultValue.Substring(1, defaultValue.Length - 2);
        }

        if (defaultValue.StartsWith("nextval"))
        {
            return GetNextVal(database, defaultValue);
        }

        return defaultValue;
    }

    private static string GetNextVal(Database database, string sequence)
    {
        var begin = sequence.IndexOf('\'');
        var end = sequence.LastIndexOf('\'');
        var sequenceName = sequence.Substring(begin + 1, end - begin - 1);

        var query = $@"SELECT (CASE WHEN is_called 
                               THEN last_value + 1 
                               ELSE last_value END ) AS nextvalue
                       FROM {sequenceName}";

        return DatabaseQueryExecutor.GetScalar(database, query).ToString();
    }

    public static IEnumerable<string?> GetColumnsNamesFromTable(DataTable table)
    {
        return table.Rows
            .Cast<DataRow>()
            .Select(row => row["column_name"].ToString());
    }

    public static IEnumerable<string?> GetIdColumnsNames(Database database, string tableName)
    {
        return GetColumnsNamesFromTable(GetIdColumnsInfo(database, tableName));
    }

    public static IEnumerable<string?> GetNonIdColumnsNames(Database database, string tableName)
    {
        return GetColumnsNamesFromTable(GetNonIdColumnsInfo(database, tableName));
    }

    public static IEnumerable<string?> GetAllColumnsNames(Database database, string tableName)
    {
        return GetColumnsNamesFromTable(GetAllColumnsInfo(database, tableName));
    }

    public static IEnumerable<string?> GetPrimaryKeysNames(Database database, string tableName)
    {
        return GetColumnsNamesFromTable(GetPrimaryKeysInfo(database, tableName));
    }

    public static IEnumerable<string?> GetForeignKeysNames(Database database, string tableName)
    {
        return GetColumnsNamesFromTable(GetForeignKeysInfo(database, tableName));
    }
}
