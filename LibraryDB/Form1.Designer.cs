namespace LibraryDB
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridView = new DataGridView();
            selectTableBox = new ComboBox();
            label1 = new Label();
            refreshButton = new Button();
            saveButton = new Button();
            filterRowPanel = new Panel();
            selectViewBox = new ComboBox();
            label2 = new Label();
            viewOpenButton = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            SuspendLayout();
            // 
            // dataGridView
            // 
            dataGridView.AllowUserToOrderColumns = true;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Location = new Point(12, 43);
            dataGridView.Name = "dataGridView";
            dataGridView.Size = new Size(1009, 527);
            dataGridView.TabIndex = 0;
            // 
            // selectTableBox
            // 
            selectTableBox.BackColor = SystemColors.MenuBar;
            selectTableBox.DropDownStyle = ComboBoxStyle.DropDownList;
            selectTableBox.FormattingEnabled = true;
            selectTableBox.Location = new Point(1069, 45);
            selectTableBox.Name = "selectTableBox";
            selectTableBox.Size = new Size(121, 23);
            selectTableBox.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(1082, 27);
            label1.Name = "label1";
            label1.Size = new Size(95, 15);
            label1.TabIndex = 2;
            label1.Text = "Выбор таблицы";
            // 
            // refreshButton
            // 
            refreshButton.Location = new Point(1069, 107);
            refreshButton.Name = "refreshButton";
            refreshButton.Size = new Size(121, 57);
            refreshButton.TabIndex = 3;
            refreshButton.Text = "Обновить";
            refreshButton.UseVisualStyleBackColor = true;
            // 
            // saveButton
            // 
            saveButton.Location = new Point(1069, 203);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(121, 61);
            saveButton.TabIndex = 4;
            saveButton.Text = "Сохранить изменения";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += SaveButton_Click;
            // 
            // filterRowPanel
            // 
            filterRowPanel.Location = new Point(12, 12);
            filterRowPanel.Name = "filterRowPanel";
            filterRowPanel.Size = new Size(1009, 25);
            filterRowPanel.TabIndex = 5;
            // 
            // selectViewBox
            // 
            selectViewBox.DropDownStyle = ComboBoxStyle.DropDownList;
            selectViewBox.FormattingEnabled = true;
            selectViewBox.Location = new Point(1069, 416);
            selectViewBox.Name = "selectViewBox";
            selectViewBox.Size = new Size(121, 23);
            selectViewBox.TabIndex = 6;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(1082, 398);
            label2.Name = "label2";
            label2.Size = new Size(83, 15);
            label2.TabIndex = 7;
            label2.Text = "Выбор отчёта";
            // 
            // viewOpenButton
            // 
            viewOpenButton.Location = new Point(1069, 460);
            viewOpenButton.Name = "viewOpenButton";
            viewOpenButton.Size = new Size(121, 52);
            viewOpenButton.TabIndex = 8;
            viewOpenButton.Text = "Открыть отчёт";
            viewOpenButton.UseVisualStyleBackColor = true;
            viewOpenButton.Click += ViewOpenButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1231, 582);
            Controls.Add(viewOpenButton);
            Controls.Add(label2);
            Controls.Add(selectViewBox);
            Controls.Add(filterRowPanel);
            Controls.Add(saveButton);
            Controls.Add(refreshButton);
            Controls.Add(label1);
            Controls.Add(selectTableBox);
            Controls.Add(dataGridView);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView;
        private ComboBox selectTableBox;
        private Label label1;
        private Button refreshButton;
        private Button saveButton;
        private Panel filterRowPanel;
        private ComboBox selectViewBox;
        private Label label2;
        private Button viewOpenButton;
    }
}
