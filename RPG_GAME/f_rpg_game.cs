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
    public partial class f_rpg_game : Form
    {
        private Player _player;
        private Monster _currentMonster;
        public f_rpg_game()
        {
            InitializeComponent();

            _player = new Player(10, 10, 20, 0, 1);
            _player.Inventory.Add(new InventoryItem(World.ItemByID(World.ITEM_ID_RUSTY_SWORD), 1));

            MoveTo(World.LocationByID(World.LOCATION_ID_HOME));

            lb_hitPoints.Text = _player.CurrentHitPoints.ToString();
            lb_gold.Text = _player.Gold.ToString();
            lb_experience.Text = _player.ExperiencePoints.ToString();
            lb_level.Text = _player.Level.ToString();
        }

        private void btn_north_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToNorth);
        }

        private void btn_east_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToEast);
        }

        private void btn_south_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToSouth);
        }

        private void btn_west_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToWest);
        }

        private void MoveTo(Location newLocation)
        {
            if(!_player.HasRequiredItemToEnterThisLocation(newLocation))
            {
                rtb_messages.Text += "You must have a " + newLocation.ItemRequiredToEnter.Name + "to enter this location" + Environment.NewLine;
                rtb_messages.Text += Environment.NewLine;
                return;
            }

            _player.CurrentLocation = newLocation;

            btn_north.Visible = (newLocation.LocationToNorth != null);
            btn_east.Visible = (newLocation.LocationToEast != null);
            btn_south.Visible = (newLocation.LocationToSouth != null);
            btn_west.Visible = (newLocation.LocationToWest != null);

            rtb_location.Text = newLocation.Name + Environment.NewLine;
            rtb_location.Text += Environment.NewLine;
            rtb_location.Text += newLocation.Description + Environment.NewLine;

            _player.CurrentHitPoints = _player.MaximumHitPoints;
            lb_hitPoints.Text = _player.CurrentHitPoints.ToString();

            if(newLocation.QuestAvaibleHere != null)
            {
                bool playerAlreadyHasQuest = _player.HasThisQuest(newLocation.QuestAvaibleHere);
                bool playerAlreadyCompletedQuest = _player.CompletedThisQuest(newLocation.QuestAvaibleHere);
                bool playerHasAllItemsToCompleteQuest = _player.HasAllQuestCompletionItems(newLocation.QuestAvaibleHere);

                if (playerAlreadyHasQuest)
                {
                    if(!playerAlreadyCompletedQuest)
                    {
                        if(playerHasAllItemsToCompleteQuest)
                        {
                            rtb_messages.Text += Environment.NewLine;
                            rtb_messages.Text += "You complete the " + newLocation.QuestAvaibleHere.Name + Environment.NewLine;

                            _player.RemoveQuestCompletionItems(newLocation.QuestAvaibleHere);

                            rtb_messages.Text += "You receive: " + Environment.NewLine;
                            rtb_messages.Text += newLocation.QuestAvaibleHere.RewardExperiencePoints.ToString() + " experience points" + Environment.NewLine;
                            rtb_messages.Text += newLocation.QuestAvaibleHere.RewardGold.ToString() + " gold" + Environment.NewLine;
                            rtb_messages.Text += newLocation.QuestAvaibleHere.RewardItem.Name + Environment.NewLine;
                            rtb_messages.Text += Environment.NewLine;

                            _player.ExperiencePoints += newLocation.QuestAvaibleHere.RewardExperiencePoints;
                            _player.Gold += newLocation.QuestAvaibleHere.RewardGold;
                            _player.AddItemInventory(newLocation.QuestAvaibleHere.RewardItem);

                            _player.MarkQuestCompleted(newLocation.QuestAvaibleHere);
                        }
                    }
                }
                else
                {
                    rtb_messages.Text += "You receive the " + newLocation.QuestAvaibleHere.Name + " quest." + Environment.NewLine;
                    rtb_messages.Text += newLocation.QuestAvaibleHere.Description + Environment.NewLine;
                    rtb_messages.Text += "To complete it, return with:" + Environment.NewLine;

                    foreach(QuestCompletionItem qci in newLocation.QuestAvaibleHere.QuestCompletionItems)
                    {
                        if(qci.Quantity == 1)
                        {
                            rtb_messages.Text += qci.Quantity.ToString() + " " + qci.Details.Name + Environment.NewLine;
                        }
                        else
                        {
                            rtb_messages.Text += qci.Quantity.ToString() + " " + qci.Details.NamePlural + Environment.NewLine;
                        }
                    }

                    rtb_messages.Text += Environment.NewLine;
                    _player.Quests.Add(new PlayerQuest(newLocation.QuestAvaibleHere));
                }
            }

            if(newLocation.MonsterLivingHere != null)
            {
                rtb_messages.Text += "You see a " + newLocation.MonsterLivingHere.Name + Environment.NewLine;

                Monster standardMonster = World.MonsterByID(newLocation.MonsterLivingHere.ID);
                _currentMonster = new Monster(standardMonster.ID, standardMonster.Name, standardMonster.MaximumDamage, standardMonster.RewardExperiencePoints, standardMonster.RewardGold, standardMonster.CurrentHitPoints, standardMonster.MaximumDamage);

                foreach(LootItem lootItem in standardMonster.LootTable)
                {
                    _currentMonster.LootTable.Add(lootItem);
                }

                cb_weapons.Visible = true;
                cb_potions.Visible = true;
                btn_use_weapon.Visible = true;
                btn_use_potion.Visible = true;
            }
            else
            {
                _currentMonster = null;

                cb_weapons.Visible = false;
                cb_potions.Visible = false;
                btn_use_weapon.Visible = false;
                btn_use_potion.Visible = false;
            }

            UpdateInventoryListInUI();
            UpdateQuestListInUI();
            UpdateWeaponListInUI();
            UpdatePotionListInUI();
        }

        private void UpdateInventoryListInUI()
        {
            dgv_inventory.RowHeadersVisible = false;

            dgv_inventory.ColumnCount = 2;
            dgv_inventory.Columns[0].Name = "Name";
            dgv_inventory.Columns[1].Width = 197;
            dgv_inventory.Columns[1].Name = "Quantity";

            dgv_inventory.Rows.Clear();

            foreach (InventoryItem ii in _player.Inventory)
            {
                if (ii.Quantity > 0)
                {
                    dgv_inventory.Rows.Add(new[] { ii.Details.Name, ii.Quantity.ToString() });
                }
            }
        }

        private void UpdateQuestListInUI()
        {
            dgv_quests.RowHeadersVisible = false;

            dgv_quests.ColumnCount = 2;
            dgv_quests.Columns[0].Name = "Name";
            dgv_quests.Columns[1].Width = 197;
            dgv_quests.Columns[1].Name = "Done?";

            dgv_quests.Rows.Clear();

            foreach (PlayerQuest pq in _player.Quests)
            {
                dgv_quests.Rows.Add(new[] { pq.Details.Name, pq.IsCompleted.ToString() });
            }
        }

        private void UpdateWeaponListInUI()
        {
            List<Weapon> weapons = new List<Weapon>();

            foreach (InventoryItem ii in _player.Inventory)
            {
                if (ii.Details is Weapon)
                {
                    if (ii.Quantity > 0)
                    {
                        weapons.Add((Weapon)ii.Details);
                    }
                }
            }

            if (weapons.Count == 0)
            {
                cb_weapons.Visible = false;
                btn_use_weapon.Visible = false;
            }
            else
            {
                cb_weapons.DataSource = weapons;
                cb_weapons.DisplayMember = "Name";
                cb_weapons.ValueMember = "ID";
                cb_weapons.SelectedIndex = 0;
            }
        }

        private void UpdatePotionListInUI()
        {
            List<HealingPotion> healingPotions = new List<HealingPotion>();

            foreach (InventoryItem ii in _player.Inventory)
            {
                if (ii.Details is HealingPotion)
                {
                    if (ii.Quantity > 0)
                    {
                        healingPotions.Add((HealingPotion)ii.Details);
                    }
                }
            }

            if (healingPotions.Count == 0)
            {
                cb_potions.Visible = false;
                btn_use_potion.Visible = false;
            }
            else
            {
                cb_potions.DataSource = healingPotions;
                cb_potions.DisplayMember = "Name";
                cb_potions.ValueMember = "ID";
                cb_potions.SelectedIndex = 0;
            }
        }

        private void btn_use_weapon_Click(object sender, EventArgs e)
        {

        }

        private void btn_use_potion_Click(object sender, EventArgs e)
        {

        }
    }
}
