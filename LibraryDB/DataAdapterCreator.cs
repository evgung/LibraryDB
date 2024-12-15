using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LibraryDB;

public class DataAdapterCreator
{
    private readonly Database database;
    private readonly string tableName;
    private readonly DataTable columnsInfo;
    private readonly DataTable primaryKeys;

    public DataAdapterCreator(Database database, string tableName)
    {
        this.database = database;
        this.tableName = tableName;
        columnsInfo = DatabaseInfo.GetNonIdColumnsInfo(database, tableName);
        primaryKeys = DatabaseInfo.GetPrimaryKeysInfo(database, tableName);
    }

    public NpgsqlDataAdapter Create()
    {
        var selectQuery = @$"SELECT * FROM {tableName}
                             ORDER BY ({GetColumnList(primaryKeys)})";
        var adapter = new NpgsqlDataAdapter(selectQuery, database.SqlConnection);
        
        GenerateInsertCommand(adapter);
        GenerateUpdateCommand(adapter);
        GenerateDeleteCommand(adapter);

        return adapter;
    }

    private void GenerateInsertCommand(NpgsqlDataAdapter adapter)
    {
        var columnList = GetColumnList(columnsInfo);
        var parameterList = GetParameterList(columnsInfo);
        var insertQuery = $"INSERT INTO {tableName} ({columnList}) VALUES ({parameterList})";

        adapter.InsertCommand = new NpgsqlCommand(insertQuery, database.SqlConnection);
        AddParametersToCommand(adapter.InsertCommand, columnsInfo);
    }

    private void GenerateUpdateCommand(NpgsqlDataAdapter adapter)
    {
        var columnsSetters = columnsInfo.Rows
            .Cast<DataRow>()
            .Select(row => $"{row["column_name"]} = @{row["column_name"]}");

        var setClause = string.Join(", ", columnsSetters);
        var whereClause = GenerateWhereClause();
        var updateQuery = $"UPDATE {tableName} SET {setClause} WHERE {whereClause}";

        adapter.UpdateCommand = new NpgsqlCommand(updateQuery, database.SqlConnection);
        AddParametersToCommand(adapter.UpdateCommand, columnsInfo);
        AddParametersToCommand(adapter.UpdateCommand, primaryKeys);
    }

    private void GenerateDeleteCommand(NpgsqlDataAdapter adapter)
    {
        var whereClause = GenerateWhereClause();
        var deleteQuery = $"DELETE FROM {tableName} WHERE {whereClause}";

        adapter.DeleteCommand = new NpgsqlCommand(deleteQuery, database.SqlConnection);
        AddParametersToCommand(adapter.DeleteCommand, primaryKeys);
    }

    private void AddParametersToCommand(NpgsqlCommand command, DataTable fromTable)
    {
        foreach (DataRow row in fromTable.Rows)
        {
            var columnName = row["column_name"].ToString();
            var columnType = row["data_type"].ToString();
            int size;

            if (row.IsNull("character_maximum_length"))
            {
                size = 0;
            }
            else
            {
                size = Convert.ToInt32(row["character_maximum_length"]);
            }

            command.Parameters.Add(
                $"@{columnName}",
                NpgsqlTypeMatcher.GetNpgsqlDbType(columnType),
                size,
                columnName
            );
        }
    }

    private string GetColumnList(DataTable table)
    {
        var columnsNames = DatabaseInfo.GetColumnsNamesFromTable(table);
        return string.Join(", ", columnsNames);
    }

    private string GetParameterList(DataTable table)
    {
        var columnsNames = DatabaseInfo.GetColumnsNamesFromTable(table);
        return string.Join(", ", columnsNames.Select(col => $"@{col}"));
    }

    private string GenerateWhereClause()
    {
        var columnList = GetColumnList(primaryKeys);
        var parameterList = GetParameterList(primaryKeys);
        return  $"({columnList}) = ({parameterList})";
    }
}
