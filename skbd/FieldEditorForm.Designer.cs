namespace skbd
{
    partial class FieldEditorForm
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox textBoxName;
        private ComboBox comboBoxType;
        private Button buttonSave;
        private Button buttonCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(20, 20);
            this.textBoxName.Size = new System.Drawing.Size(200, 20);
            // 
            // comboBoxType
            // 
            this.comboBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxType.Location = new System.Drawing.Point(20, 60);
            this.comboBoxType.Size = new System.Drawing.Size(200, 21);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(20, 100);
            this.buttonSave.Size = new System.Drawing.Size(80, 30);
            this.buttonSave.Text = "Save";
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(140, 100);
            this.buttonCancel.Size = new System.Drawing.Size(80, 30);
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // FieldEditorForm
            // 
            this.ClientSize = new System.Drawing.Size(250, 150);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.comboBoxType);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonCancel);
            this.Text = "Field Editor";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
