using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Motor;

namespace RPG_GAME
{
    public partial class TradingScreen : Form
    {
        private Player _currentPlayer;
        public TradingScreen(Player player)
        {
            _currentPlayer = player;
            InitializeComponent();

            DataGridViewCellStyle rightAlignedCellStyle = new DataGridViewCellStyle();
            rightAlignedCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgv_my_items.RowHeadersVisible = false;
            dgv_my_items.AutoGenerateColumns = false;

            //hidden ID column
            dgv_my_items.Columns.Add(new DataGridViewTextBoxColumn { 
                DataPropertyName = "ItemID",
                Visible = false
            });

            dgv_my_items.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                Width = 100,
                DataPropertyName = "Description"
            });

            dgv_my_items.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Qty",
                Width = 30,
                DefaultCellStyle = rightAlignedCellStyle,
                DataPropertyName = "Quantity"
            });

            dgv_my_items.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Price",
                Width = 35,
                DefaultCellStyle = rightAlignedCellStyle,
                DataPropertyName = "Price"
            });

            dgv_my_items.Columns.Add(new DataGridViewButtonColumn
            {
                Text = "Sell 1",
                UseColumnTextForButtonValue = true,
                Width = 50,
                DataPropertyName = "ItemID"
            });

            dgv_my_items.DataSource = _currentPlayer.Inventory;

            dgv_my_items.CellClick += dgv_my_items_CellClick;

            dgv_vendor_items.RowHeadersVisible = false;
            dgv_vendor_items.AutoGenerateColumns = false;

            dgv_vendor_items.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ItemID",
                Visible = false
            });

            dgv_vendor_items.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                Width = 100,
                DataPropertyName = "Description"
            });

            dgv_vendor_items.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Price",
                Width = 35,
                DefaultCellStyle = rightAlignedCellStyle,
                DataPropertyName = "Price"
            });

            dgv_vendor_items.Columns.Add(new DataGridViewButtonColumn
            {
                Text = "Buy 1",
                UseColumnTextForButtonValue = true,
                Width = 50,
                DataPropertyName = "ItemID"
            });

            dgv_vendor_items.DataSource = _currentPlayer.CurrentLocation.VendorWorkingHere.Inventory;
            dgv_vendor_items.CellClick += dgv_vendor_items_CellClick;
        }

        private void dgv_my_items_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //column 4 -> button
            if(e.ColumnIndex == 4)
            {
                var itemID = dgv_my_items.Rows[e.RowIndex].Cells[0].Value;
                Item itemBeingSold = World.ItemByID(Convert.ToInt32(itemID));

                if(itemBeingSold.Price == World.UNSSELLABLE_ITEM_PRICE)
                {
                    MessageBox.Show("You cannot sell the " + itemBeingSold.Name);
                }
                else
                {
                    _currentPlayer.RemoveItemFromInventory(itemBeingSold);
                    _currentPlayer.Gold += itemBeingSold.Price;
                }
            }
        }
        private void dgv_vendor_items_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //column 3 -> button
            if(e.ColumnIndex == 3)
            {
                var itemID = dgv_vendor_items.Rows[e.RowIndex].Cells[0].Value;
                Item itemBeingBought = World.ItemByID(Convert.ToInt32(itemID));

                if(_currentPlayer.Gold >= itemBeingBought.Price)
                {
                    List<QuestCompletionItem> item = new List<QuestCompletionItem>();
                    item.Add(new QuestCompletionItem(itemBeingBought, 1));
                    _currentPlayer.AddItemsInventory(item);

                    _currentPlayer.Gold -= itemBeingBought.Price;
                }
                else
                {
                    MessageBox.Show("You do not have enough gold to buy the " + itemBeingBought.Name);
                }
            }
        }
        private void btn_close_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
