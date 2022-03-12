
namespace RPG_GAME
{
    partial class TradingScreen
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
            this.lb_my_inventory = new System.Windows.Forms.Label();
            this.lb_vendor_inventory = new System.Windows.Forms.Label();
            this.dgv_my_items = new System.Windows.Forms.DataGridView();
            this.dgv_vendor_items = new System.Windows.Forms.DataGridView();
            this.btn_close = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_my_items)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_vendor_items)).BeginInit();
            this.SuspendLayout();
            // 
            // lb_my_inventory
            // 
            this.lb_my_inventory.AutoSize = true;
            this.lb_my_inventory.Location = new System.Drawing.Point(99, 13);
            this.lb_my_inventory.Name = "lb_my_inventory";
            this.lb_my_inventory.Size = new System.Drawing.Size(72, 13);
            this.lb_my_inventory.TabIndex = 0;
            this.lb_my_inventory.Text = "My Inventory";
            // 
            // lb_vendor_inventory
            // 
            this.lb_vendor_inventory.AutoSize = true;
            this.lb_vendor_inventory.Location = new System.Drawing.Point(349, 13);
            this.lb_vendor_inventory.Name = "lb_vendor_inventory";
            this.lb_vendor_inventory.Size = new System.Drawing.Size(99, 13);
            this.lb_vendor_inventory.TabIndex = 1;
            this.lb_vendor_inventory.Text = "Vendor\'s Inventory";
            // 
            // dgv_my_items
            // 
            this.dgv_my_items.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_my_items.Location = new System.Drawing.Point(13, 43);
            this.dgv_my_items.Name = "dgv_my_items";
            this.dgv_my_items.RowTemplate.Height = 23;
            this.dgv_my_items.Size = new System.Drawing.Size(240, 216);
            this.dgv_my_items.TabIndex = 2;
            //this.dgv_my_items.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_my_items_CellClick);
            // 
            // dgv_vendor_items
            // 
            this.dgv_vendor_items.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_vendor_items.Location = new System.Drawing.Point(276, 43);
            this.dgv_vendor_items.Name = "dgv_vendor_items";
            this.dgv_vendor_items.RowTemplate.Height = 23;
            this.dgv_vendor_items.Size = new System.Drawing.Size(240, 216);
            this.dgv_vendor_items.TabIndex = 3;
            // 
            // btn_close
            // 
            this.btn_close.Location = new System.Drawing.Point(441, 274);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(75, 23);
            this.btn_close.TabIndex = 4;
            this.btn_close.Text = "Close";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // TradingScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 322);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.dgv_vendor_items);
            this.Controls.Add(this.dgv_my_items);
            this.Controls.Add(this.lb_vendor_inventory);
            this.Controls.Add(this.lb_my_inventory);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TradingScreen";
            this.Text = "Trade";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_my_items)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_vendor_items)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lb_my_inventory;
        private System.Windows.Forms.Label lb_vendor_inventory;
        private System.Windows.Forms.DataGridView dgv_my_items;
        private System.Windows.Forms.DataGridView dgv_vendor_items;
        private System.Windows.Forms.Button btn_close;
    }
}