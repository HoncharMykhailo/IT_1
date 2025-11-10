using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using skbd.Models;

using System.IO;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;


namespace skbd
{
    public partial class Form1 : Form
    {
        private int currentDbId = -1;
        private int currentTableId = -1;
        private List<Field> currentFields = new List<Field>();
        private int state = 0;
        public Form1()
        {
            InitializeComponent();
            LoadDatabaseButtons();
        }

        private void LoadDatabaseButtons()
        {
            switch (state)
            {
                case 0:
                    //  buttonBack.Visible = false;
                    //  buttonDelete.Visible = false;
                    flowLayoutPanelDbs.Controls.Clear();
                    using (var context = new SubdContext())
                    {
                        var databases = context.Dbases.ToList();
                        foreach (var db in databases)
                        {
                            Button dbButton = new Button
                            {
                                Text = db.Name,
                                Width = 180,
                                Height = 40
                            };

                            dbButton.Click += (s, e) =>
                            {
                                currentDbId = db.Id;
                                //     MessageBox.Show($" state {state} !");
                                ShowDatabasePanel(currentDbId);
                            };

                            flowLayoutPanelDbs.Controls.Add(dbButton);
                        }
                    }
                    break;
                case 1:
                buttonBack.Visible = true;
                    flowLayoutPanelDbs.Controls.Clear();
                    buttonBack.Visible = true;
                    buttonDelete.Visible = true;
                    buttonSave.Visible = true;
                    buttonLoad.Visible = true;
                    using (var context = new SubdContext())
                    {
                        var tables = context.Tables
                                            .Where(t => t.DbId == currentDbId)
                                            .ToList();

                        foreach (var table in tables)
                        {
                            Button tableButton = new Button
                            {
                                Text = table.Name,
                                Width = 180,
                                Height = 40
                            };

                            tableButton.Click += (s, e) =>
                            {
                                currentTableId = table.Id;
                                flowLayoutPanelDbs.Visible = false;
                                state = 2;
                                showTable(table.Id);
                            };

                            flowLayoutPanelDbs.Controls.Add(tableButton);
                        }
                        break;
                    }
                default:
                    MessageBox.Show($"error current state {state} !");
                    break;
            }
        }
        private void buttonAddDb_Click(object sender, EventArgs e)
        {
            switch (state)
            {
                case 2:

                    if (currentTableId == -1)
                    {
                        MessageBox.Show("Будь ласка, виберіть таблицю для додавання поля.");
                        return;
                    }

                    Form addFieldForm = new Form();
                    addFieldForm.Text = "Add New Field";
                    addFieldForm.Size = new Size(300, 200);
                    addFieldForm.StartPosition = FormStartPosition.CenterParent;

                    TextBox textBoxFieldName = new TextBox { Location = new Point(20, 20), Width = 240 };
                    addFieldForm.Controls.Add(textBoxFieldName);

                    ComboBox comboBoxType = new ComboBox { Location = new Point(20, 60), Width = 240, DropDownStyle = ComboBoxStyle.DropDownList };
                    using (var context = new SubdContext())
                    {
                        var types = context.Types.ToList();
                        comboBoxType.DataSource = types;
                        comboBoxType.DisplayMember = "Name";
                        comboBoxType.ValueMember = "Id";
                    }
                    addFieldForm.Controls.Add(comboBoxType);

                    Button buttonSaveField = new Button { Text = "Save", Location = new Point(20, 100), Width = 100 };
                    buttonSaveField.Click += (s, args) =>
                    {
                        string fieldName = textBoxFieldName.Text.Trim();
                        if (string.IsNullOrEmpty(fieldName))
                        {
                            MessageBox.Show("Введіть назву поля!");
                            return;
                        }
                        if (comboBoxType.SelectedValue == null)
                        {
                            MessageBox.Show("Будь ласка, виберіть тип поля.");
                            return;
                        }
                        int typeId = (int)comboBoxType.SelectedValue;

                        //    MessageBox.Show($"Creating {fieldName} as {typeId}");

                        using (var context = new SubdContext())
                        {
                            Field newField = new Field
                            {
                                Name = fieldName,
                                TableId = currentTableId,
                                TypeId = typeId
                            };
                            context.Fields.Add(newField);
                            context.SaveChanges();
                        }

                        //   MessageBox.Show($"Поле '{fieldName}' додано!");
                        addFieldForm.Close();

                        //   ShowTable(currentTableId);
                    };
                    addFieldForm.Controls.Add(buttonSaveField);

                    addFieldForm.ShowDialog();
                    showTable(currentTableId);
                    break;
                default:
                    textBoxNewDb.Visible = true;
                    buttonCreateDb.Visible = true;
                    break;

            }

        }
        private void buttonCreateDb_Click(object sender, EventArgs e)
        {
            switch (state)
            {
                case 0:
                    {
                        string newDbName = textBoxNewDb.Text.Trim();
                        if (string.IsNullOrEmpty(newDbName))
                        {
                            MessageBox.Show("Enter a database name!");
                            return;
                        }

                        using (var context = new SubdContext())
                        {
                            var newDb = new Dbase { Name = newDbName };
                            context.Dbases.Add(newDb);
                            context.SaveChanges();
                        }

                        LoadDatabaseButtons();
                        textBoxNewDb.Text = "";
                        textBoxNewDb.Visible = false;
                        buttonCreateDb.Visible = false;

                        MessageBox.Show($"Database '{newDbName}' created!");
                        break;
                    }
                case 1:
                    {
                        string newTableName = textBoxNewDb.Text.Trim();
                        if (string.IsNullOrEmpty(newTableName))
                        {
                            MessageBox.Show("Enter a table name!");
                            return;
                        }

                        using (var context = new SubdContext())
                        {
                            var newTable = new Table { Name = newTableName, DbId = currentDbId };
                            context.Tables.Add(newTable);
                            context.SaveChanges();
                        }

                        LoadDatabaseButtons();
                        textBoxNewDb.Text = "";
                        textBoxNewDb.Visible = false;
                        buttonCreateDb.Visible = false;

                        MessageBox.Show($"Table '{newTableName}' created!");
                        break;
                    }
                case 2:
                    {
                        AddNewRow();
                        break;
                    }
                default:
                    MessageBox.Show($"error current state {state} !");
                    break;
            }
        }
        private void ShowDatabasePanel(int dbId)
        {

            currentDbId = dbId;
            state = 1;
            buttonAddDb.Text = "Add Table";
            flowLayoutPanelDbs.Controls.Clear();

            LoadDatabaseButtons();
        }
        private void buttonBack_Click(object sender, EventArgs e)
        {
            state--;
            //   MessageBox.Show($" state {state} !");
            switch (state)
            {
                case 0:
                    buttonBack.Visible = false;
                    buttonCreateDb.Text = "Create Database";
                    buttonAddDb.Text = "Add Database";
                    buttonCreateDb.Visible = false;
                    flowLayoutPanelDbs.Visible = true;
                    buttonSave.Visible = false;
                    buttonLoad.Visible = false;
                    LoadDatabaseButtons();
                    break;
                case 1:
                    buttonDiff.Visible = false;
                    dataGridViewTable.Visible = false;
                    buttonCreateDb.Text = "Create Table";
                    buttonCreateDb.Visible = false;
                    flowLayoutPanelDbs.Visible = true;
                    buttonDelete.Visible = true;
                    LoadDatabaseButtons();
                    break;

                default:

                    MessageBox.Show($"error current state {state} !");
                    break;
            }
        }
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            switch (state)
            {
                case 1: 
                    {
                        if (currentDbId == -1)
                        {
                            MessageBox.Show("Будь ласка, виберіть базу для видалення.");
                            return;
                        }

                        var confirmDb = MessageBox.Show(
                            "Видалити цю базу разом з усіма таблицями, полями, рядками та значеннями?",
                            "Підтвердження",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning
                        );

                        if (confirmDb == DialogResult.No) return;

                        using (var context = new SubdContext())
                        {
                            var db = context.Dbases
                                .Include(d => d.Tables)
                                    .ThenInclude(t => t.Fields)
                                        .ThenInclude(f => f.Values)
                                .Include(d => d.Tables)
                                    .ThenInclude(t => t.Rows)
                                        .ThenInclude(r => r.Values)
                                .FirstOrDefault(d => d.Id == currentDbId);

                            if (db == null)
                            {
                                MessageBox.Show("Базу даних не знайдено!");
                                return;
                            }

                            foreach (var table in db.Tables)
                            {
                                foreach (var field in table.Fields)
                                {
                                    context.Values.RemoveRange(field.Values);
                                }

                                foreach (var row in table.Rows)
                                {
                                    context.Values.RemoveRange(row.Values);
                                }

                                context.Rows.RemoveRange(table.Rows);

                                context.Fields.RemoveRange(table.Fields);

                                context.Tables.Remove(table);
                            }

                            context.Dbases.Remove(db);

                            context.SaveChanges();
                        }

                        currentDbId = -1;
                        state = 0;
                        LoadDatabaseButtons();
                        MessageBox.Show("Базу даних та всі її таблиці успішно видалено!");
                        break;
                    }


                case 2: 
                    {
                        if (currentTableId == -1)
                        {
                            MessageBox.Show("Будь ласка, виберіть таблицю для видалення.");
                            return;
                        }

                        var confirmTable = MessageBox.Show(
                            "Видалити цю таблицю разом з усіма полями та значеннями?",
                            "Підтвердження",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning
                        );

                        if (confirmTable == DialogResult.No) return;

                        using (var context = new SubdContext())
                        {
                            var table = context.Tables
                                .Include(t => t.Fields)
                                    .ThenInclude(f => f.Values)
                                .Include(t => t.Rows)
                                    .ThenInclude(r => r.Values)
                                .FirstOrDefault(t => t.Id == currentTableId);

                            if (table == null)
                            {
                                MessageBox.Show("Таблицю не знайдено!");
                                return;
                            }

                            foreach (var field in table.Fields)
                            {
                                context.Values.RemoveRange(field.Values);
                            }

                            foreach (var row in table.Rows)
                            {
                                context.Values.RemoveRange(row.Values);
                            }

                            context.Rows.RemoveRange(table.Rows);

                            context.Fields.RemoveRange(table.Fields);

                            context.Tables.Remove(table);

                            context.SaveChanges();
                        }

                        currentTableId = -1;
                        state = 1; 
                        LoadDatabaseButtons();
                        MessageBox.Show("Таблицю та всі її поля успішно видалено!");
                        dataGridViewTable.Visible = false;
                        break;
                    }



                default:
                    MessageBox.Show($"Невідомий стан: {state}!");
                    break;
            }
        }

