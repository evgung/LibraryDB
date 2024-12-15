using Npgsql;
using System.Data;

namespace LibraryDB;

public partial class Form1 : Form
{
    private readonly Database database;
    private DataTableAdapter dataTableAdapter;
    private readonly DataVisualizer dataVisualizer;
    public string CurrentTableName => (string)selectTableBox.SelectedItem;
    public string CurrentViewName => (string)selectViewBox.SelectedItem;

    public Form1()
    {
        InitializeComponent();

        database = new Database();
        database.OpenConnection();

        var tables = new Tables(database);
        selectTableBox.DataSource = tables.TablesNames;
        selectTableBox.SelectedIndex = -1;
        selectViewBox.DataSource = tables.ViewsNames;
        selectViewBox.SelectedIndex = -1;

        dataVisualizer = new DataVisualizer(database, dataGridView, filterRowPanel);

        selectTableBox.SelectedIndexChanged += ShowTable;
        refreshButton.Click += ShowTable;
        dataGridView.DefaultValuesNeeded += DataGridView_DefaultValuesNeeded;
    }

    private void ShowTable(object? sender, EventArgs e)
    {
        if (selectTableBox.SelectedIndex == -1)
            return;
        dataTableAdapter = new DataTableAdapter(database, CurrentTableName);
        dataVisualizer.SetNewDataSource(dataTableAdapter);
        dataVisualizer.Visualize();
    }

    private void SaveButton_Click(object? sender, EventArgs e)
    {
        if (selectTableBox.SelectedIndex == -1)
            return;
        dataTableAdapter.UpdateAllData();
    }

    private void DataGridView_DefaultValuesNeeded(object? sender, DataGridViewRowEventArgs e)
    {
        var defaultValues = DatabaseInfo.GetDefaultValues(database, CurrentTableName);

        foreach (var column in defaultValues)
        {
            var columnName = column.Key;
            var defaultValue = column.Value;

            e.Row.Cells[columnName].Value = defaultValue;
        }
    }

    private void ViewOpenButton_Click(object sender, EventArgs e)
    {
        if (selectViewBox.SelectedIndex == -1)
            return;
        var viewForm = new ViewForm(database, CurrentViewName);
        viewForm.Show();
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        database.CloseConnection();
        base.OnFormClosed(e);
    }
}
