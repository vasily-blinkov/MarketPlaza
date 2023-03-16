namespace Wholesale.Desktop.Forms
{
    partial class ParentForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParentForm));
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.administrationItem = new System.Windows.Forms.ToolStripMenuItem();
            this.leasesItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lesseesItem = new System.Windows.Forms.ToolStripMenuItem();
            this.goodsItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.administrationItem,
            this.leasesItem,
            this.goodsItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(800, 24);
            this.mainMenu.TabIndex = 1;
            this.mainMenu.Text = "Главное меню";
            // 
            // administrationItem
            // 
            this.administrationItem.Name = "administrationItem";
            this.administrationItem.Size = new System.Drawing.Size(134, 20);
            this.administrationItem.Text = "&Администрирование";
            this.administrationItem.Click += new System.EventHandler(this.OpenUsers);
            // 
            // leasesItem
            // 
            this.leasesItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lesseesItem});
            this.leasesItem.Name = "leasesItem";
            this.leasesItem.Size = new System.Drawing.Size(59, 20);
            this.leasesItem.Text = "А&ренда";
            // 
            // lesseesItem
            // 
            this.lesseesItem.Name = "lesseesItem";
            this.lesseesItem.Size = new System.Drawing.Size(142, 22);
            this.lesseesItem.Text = "Ар&ендаторы";
            this.lesseesItem.Click += new System.EventHandler(this.OpenLessees);
            // 
            // goodsItem
            // 
            this.goodsItem.Name = "goodsItem";
            this.goodsItem.Size = new System.Drawing.Size(60, 20);
            this.goodsItem.Text = "&Товары";
            this.goodsItem.Click += new System.EventHandler(this.OpenGoods);
            // 
            // ParentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.mainMenu);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.mainMenu;
            this.Name = "ParentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Market Plaza";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Exit);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem administrationItem;
        private System.Windows.Forms.ToolStripMenuItem leasesItem;
        private System.Windows.Forms.ToolStripMenuItem lesseesItem;
        private System.Windows.Forms.ToolStripMenuItem goodsItem;
    }
}