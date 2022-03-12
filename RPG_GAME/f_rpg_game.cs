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
using System.IO;

namespace RPG_GAME
{
    public partial class f_rpg_game : Form
    {
        private Player _player;
        private const string PLAYER_DATA_FILE_NAME = "PlayerData.xml";
        public f_rpg_game()
        {
            InitializeComponent();

            if (File.Exists(PLAYER_DATA_FILE_NAME))
            {
                _player = Player.CreatePlayerFromXmlString(File.ReadAllText(PLAYER_DATA_FILE_NAME));
            }
            else
            {
                _player = Player.CreateDefaultPlayer();
            }

            lb_hitPoints.DataBindings.Add("Text", _player, "CurrentHitPoints");
            lb_gold.DataBindings.Add("Text", _player, "Gold");
            lb_experience.DataBindings.Add("Text", _player, "ExperiencePoints");
            lb_level.DataBindings.Add("Text", _player, "Level");

            dgv_inventory.RowHeadersVisible = false;
            dgv_inventory.AutoGenerateColumns = false;
            dgv_inventory.DataSource = _player.Inventory;

            dgv_inventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                Width = 197,
                DataPropertyName = "Description"
            });

            dgv_inventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Quantity",
                DataPropertyName = "Quantity"
            });

            dgv_quests.RowHeadersVisible = false;
            dgv_quests.AutoGenerateColumns = false;
            dgv_quests.DataSource = _player.Quests;

            dgv_quests.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                Width = 197,
                DataPropertyName = "Name"
            });

            dgv_quests.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Done?",
                DataPropertyName = "IsCompleted"
            });

            cb_weapons.DataSource = _player.Weapons;
            cb_weapons.DisplayMember = "Name";
            cb_weapons.ValueMember = "Id";

            if(_player.CurrentWeapon != null)
            {
                cb_weapons.SelectedItem = _player.CurrentWeapon;
            }

            cb_weapons.SelectedIndexChanged += cb_weapons_SelectedIndexChanged;

            cb_potions.DataSource = _player.Potions;
            cb_potions.DisplayMember = "Name";
            cb_potions.ValueMember = "Id";

            _player.PropertyChanged += PlayerOnPropertyChanged;
            _player.OnMessage += DisplayMessage;

            _player.MoveRefresh();
        }

        private void btn_north_Click(object sender, EventArgs e)
        {
            _player.MoveNorth();
        }

        private void btn_east_Click(object sender, EventArgs e)
        {
            _player.MoveEast();
        }

        private void btn_south_Click(object sender, EventArgs e)
        {
            _player.MoveSouth();
        }

        private void btn_west_Click(object sender, EventArgs e)
        {
            _player.MoveWest();
        }

        private void btn_use_weapon_Click(object sender, EventArgs e)
        {
            Weapon currentWeapon = (Weapon)cb_weapons.SelectedItem;
            _player.UseWeapon(currentWeapon);
        }

        private void btn_use_potion_Click(object sender, EventArgs e)
        {
            HealingPotion potion = (HealingPotion) cb_potions.SelectedItem;
            _player.UsePotion(potion);
        }

        private void PlayerOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "Weapons")
            {
                cb_weapons.DataSource = _player.Weapons;
                if (!_player.Weapons.Any())
                {
                    cb_weapons.Visible = false;
                    btn_use_weapon.Visible = false;
                }
            }

            if (propertyChangedEventArgs.PropertyName == "Potions")
            {
                cb_potions.DataSource = _player.Potions;
                if (!_player.Potions.Any())
                {
                    cb_potions.Visible = false;
                    btn_use_potion.Visible = false;
                }
            }

            if(propertyChangedEventArgs.PropertyName == "CurrentLocation")
            {
                btn_north.Visible = (_player.CurrentLocation.LocationToNorth != null);
                btn_east.Visible = (_player.CurrentLocation.LocationToEast != null);
                btn_south.Visible = (_player.CurrentLocation.LocationToSouth != null);
                btn_west.Visible = (_player.CurrentLocation.LocationToWest != null);

                rtb_location.Text = _player.CurrentLocation.Name + Environment.NewLine;
                rtb_location.Text += Environment.NewLine;
                rtb_location.Text += _player.CurrentLocation.Description + Environment.NewLine;

                if(_player.CurrentLocation.MonsterLivingHere == null)
                {
                    cb_weapons.Visible = false;
                    cb_potions.Visible = false;
                    btn_use_weapon.Visible = false;
                    btn_use_potion.Visible = false;
                }
                else
                {
                    cb_weapons.Visible = _player.Weapons.Any();
                    cb_potions.Visible = _player.Potions.Any();
                    btn_use_weapon.Visible = _player.Weapons.Any();
                    btn_use_potion.Visible = _player.Potions.Any();
                }
            }
        }

        private void DisplayMessage(object sender, MessageEventArgs messageEventArgs)
        {
            rtb_messages.Text += messageEventArgs.Message + Environment.NewLine;
            if (messageEventArgs.AddExtraNewLine)
            {
                rtb_messages.Text += Environment.NewLine;
            }
            rtb_messages.SelectionStart = rtb_messages.Text.Length;
            rtb_messages.ScrollToCaret();
        }

        private void f_rpg_game_FormClosed(object sender, FormClosedEventArgs e)
        {
            File.WriteAllText(PLAYER_DATA_FILE_NAME, _player.ToXmlString());
        }

        private void cb_weapons_SelectedIndexChanged(object sender, EventArgs e)
        {
            _player.CurrentWeapon = (Weapon) cb_weapons.SelectedItem;
        }
    }
}
