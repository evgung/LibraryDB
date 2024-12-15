using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryDB;

public class DataTableAdapter
{
    private readonly NpgsqlDataAdapter dataAdapter;
    public DataTable DataTable { get; }
    public string TableName { get; }

    public DataTableAdapter(Database database, string tableName)
    {
        DataTable = new DataTable();
        dataAdapter = new DataAdapterCreator(database, tableName).Create();
        TableName = tableName;
    }

    public void FillDataTable()
    {
        dataAdapter.Fill(DataTable);
    }

    public void UpdateAllData()
    {
        try
        {
            dataAdapter.Update(DataTable);
        }
        catch (PostgresException e)
        {
            MessageBox.Show($"Error: {e.Message}");
        }
    }
}
