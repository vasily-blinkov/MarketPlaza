namespace Wholesale.Desktop.Forms
{
    partial class GoodsForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GoodsForm));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.filterLabel = new System.Windows.Forms.ToolStripLabel();
            this.filterBox = new System.Windows.Forms.ToolStripTextBox();
            this.readButton = new System.Windows.Forms.ToolStripButton();
            this.masterDetailsSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.crudLabel = new System.Windows.Forms.ToolStripLabel();
            this.createButton = new System.Windows.Forms.ToolStripButton();
            this.upsertButton = new System.Windows.Forms.ToolStripButton();
            this.deleteButton = new System.Windows.Forms.ToolStripButton();
            this.transposeButton = new System.Windows.Forms.ToolStripButton();
            this.viewLabel = new System.Windows.Forms.ToolStripLabel();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.masterGrid = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.titleBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.priceBox = new System.Windows.Forms.TextBox();
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.masterGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.filterLabel,
            this.filterBox,
            this.readButton,
            this.masterDetailsSeparator,
            this.crudLabel,
            this.createButton,
            this.upsertButton,
            this.deleteButton,
            this.transposeButton,
            this.viewLabel});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(684, 25);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip1";
            // 
            // filterLabel
            // 
            this.filterLabel.Name = "filterLabel";
            this.filterLabel.Size = new System.Drawing.Size(45, 22);
            this.filterLabel.Text = "Поиск:";
            // 
            // filterBox
            // 
            this.filterBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.filterBox.Name = "filterBox";
            this.filterBox.Size = new System.Drawing.Size(100, 25);
            // 
            // readButton
            // 
            this.readButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.readButton.Image = ((System.Drawing.Image)(resources.GetObject("readButton.Image")));
            this.readButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.readButton.Name = "readButton";
            this.readButton.Size = new System.Drawing.Size(65, 22);
            this.readButton.Text = "О&бновить";
            this.readButton.Click += new System.EventHandler(this.Refresh);
            // 
            // masterDetailsSeparator
            // 
            this.masterDetailsSeparator.Name = "masterDetailsSeparator";
            this.masterDetailsSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // crudLabel
            // 
            this.crudLabel.Name = "crudLabel";
            this.crudLabel.Size = new System.Drawing.Size(53, 22);
            this.crudLabel.Text = "Данные:";
            // 
            // createButton
            // 
            this.createButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.createButton.Image = ((System.Drawing.Image)(resources.GetObject("createButton.Image")));
            this.createButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(54, 22);
            this.createButton.Text = "&Создать";
            this.createButton.Click += new System.EventHandler(this.CreateEntity);
            // 
            // upsertButton
            // 
            this.upsertButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.upsertButton.Image = ((System.Drawing.Image)(resources.GetObject("upsertButton.Image")));
            this.upsertButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.upsertButton.Name = "upsertButton";
            this.upsertButton.Size = new System.Drawing.Size(70, 22);
            this.upsertButton.Text = "С&охранить";
            this.upsertButton.Click += new System.EventHandler(this.SaveUser);
            // 
            // deleteButton
            // 
            this.deleteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.deleteButton.Image = ((System.Drawing.Image)(resources.GetObject("deleteButton.Image")));
            this.deleteButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(55, 22);
            this.deleteButton.Text = "&Удалить";
            this.deleteButton.Click += new System.EventHandler(this.DeleteEntity);
            // 
            // transposeButton
            // 
            this.transposeButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.transposeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.transposeButton.Image = ((System.Drawing.Image)(resources.GetObject("transposeButton.Image")));
            this.transposeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.transposeButton.Name = "transposeButton";
            this.transposeButton.Size = new System.Drawing.Size(108, 22);
            this.transposeButton.Text = "&Транспонировать";
            this.transposeButton.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            this.transposeButton.Click += new System.EventHandler(this.Transpose);
            // 
            // viewLabel
            // 
            this.viewLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.viewLabel.Name = "viewLabel";
            this.viewLabel.Size = new System.Drawing.Size(30, 22);
            this.viewLabel.Text = "Вид:";
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 25);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.masterGrid);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.priceBox);
            this.splitContainer.Panel2.Controls.Add(this.label2);
            this.splitContainer.Panel2.Controls.Add(this.titleBox);
            this.splitContainer.Panel2.Controls.Add(this.label1);
            this.splitContainer.Size = new System.Drawing.Size(684, 234);
            this.splitContainer.SplitterDistance = 176;
            this.splitContainer.TabIndex = 1;
            // 
            // masterGrid
            // 
            this.masterGrid.AllowUserToAddRows = false;
            this.masterGrid.AllowUserToDeleteRows = false;
            this.masterGrid.AllowUserToResizeRows = false;
            this.masterGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.masterGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.masterGrid.Location = new System.Drawing.Point(0, 0);
            this.masterGrid.MultiSelect = false;
            this.masterGrid.Name = "masterGrid";
            this.masterGrid.ReadOnly = true;
            this.masterGrid.RowHeadersVisible = false;
            this.masterGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.masterGrid.Size = new System.Drawing.Size(176, 234);
            this.masterGrid.TabIndex = 0;
            this.masterGrid.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.LoadEntity);
            this.masterGrid.SelectionChanged += new System.EventHandler(this.SelectEntity);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Цена";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // titleBox
            // 
            this.titleBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.titleBox.Location = new System.Drawing.Point(67, 4);
            this.titleBox.Name = "titleBox";
            this.titleBox.Size = new System.Drawing.Size(425, 20);
            this.titleBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Название";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // priceBox
            // 
            this.priceBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.priceBox.Location = new System.Drawing.Point(67, 30);
            this.priceBox.Name = "priceBox";
            this.priceBox.Size = new System.Drawing.Size(425, 20);
            this.priceBox.TabIndex = 5;
            // 
            // GoodsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 259);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.toolStrip);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(700, 298);
            this.Name = "GoodsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Товары";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HandleClosing);
            this.Load += new System.EventHandler(this.Prepare);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.masterGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton transposeButton;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.DataGridView masterGrid;
        private System.Windows.Forms.ToolStripLabel crudLabel;
        private System.Windows.Forms.ToolStripLabel viewLabel;
        private System.Windows.Forms.ToolStripButton createButton;
        private System.Windows.Forms.ToolStripButton readButton;
        private System.Windows.Forms.ToolStripButton upsertButton;
        private System.Windows.Forms.ToolStripButton deleteButton;
        private System.Windows.Forms.ToolStripLabel filterLabel;
        private System.Windows.Forms.ToolStripTextBox filterBox;
        private System.Windows.Forms.ToolStripSeparator masterDetailsSeparator;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox titleBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox priceBox;
    }
}