        private void showTable(int table_id)
        {
            buttonDiff.Visible = true;
            buttonAddDb.Text = "Add Field";
            buttonCreateDb.Visible = true;
            buttonCreateDb.Text = "Add Row";
            //  buttonDelete.Visible = false;
            dataGridViewTable.Visible = true;
            if (currentTableId == -1)
            {
                MessageBox.Show("Будь ласка, оберіть таблицю!");
                return;
            }

            using (var context = new SubdContext())
            {
                var fields = context.Fields
                                    .Where(f => f.TableId == currentTableId)
                                    .ToList();

                var values = context.Values
                                    .Where(v => v.Field.TableId == currentTableId)
                                    .ToList();

                dataGridViewTable.Columns.Clear();
                dataGridViewTable.Rows.Clear();

                DataGridViewButtonColumn deleteColumn = new DataGridViewButtonColumn();
                deleteColumn.HeaderText = "";
                deleteColumn.Text = "X";
                deleteColumn.UseColumnTextForButtonValue = true;
                dataGridViewTable.Columns.Add(deleteColumn);

                foreach (var field in fields)
                {
                    var type = context.Types.First(t => t.Id == field.TypeId);

                    DataGridViewTextBoxColumn fieldColumn = new DataGridViewTextBoxColumn();
                    fieldColumn.HeaderText = $"{field.Name} ({type.Name})   [X]"; 
                    fieldColumn.Tag = field.Id; 
                    dataGridViewTable.Columns.Add(fieldColumn);
                }

                var rowIds = values.Select(v => v.RowId).Distinct().ToList();

                foreach (var rowId in rowIds)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(dataGridViewTable);

                    row.Cells[0].Value = "X";

                    foreach (var value in values.Where(v => v.RowId == rowId))
                    {
                        
                        for (int i = 1; i < dataGridViewTable.Columns.Count; i++)
                        {
                            if ((int)dataGridViewTable.Columns[i].Tag == value.FieldId)
                            {
                                row.Cells[i].Value = value.Val; 
                                break;
                            }
                        }
                    }

                    // у showTable
                    row.Tag = rowId; // <-- зберігаємо Row.Id тут
                    dataGridViewTable.Rows.Add(row);

                }
            }
        }

        public void AddNewRow()
        {
            using (var context = new SubdContext())
            {
                // 1. Створюємо рядок
                var newRow = new Row
                {
                    TableId = currentTableId
                };
                context.Rows.Add(newRow);
                context.SaveChanges(); // тут новий row отримує Id

                // 2. Для кожного поля створюємо Value
                foreach (var field in context.Fields.Where(f => f.TableId == currentTableId))
                {
                    var newValue = new Value
                    {
                        RowId = newRow.Id, // існуючий Row
                        FieldId = field.Id,
                        Val = "" // пустий рядок замість null
                    };
                    context.Values.Add(newValue);
                }

                context.SaveChanges();
            }

            // Оновлюємо UI
            showTable(currentTableId);
        }

        public bool ValidateValue(string val, int typeId)
        {
            switch (typeId)
            {
                case 1:
                    return int.TryParse(val, out _);
                case 2:
                    return double.TryParse(val, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out _);
                case 3:
                    return val.Length == 1;
                case 4:
                    return true;
                case 5:
                    return System.Text.RegularExpressions.Regex.IsMatch(val, @"^[+-]?\d+([+-]\d+)i$");
                case 6:
                    return System.Text.RegularExpressions.Regex.IsMatch(val, @"^[+-]?(\d+(\.\d+)?)([+-](\d+(\.\d+)?))i$");
                default:
                    return false;
            }
        }



        private void dataGridViewTable_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 0) 
            {
                e.Cancel = true;
            }
        }


        private void dataGridViewTable_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex <= 0) return;

            int fieldId = (int)dataGridViewTable.Columns[e.ColumnIndex].Tag;
          //  MessageBox.Show($"fieldId = {fieldId} ");


            

            var cell = dataGridViewTable.Rows[e.RowIndex].Cells[e.ColumnIndex];
            string newValue = cell.Value?.ToString().Trim();
         //   MessageBox.Show($"Val =  {newValue}  ");
            

            // Дістаємо rowId з рядка (наприклад, у Tag або у першій колонці)
            int rowId = (int)dataGridViewTable.Rows[e.RowIndex].Tag;

          //  MessageBox.Show($"rowId = {rowId}");






            // Перевірка валідності
            int typeId;
            using (var context = new SubdContext())
            {
                var field = context.Fields.FirstOrDefault(f => f.Id == fieldId);
                if (field == null) return;
                typeId = field.TypeId;
            }

            if (!ValidateValue(newValue, typeId))
            {
                MessageBox.Show($"Невірне значення для цього типу! Очікуваний тип: {typeId}");
                cell.Tag = cell.Value?.ToString();

                return;
            }

            // Оновлюємо в базі
            using (var context = new SubdContext())
            {
                var valEntity = context.Values
                    .FirstOrDefault(v => v.RowId == rowId && v.FieldId == fieldId);

                if (valEntity != null)
                {
                    valEntity.Val = newValue;
                }
                else
                {
                    valEntity = new Value
                    {
                        RowId = rowId,
                        FieldId = fieldId,
                        Val = newValue
                    };
                    context.Values.Add(valEntity);
                }
                context.SaveChanges();
            }

            // Зберігаємо поточне значення у Tag клітинки (щоб відкотитися при невдалій валідації)
            cell.Tag = newValue;
        }









        private void dataGridViewTable_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex > 0)
            {
                string header = dataGridViewTable.Columns[e.ColumnIndex].HeaderText;
                if (header.EndsWith("[X]"))
                {
                    var confirm = MessageBox.Show(
                        "Ви впевнені, що хочете видалити це поле (разом з усіма його значеннями)?",
                        "Підтвердження видалення",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );

                    if (confirm != DialogResult.Yes)
                        return;

                    int fieldId = (int)dataGridViewTable.Columns[e.ColumnIndex].Tag;

                    using (var context = new SubdContext())
                    {
                        var field = context.Fields.FirstOrDefault(f => f.Id == fieldId);
                        if (field != null)
                        {
                            var valuesToDelete = context.Values.Where(v => v.FieldId == fieldId);
                            context.Values.RemoveRange(valuesToDelete);
                            context.Fields.Remove(field);
                            context.SaveChanges();
                        }
                    }

                    dataGridViewTable.Columns.RemoveAt(e.ColumnIndex);
                }
            }
        }

        private void dataGridViewTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 0) 
            {
                var confirm = MessageBox.Show(
                    "Ви впевнені, що хочете видалити цей рядок?",
                    "Підтвердження видалення",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (confirm != DialogResult.Yes)
                    return;

                int rowIndex = e.RowIndex;
                var row = dataGridViewTable.Rows[rowIndex];
                int? rowId = null;

                using (var context = new SubdContext())
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.ColumnIndex > 0 && cell.Value != null)
                        {
                            int fieldId = (int)dataGridViewTable.Columns[cell.ColumnIndex].Tag;
                            var value = context.Values.FirstOrDefault(v => v.FieldId == fieldId && v.Val == cell.Value.ToString());
                            if (value != null)
                            {
                                rowId = value.RowId;
                                break;
                            }
                        }
                    }
                    if (rowId.HasValue)
                    {
                        var valuesToDelete = context.Values.Where(v => v.RowId == rowId.Value);
                        context.Values.RemoveRange(valuesToDelete);
                        context.SaveChanges();
                    }
                }
                dataGridViewTable.Rows.RemoveAt(rowIndex);
            }
        }


        private void buttonSave_Click(object sender, EventArgs e)
        {
            using (var context = new SubdContext())
            {
                var databases = context.Dbases
                    .Include(d => d.Tables)
                        .ThenInclude(t => t.Fields)
                            .ThenInclude(f => f.Values)
                    .ToList();

                var dbCopy = databases.Select(d => new
                {
                    d.Name,
                    Tables = d.Tables.Select(t => new
                    {
                        t.Name,
                        Fields = t.Fields.Select(f => new
                        {
                            f.Name,
                            f.TypeId,
                            Values = f.Values.Select(v => new
                            {
                                v.RowId,
                                v.Val
                            }).ToList()
                        }).ToList()
                    }).ToList()
                }).ToList();

                string json = JsonConvert.SerializeObject(dbCopy, Formatting.Indented);
                File.WriteAllText("backup.json", json);
                MessageBox.Show("Бази даних збережено у backup.json");
            }
        }
        private void buttonLoad_Click(object sender, EventArgs e)
        {
            if (!File.Exists("backup.json"))
            {
                MessageBox.Show("Файл backup.json не знайдено!");
                return;
            }

            string json = File.ReadAllText("backup.json");

            var databasesData = JsonConvert.DeserializeObject<List<dynamic>>(json);
            if (databasesData == null || databasesData.Count == 0)
            {
                MessageBox.Show("Файл порожній або пошкоджений!");
                return;
            }

            using (var context = new SubdContext())
            {
                context.Values.RemoveRange(context.Values);
                context.Fields.RemoveRange(context.Fields);
                context.Tables.RemoveRange(context.Tables);
                context.Dbases.RemoveRange(context.Dbases);
                context.Rows.RemoveRange(context.Rows); 
                context.SaveChanges();

                foreach (var dbData in databasesData)
                {
                    var newDb = new Dbase { Name = dbData.Name.ToString() };
                    context.Dbases.Add(newDb);
                    context.SaveChanges();

                    foreach (var tableData in dbData.Tables)
                    {
                        var newTable = new Table { Name = tableData.Name.ToString(), DbId = newDb.Id };
                        context.Tables.Add(newTable);
                        context.SaveChanges();

                        var rowIds = new HashSet<int>();
                        foreach (var fieldData in tableData.Fields)
                        {
                            foreach (var valData in fieldData.Values)
                            {
                                rowIds.Add((int)valData.RowId);
                            }
                        }

                        var rowMap = new Dictionary<int, Row>();
                        foreach (var oldRowId in rowIds)
                        {
                            var row = new Row { TableId = newTable.Id };
                            context.Rows.Add(row);
                            context.SaveChanges();
                            rowMap[oldRowId] = row; 
                        }

                        foreach (var fieldData in tableData.Fields)
                        {
                            var newField = new Field
                            {
                                Name = fieldData.Name.ToString(),
                                TypeId = (int)fieldData.TypeId,
                                TableId = newTable.Id
                            };
                            context.Fields.Add(newField);
                            context.SaveChanges();

                            foreach (var valData in fieldData.Values)
                            {
                                var newValue = new Value
                                {
                                    RowId = rowMap[(int)valData.RowId].Id,
                                    FieldId = newField.Id,
                                    Val = valData.Val.ToString()
                                };
                                context.Values.Add(newValue);
                            }
                            context.SaveChanges();
                        }
                    }
                }
            }
            MessageBox.Show("Бази даних завантажено з backup.json");
            LoadDatabaseButtons(); 
        }

        private void buttonDiff_Click(object sender, EventArgs e)
        {
            if (currentTableId == -1)
            {
                MessageBox.Show("Будь ласка, оберіть таблицю для порівняння!");
                return;
            }

            Form diffForm = new Form();
            diffForm.Text = "Table Difference";
            diffForm.Size = new Size(1000, 600);
            diffForm.StartPosition = FormStartPosition.CenterParent;

            DataGridView dgvCurrent = new DataGridView
            {
                Location = new Point(20, 50),
                Size = new Size(450, 400),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true
            };
            diffForm.Controls.Add(dgvCurrent);

            ComboBox cbFields = new ComboBox
            {
                Location = new Point(20, 20),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            diffForm.Controls.Add(cbFields);

            ComboBox cbTables = new ComboBox
            {
                Location = new Point(240, 20),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            diffForm.Controls.Add(cbTables);

            ComboBox cbOtherFields = new ComboBox
            {
                Location = new Point(500, 20),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Enabled = false 
            };
            diffForm.Controls.Add(cbOtherFields);

            using (var context = new SubdContext())
            {
                var currentFields = context.Fields.Where(f => f.TableId == currentTableId).ToList();
                foreach (var field in currentFields)
                    cbFields.Items.Add(field.Name);
                if (cbFields.Items.Count > 0) cbFields.SelectedIndex = 0;

                var allTables = context.Tables
                                       .Where(t => t.DbId == currentDbId)
                                       .ToList();

                foreach (var table in allTables)
                {
                    if (table.Id == currentTableId) continue;
                    cbTables.Items.Add(new ComboBoxItem { Text = table.Name, Value = table.Id });
                }
                if (cbTables.Items.Count > 0) cbTables.SelectedIndex = 0;

                ShowTableInGrid(currentTableId, dgvCurrent);
            }

            cbTables.SelectedIndexChanged += (s, ev) =>
            {
                var selected = cbTables.SelectedItem as ComboBoxItem;
                if (selected != null)
                {
                    int tableId = selected.Value;

                    cbOtherFields.Enabled = true;

                    cbOtherFields.Items.Clear();
                    using (var context = new SubdContext())
                    {
                        var otherFields = context.Fields.Where(f => f.TableId == tableId).ToList();
                        foreach (var f in otherFields)
                            cbOtherFields.Items.Add(f.Name);
                        if (cbOtherFields.Items.Count > 0)
                            cbOtherFields.SelectedIndex = 0;

                        DataGridView dgvOther = diffForm.Controls.OfType<DataGridView>()
                            .FirstOrDefault(d => d.Tag != null && (string)d.Tag == "OtherTable");

                        if (dgvOther == null)
                        {
                            dgvOther = new DataGridView
                            {
                                Location = new Point(500, 50),
                                Size = new Size(450, 400),
                                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                                ReadOnly = true,
                                Tag = "OtherTable"
                            };
                            diffForm.Controls.Add(dgvOther);
                        }

                        ShowTableInGrid(tableId, dgvOther);
                    }
                }
            };

            Button btnCompare = new Button
            {
                Text = "Compare",
                Location = new Point(20, 470),
                Width = 100
            };
            diffForm.Controls.Add(btnCompare);

            btnCompare.Click += (s, ev) =>
            {
                if (cbFields.SelectedItem == null || cbOtherFields.SelectedItem == null)
                {
                    MessageBox.Show("Будь ласка, оберіть поля для порівняння.");
                    return;
                }

                string field1Name = cbFields.SelectedItem.ToString();
                string field2Name = cbOtherFields.SelectedItem.ToString();

                using (var context = new SubdContext())
                {
                    var field1 = context.Fields.FirstOrDefault(f => f.TableId == currentTableId && f.Name == field1Name);
                    var field2 = context.Fields.FirstOrDefault(f => f.TableId == ((ComboBoxItem)cbTables.SelectedItem).Value && f.Name == field2Name);

                    if (field1 == null || field2 == null)
                    {
                        MessageBox.Show("Не вдалося знайти обране поле у базі.");
                        return;
                    }

                    if (field1.TypeId != field2.TypeId)
                    {
                        MessageBox.Show("Типи обраних полів не збігаються!");
                        return;
                    }

                    var values1 = context.Values.Where(v => v.Field.TableId == currentTableId).ToList();
                    var values2 = context.Values.Where(v => v.Field.TableId == field2.TableId).ToList();

                    var rowIds1 = values1.Select(v => v.RowId).Distinct().ToList();
                    var rowIds2 = values2.Select(v => v.RowId).Distinct().ToList();


                    var dgvDiffOld = diffForm.Controls.OfType<DataGridView>().FirstOrDefault(d => d.Tag != null && (string)d.Tag == "DiffResult");
                    if (dgvDiffOld != null) diffForm.Controls.Remove(dgvDiffOld);

                    DataGridView dgvDiff = new DataGridView
                    {
                        Location = new Point(20, 510),
                        Size = new Size(930, 200),
                        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                        ReadOnly = true,
                        Tag = "DiffResult"
                    };
                    diffForm.Controls.Add(dgvDiff);

                    var firstTableColumns = context.Fields.Where(f => f.TableId == currentTableId).ToList();
                    dgvDiff.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "RowId" });
                    foreach (var f in firstTableColumns)
                    {
                        dgvDiff.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            HeaderText = $"{f.Name} ({context.Types.First(t => t.Id == f.TypeId).Name})",
                            Tag = f.Id
                        });
                    }

                    int maxRows = Math.Max(rowIds1.Count, rowIds2.Count);

                    foreach (var rowId1 in rowIds1)
                    {
                        bool hasMatch = false;

                        foreach (var rowId2 in rowIds2)
                        {
                            string val1 = values1.FirstOrDefault(v => v.RowId == rowId1 && v.FieldId == field1.Id)?.Val?.Trim();
                            string val2 = values2.FirstOrDefault(v => v.RowId == rowId2 && v.FieldId == field2.Id)?.Val?.Trim();

                            if (val1 == val2)
                            {
                                hasMatch = true;
                                break;
                            }
                        }
                        if (!hasMatch)
                        {
                            DataGridViewRow row = new DataGridViewRow();
                            row.CreateCells(dgvDiff);
                            row.Cells[0].Value = rowId1;

                            foreach (var col in firstTableColumns)
                            {
                                int colIndex = dgvDiff.Columns.Cast<DataGridViewColumn>()
                                    .FirstOrDefault(c => c.Tag != null && (int)c.Tag == col.Id)?.Index ?? -1;

                                if (colIndex != -1)
                                {
                                    row.Cells[colIndex].Value = values1.FirstOrDefault(v => v.RowId == rowId1 && v.FieldId == col.Id)?.Val;
                                }
                            }
                            dgvDiff.Rows.Add(row);
                        }
                    }
                }
            };


        diffForm.ShowDialog();
        }

        class ComboBoxItem
        {
            public string Text { get; set; }
            public int Value { get; set; }
            public override string ToString() => Text;
        }


        private void ShowTableInGrid(int tableId, DataGridView dgv)
        {

            
            dgv.Columns.Clear();
            dgv.Rows.Clear();

            using (var context = new SubdContext())
            {
                var fields = context.Fields.Where(f => f.TableId == tableId).ToList();
                var values = context.Values.Where(v => fields.Select(f => f.Id).Contains(v.FieldId)).ToList();
                var rowIds = values.Select(v => v.RowId).Distinct().ToList();

                foreach (var field in fields)
                {
                    dgv.Columns.Add(field.Name, field.Name);
                }

                foreach (var rowId in rowIds)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(dgv);

                    for (int i = 0; i < fields.Count; i++)
                    {
                        var value = values.FirstOrDefault(v => v.RowId == rowId && v.FieldId == fields[i].Id);
                        row.Cells[i].Value = value?.Val;
                    }

                    dgv.Rows.Add(row);
                }
            }
            
        }
    
    }
}
