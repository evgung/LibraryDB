using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace LibraryDB;

public class Filtrator
{
    private readonly DataGridView dataGridView;
    private readonly Panel filterRowPanel;
    private readonly Dictionary<int, ComboBox> columnsFilters = new();

    private DataTable dataTable;
    private BindingSource bindingSource;

    public Filtrator(DataGridView dataGridView, Panel filterRowPanel)
    {
        this.dataGridView = dataGridView;
        this.filterRowPanel = filterRowPanel;
    }

    public void Reset(DataTable dataTable)
    {
        this.dataTable = dataTable;

        dataGridView.ColumnWidthChanged -= ResizeAllFilters;
        dataGridView.ColumnDisplayIndexChanged -= ResizeAllFilters;
        filterRowPanel.Controls.Clear();
        columnsFilters.Clear();

        bindingSource = new BindingSource
        {
            DataSource = dataTable
        };

        dataGridView.DataSource = bindingSource;
    }

    public void SetFilters()
    {
        AddFilterComboBoxes();

        dataGridView.ColumnWidthChanged += ResizeAllFilters;
        dataGridView.ColumnDisplayIndexChanged += ResizeAllFilters;
    }

    public void AddFilterComboBoxes()
    {
        foreach (DataGridViewColumn column in dataGridView.Columns)
        {
            var comboBox = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = column.Width,
                Tag = column.Name,    // Сохраняем имя столбца в Tag
                Location = new Point(
                    dataGridView.GetColumnDisplayRectangle(column.Index, true).X,
                    0
                )
            };

            comboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
            FillComboBoxWithUniqueValues(comboBox, column.Name);
            filterRowPanel.Controls.Add(comboBox);
            columnsFilters[column.Index] = comboBox;
        }
    }

    private void FillComboBoxWithUniqueValues(ComboBox comboBox, string columnName)
    {
        var uniqueValues = dataTable.AsEnumerable()
                            .Select(row => row[columnName])
                            .Where(value => value != DBNull.Value)
                            .Distinct()
                            .OrderBy(value => value)
                            .ToArray();

        comboBox.Items.Add("(Все)");
        comboBox.Items.AddRange(uniqueValues);

        comboBox.SelectedIndex = 0;
    }

    private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        var comboBox = sender as ComboBox;
        var columnName = comboBox.Tag.ToString();
        var selectedValue = comboBox.SelectedItem.ToString();

        if (selectedValue == "(Все)")
        {
            bindingSource.Filter = null; // Сбрасываем фильтр
        }
        else
        {
            bindingSource.Filter = $"{columnName} = '{selectedValue}'";
        }
    }

    private void ResizeAllFilters(object? sender, DataGridViewColumnEventArgs e)
    {
        foreach (DataGridViewColumn column in dataGridView.Columns)
        {
            var comboBox = columnsFilters[column.Index];
            comboBox.Location = new Point(
                dataGridView.GetColumnDisplayRectangle(column.Index, true).X,
                0
            );
            comboBox.Width = column.Width;
        }
    }
}
