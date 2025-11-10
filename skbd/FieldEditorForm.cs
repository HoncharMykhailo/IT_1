using System;
using System.Linq;
using System.Windows.Forms;
using skbd.Models;

namespace skbd
{
    public partial class FieldEditorForm : Form
    {
        public string FieldName { get; private set; }
        public int SelectedTypeId { get; private set; }

        public FieldEditorForm(string initialName = "", int? initialTypeId = null)
        {
            InitializeComponent();

            textBoxName.Text = initialName;

            using (var context = new SubdContext())
            {
                var types = context.Types.ToList();  
                comboBoxType.DataSource = types;
                comboBoxType.DisplayMember = "Name"; 
                comboBoxType.ValueMember = "Id";     
            }

            if (initialTypeId.HasValue)
                comboBoxType.SelectedValue = initialTypeId.Value;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            FieldName = textBoxName.Text.Trim();
            SelectedTypeId = (int)comboBoxType.SelectedValue;

            if (string.IsNullOrEmpty(FieldName))
            {
                MessageBox.Show("Please enter a field name.");
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
