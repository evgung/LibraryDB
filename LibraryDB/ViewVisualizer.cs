using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryDB;

public class ViewVisualizer
{
    private readonly Database database;
    private readonly DataGridView dataGridView;
    private readonly Filtrator filtrator;
    private readonly string viewName;

    public ViewVisualizer(Database database, DataGridView dataGridView, Panel filterRowPanel, string viewName)
    {
        this.database = database;
        this.dataGridView = dataGridView;
        filtrator = new Filtrator(dataGridView, filterRowPanel);
        this.viewName = viewName;
    }

    public void Visualize()
    {
        var query = $@"
            SELECT * FROM {viewName}
        ";

        var dataTable = DatabaseQueryExecutor.GetDataTable(database, query);

        filtrator.Reset(dataTable);
        filtrator.SetFilters();
    }
}
