using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;

namespace LibraryDB;

public class DataVisualizer
{
    private readonly Database database;
    private readonly DataGridView dataGridView;
    private readonly Filtrator filtrator;

    private DataTableAdapter dataTableAdapter;

    public DataVisualizer(Database database, DataGridView dataGridView, Panel filterRowPanel)
    {
        this.database = database;
        this.dataGridView = dataGridView;
        filtrator = new Filtrator(dataGridView, filterRowPanel);
    }

    public virtual void SetNewDataSource(DataTableAdapter dataTableAdapter)
    {
        this.dataTableAdapter = dataTableAdapter;
        filtrator.Reset(dataTableAdapter.DataTable);
    }

    public virtual void Visualize()
    {
        AddIdColumn();
        AddForeignColumns();
        dataTableAdapter.FillDataTable();

        filtrator.SetFilters();
    }

    private void AddIdColumn()
    {
        var idColumns = DatabaseInfo.GetIdColumnsNames(database, dataTableAdapter.TableName);

        foreach (var columnName in idColumns)
        {
            var column = new DataGridViewTextBoxColumn
            {
                Name = columnName,
                ReadOnly = true,
                DataPropertyName = columnName,
            };

            dataGridView.Columns.Add(column);
        }
    }

    private void AddForeignColumns()
    {
        var foreignKeys = DatabaseInfo.GetForeignKeysInfo(database, dataTableAdapter.TableName);

        foreach (DataRow row in foreignKeys.Rows)
        {
            var foreignKeyColumnName = row["foreign_key_column"].ToString();
            var referencedTableName = row["referenced_table"].ToString();
            var referencedColumnName = row["referenced_column"].ToString();

            var referencedTable = GetReferencedTable(referencedTableName);

            var comboBoxColumn = new DataGridViewComboBoxColumn
            {
                Name = foreignKeyColumnName,
                DataSource = referencedTable,                // Имя ссылочной таблицы
                DataPropertyName = foreignKeyColumnName,     // Имя столбца в ссылающейся (этой) таблице
                ValueMember = referencedColumnName,          // Имя столбца, на который ссылаются в ссылочной таблице
                DisplayMember = "ColumnForDisplay",          // Имя столбца для отображения из ссылочной таблицы
            };

            dataGridView.Columns.Add(comboBoxColumn);
        }
    }

    private DataTable GetReferencedTable(string referencedTableName)
    {
        var query = $"SELECT * FROM {referencedTableName}";
        var referencedTable = DatabaseQueryExecutor.GetDataTable(database, query);
        var columnsNames = DatabaseInfo.GetAllColumnsNames(database, referencedTableName);

        var expression = string.Join(
            " + ' - ' + ",
            columnsNames.Select(col => $"IIF({col} IS NULL, '', {col})")
        );

        referencedTable.Columns.Add("ColumnForDisplay", typeof(string), expression);

        return referencedTable;
    }
}
