namespace skbd
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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


        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelDbs;
        private System.Windows.Forms.Button buttonAddDb;
        private System.Windows.Forms.TextBox textBoxNewDb;
        private System.Windows.Forms.Button buttonCreateDb;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelTable;


        private System.Windows.Forms.DataGridView dataGridViewTable;
        private System.Windows.Forms.Button buttonLoadTable;
        private System.Windows.Forms.Button buttonDiff;

        private System.Windows.Forms.TextBox textBoxNew;
        
        private void InitializeComponent()
        {
            this.flowLayoutPanelDbs = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonAddDb = new System.Windows.Forms.Button();
            this.textBoxNewDb = new System.Windows.Forms.TextBox();
            this.buttonCreateDb = new System.Windows.Forms.Button();
            this.buttonBack = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.buttonDiff = new System.Windows.Forms.Button();

            this.SuspendLayout();
            // 
            // flowLayoutPanelDbs
            // 
            this.flowLayoutPanelDbs.Location = new System.Drawing.Point(20, 20);
            this.flowLayoutPanelDbs.Size = new System.Drawing.Size(400, 400);
            this.flowLayoutPanelDbs.AutoScroll = true;
            // 
            // buttonAddDb
            // 
            this.buttonAddDb.Location = new System.Drawing.Point(450 + 220, 20);
            this.buttonAddDb.Size = new System.Drawing.Size(120, 40);
            this.buttonAddDb.Text = "Add Database";
            this.buttonAddDb.Anchor = AnchorStyles.Right | AnchorStyles.Top;

            this.buttonAddDb.Click += new System.EventHandler(this.buttonAddDb_Click);
            // 
            // textBoxNewDb
            // 
            this.textBoxNewDb.Location = new System.Drawing.Point(450 + 220, 70);
            this.textBoxNewDb.Size = new System.Drawing.Size(120, 20);
            this.textBoxNewDb.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.textBoxNewDb.Visible = false;
            // 
            // buttonCreateDb
            // 
            this.buttonCreateDb.Location = new System.Drawing.Point(450 + 220, 100);
            this.buttonCreateDb.Size = new System.Drawing.Size(120, 30);
            this.buttonCreateDb.Text = "Create";
            this.buttonCreateDb.Visible = false;
            this.buttonCreateDb.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.buttonCreateDb.Click += new System.EventHandler(this.buttonCreateDb_Click);
            // 
            // buttonDeleteDb
            // 
            this.buttonDelete.Location = new System.Drawing.Point(450 + 220, 200);
            this.buttonDelete.Size = new System.Drawing.Size(120, 30);
            this.buttonDelete.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.buttonDelete.Text = "Delete";
            // this.buttonDelete.Visible = false;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonBack
            // 
            this.buttonBack.Location = new System.Drawing.Point(450 + 220, 400);
            this.buttonBack.Size = new System.Drawing.Size(120, 30);
            this.buttonBack.Text = "Back";
            this.buttonBack.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            this.buttonBack.Visible = false;




            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(450 + 220, 350);
            this.buttonSave.Size = new System.Drawing.Size(120, 30);
            this.buttonSave.Text = "Save";
            this.buttonSave.Visible = true;
            this.buttonSave.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(450 + 220, 300);
            this.buttonLoad.Size = new System.Drawing.Size(120, 30);
            this.buttonLoad.Text = "Load";
            this.buttonLoad.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);




            this.buttonDiff.Location = new System.Drawing.Point(450 + 220, 250);
            this.buttonDiff.Size = new System.Drawing.Size(120, 30);
            this.buttonDiff.Text = "Diff";
            this.buttonDiff.Visible = false;
            this.buttonDiff.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonDiff.Click += new System.EventHandler(this.buttonDiff_Click);






            this.dataGridViewTable = new System.Windows.Forms.DataGridView();
            // 
            // dataGridViewTable
            // 
            this.dataGridViewTable.Location = new System.Drawing.Point(20, 20);
            this.dataGridViewTable.Size = new System.Drawing.Size(600, 400);
            this.dataGridViewTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewTable.AllowUserToAddRows = false;
            this.dataGridViewTable.RowHeadersVisible = true;
            this.dataGridViewTable.Visible = false;
            dataGridViewTable.CellContentClick += dataGridViewTable_CellContentClick;
            dataGridViewTable.ColumnHeaderMouseClick += dataGridViewTable_ColumnHeaderMouseClick;
            this.dataGridViewTable.CellBeginEdit += dataGridViewTable_CellBeginEdit;
            this.dataGridViewTable.CellEndEdit += dataGridViewTable_CellEndEdit;





            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.flowLayoutPanelDbs);
            this.Controls.Add(this.buttonAddDb);
            this.Controls.Add(this.textBoxNewDb);
            this.Controls.Add(this.buttonCreateDb);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.dataGridViewTable);
            this.Controls.Add(this.buttonDiff);



            this.Name = "Form1";
            this.Text = "Database Manager";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

       



    }
}

