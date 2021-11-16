
namespace AudioDataInterface
{
    partial class MainWindow
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.оПрограммеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.button_capture = new System.Windows.Forms.ToolStripButton();
            this.button_encoder = new System.Windows.Forms.ToolStripButton();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.column_block = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column_data = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column_type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column_hcIn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column_hcOut = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column_hcReturn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listView = new System.Windows.Forms.ListView();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menu_edit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.menu_remove = new System.Windows.Forms.ToolStripMenuItem();
            this.column_markerCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.справкаToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(711, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip";
            // 
            // справкаToolStripMenuItem
            // 
            this.справкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.оПрограммеToolStripMenuItem});
            this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            this.справкаToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.справкаToolStripMenuItem.Text = "Справка";
            // 
            // оПрограммеToolStripMenuItem
            // 
            this.оПрограммеToolStripMenuItem.Name = "оПрограммеToolStripMenuItem";
            this.оПрограммеToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.оПрограммеToolStripMenuItem.Text = "О программе";
            // 
            // toolStrip
            // 
            this.toolStrip.BackColor = System.Drawing.Color.LightGray;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.button_capture,
            this.button_encoder});
            this.toolStrip.Location = new System.Drawing.Point(0, 24);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(711, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "toolStrip";
            // 
            // button_capture
            // 
            this.button_capture.Image = global::AudioDataInterface.Properties.Resources.icon_capture;
            this.button_capture.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_capture.Name = "button_capture";
            this.button_capture.Size = new System.Drawing.Size(66, 22);
            this.button_capture.Text = "Запись";
            this.button_capture.Click += new System.EventHandler(this.button_capture_Click);
            // 
            // button_encoder
            // 
            this.button_encoder.Image = global::AudioDataInterface.Properties.Resources.icon_encode;
            this.button_encoder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_encoder.Name = "button_encoder";
            this.button_encoder.Size = new System.Drawing.Size(73, 22);
            this.button_encoder.Text = "Энкодер";
            this.button_encoder.Click += new System.EventHandler(this.button_encoder_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Location = new System.Drawing.Point(0, 470);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(711, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip";
            // 
            // column_block
            // 
            this.column_block.Text = "Блок";
            this.column_block.Width = 50;
            // 
            // column_data
            // 
            this.column_data.Text = "Данные";
            this.column_data.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.column_data.Width = 200;
            // 
            // column_type
            // 
            this.column_type.Text = "Тип";
            this.column_type.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.column_type.Width = 50;
            // 
            // column_hcIn
            // 
            this.column_hcIn.Text = "HC-in";
            this.column_hcIn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.column_hcIn.Width = 100;
            // 
            // column_hcOut
            // 
            this.column_hcOut.Text = "HC-out";
            this.column_hcOut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.column_hcOut.Width = 100;
            // 
            // column_hcReturn
            // 
            this.column_hcReturn.Text = "HC-return";
            this.column_hcReturn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.column_hcReturn.Width = 100;
            // 
            // listView
            // 
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.column_block,
            this.column_data,
            this.column_type,
            this.column_markerCode,
            this.column_hcIn,
            this.column_hcOut,
            this.column_hcReturn});
            this.listView.ContextMenuStrip = this.contextMenuStrip;
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.FullRowSelect = true;
            this.listView.GridLines = true;
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(0, 49);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(711, 421);
            this.listView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView.TabIndex = 3;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_edit,
            this.menuSeparator,
            this.menu_remove});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(192, 54);
            // 
            // menu_edit
            // 
            this.menu_edit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.menu_edit.Name = "menu_edit";
            this.menu_edit.Size = new System.Drawing.Size(191, 22);
            this.menu_edit.Text = "Редактировать блок";
            // 
            // menuSeparator
            // 
            this.menuSeparator.Name = "menuSeparator";
            this.menuSeparator.Size = new System.Drawing.Size(188, 6);
            // 
            // menu_remove
            // 
            this.menu_remove.Image = global::AudioDataInterface.Properties.Resources.icon_remove;
            this.menu_remove.Name = "menu_remove";
            this.menu_remove.Size = new System.Drawing.Size(191, 22);
            this.menu_remove.Text = "Удалить";
            // 
            // column_markerCode
            // 
            this.column_markerCode.Text = "Код маркера";
            this.column_markerCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.column_markerCode.Width = 80;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(711, 492);
            this.Controls.Add(this.listView);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.menuStrip);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainWindow";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Audio Data Interface - Main";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem оПрограммеToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripButton button_capture;
        private System.Windows.Forms.ColumnHeader column_block;
        private System.Windows.Forms.ColumnHeader column_data;
        private System.Windows.Forms.ColumnHeader column_type;
        private System.Windows.Forms.ColumnHeader column_hcIn;
        private System.Windows.Forms.ColumnHeader column_hcOut;
        private System.Windows.Forms.ColumnHeader column_hcReturn;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ToolStripButton button_encoder;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem menu_edit;
        private System.Windows.Forms.ToolStripSeparator menuSeparator;
        private System.Windows.Forms.ToolStripMenuItem menu_remove;
        private System.Windows.Forms.ColumnHeader column_markerCode;
    }
}

