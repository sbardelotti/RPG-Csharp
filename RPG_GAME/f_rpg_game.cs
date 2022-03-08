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
        private Monster _currentMonster;
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

            MoveTo(_player.CurrentLocation);
            UpdatePlayerStats();
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
            UpdatePlayerStats();

            if (newLocation.QuestAvaibleHere != null)
            {
                foreach(Quest questAvaibleHere in newLocation.QuestAvaibleHere)
                {
                    bool playerAlreadyHasQuest = _player.HasThisQuest(questAvaibleHere);
                    bool playerAlreadyCompletedQuest = _player.CompletedThisQuest(questAvaibleHere);
                    bool playerHasAllItemsToCompleteQuest = _player.HasAllQuestCompletionItems(questAvaibleHere);

                    if (playerAlreadyHasQuest)
                    {
                        if (!playerAlreadyCompletedQuest)
                        {
                            if (playerHasAllItemsToCompleteQuest)
                            {
                                rtb_messages.Text += Environment.NewLine;
                                rtb_messages.Text += "You complete the " + questAvaibleHere.Name + Environment.NewLine;

                                _player.RemoveQuestCompletionItems(questAvaibleHere);

                                rtb_messages.Text += "You receive: " + Environment.NewLine;
                                rtb_messages.Text += questAvaibleHere.RewardExperiencePoints.ToString() + " experience points" + Environment.NewLine;
                                rtb_messages.Text += questAvaibleHere.RewardGold.ToString() + " gold" + Environment.NewLine;
                                if (questAvaibleHere.RewardItem != null)
                                {
                                    foreach (QuestCompletionItem qci in questAvaibleHere.RewardItem)
                                    {
                                        if (qci.Quantity > 1)
                                        {
                                            rtb_messages.Text += qci.Details.NamePlural + " x " + qci.Quantity + Environment.NewLine;
                                        }
                                        else
                                        {
                                            rtb_messages.Text += qci.Details.Name + Environment.NewLine;
                                        }

                                    }
                                }
                                rtb_messages.Text += Environment.NewLine;

                                _player.AddExperiencePoints(questAvaibleHere.RewardExperiencePoints);
                                _player.Gold += questAvaibleHere.RewardGold;
                                if (questAvaibleHere.RewardItem != null)
                                {
                                    _player.AddItemsInventory(questAvaibleHere.RewardItem);
                                }

                                _player.MarkQuestCompleted(questAvaibleHere);
                            }
                        }
                    }
                    else
                    {
                        if (questAvaibleHere.ConditionToStartQuest)
                        {
                            rtb_messages.Text += "You receive the " + questAvaibleHere.Name + " quest." + Environment.NewLine;
                            rtb_messages.Text += questAvaibleHere.Description + Environment.NewLine;
                            rtb_messages.Text += "To complete it, return with:" + Environment.NewLine;

                            foreach (QuestCompletionItem qci in questAvaibleHere.QuestCompletionItems)
                            {
                                if (qci.Quantity == 1)
                                {
                                    rtb_messages.Text += qci.Quantity.ToString() + " " + qci.Details.Name + Environment.NewLine;
                                }
                                else
                                {
                                    rtb_messages.Text += qci.Quantity.ToString() + " " + qci.Details.NamePlural + Environment.NewLine;
                                }
                            }

                            rtb_messages.Text += Environment.NewLine;
                            _player.Quests.Add(new PlayerQuest(questAvaibleHere));
                        }
                    }
                }
            }

            if(newLocation.MonsterLivingHere != null)
            {
                rtb_messages.Text += "You see a " + newLocation.MonsterLivingHere.Name + Environment.NewLine;
                rtb_messages.Text += Environment.NewLine;

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
            ScrollToBottomOfMessages();
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
                cb_weapons.SelectedIndexChanged -= cb_weapons_SelectedIndexChanged;
                cb_weapons.DataSource = weapons;
                cb_weapons.SelectedIndexChanged += cb_weapons_SelectedIndexChanged;
                cb_weapons.DisplayMember = "Name";
                cb_weapons.ValueMember = "ID";

                if(_player.CurrentWeapon != null)
                {
                    cb_weapons.SelectedItem = _player.CurrentWeapon;
                }
                else
                {
                    cb_weapons.SelectedIndex = 0;
                }
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
            Weapon currentWeapon = (Weapon)cb_weapons.SelectedItem;
            int damageToMonster = RandomNumberGenerator.NumberBetween(currentWeapon.MinimumDamage, currentWeapon.MaximumDamage);

            _currentMonster.CurrentHitPoints -= damageToMonster;

            rtb_messages.Text += "You hit the " + _currentMonster.Name + " for " + damageToMonster.ToString() + " points." + Environment.NewLine;
            
            if(_currentMonster.CurrentHitPoints <= 0)
            {
                rtb_messages.Text += Environment.NewLine;
                rtb_messages.Text += "You defeated the " + _currentMonster.Name + Environment.NewLine;

                _player.AddExperiencePoints(_currentMonster.RewardExperiencePoints);
                rtb_messages.Text += "You receive " + _currentMonster.RewardExperiencePoints + " experience points." + Environment.NewLine;
                _player.Gold += _currentMonster.RewardGold;
                rtb_messages.Text += "You receive " + _currentMonster.RewardGold + " gold." + Environment.NewLine;

                List<QuestCompletionItem> lootedItems = new List<QuestCompletionItem>();

                foreach(LootItem lootItem in _currentMonster.LootTable)
                {
                    if(RandomNumberGenerator.NumberBetween(1, 100) <= lootItem.DropPercentage)
                    {
                        lootedItems.Add(new QuestCompletionItem(lootItem.Details, RandomNumberGenerator.NumberBetween(1, lootItem.MaximumDrop)));
                    }
                }

                if(lootedItems.Count == 0)
                {
                    foreach(LootItem lootItem in _currentMonster.LootTable)
                    {
                        if (lootItem.IsDefaultItem)
                        {
                            lootedItems.Add(new QuestCompletionItem(lootItem.Details, 1));
                        }
                    }
                }

                if (lootedItems.Count != 0)
                {
                    _player.AddItemsInventory(lootedItems);

                    foreach (QuestCompletionItem qci in lootedItems)
                    {
                        if (qci.Quantity == 1)
                        {
                            rtb_messages.Text += "You loot 1 " + qci.Details.Name + Environment.NewLine;
                        }
                        else
                        {
                            rtb_messages.Text += "You loot " + qci.Quantity.ToString() + " " + qci.Details.NamePlural + Environment.NewLine;
                        }
                    }
                }
                else
                {
                    rtb_messages.Text += "You loot nothing" + Environment.NewLine;
                }
                UpdateInventoryListInUI();
                UpdateWeaponListInUI();
                UpdatePotionListInUI();
                
                rtb_messages.Text += Environment.NewLine;

                MoveTo(_player.CurrentLocation);

            }
            else
            {
                int damageToPlayer = RandomNumberGenerator.NumberBetween(0, _currentMonster.MaximumHitPoints);

                rtb_messages.Text += "The " + _currentMonster.Name + " did " + damageToPlayer.ToString() + " points of damage." + Environment.NewLine;
                _player.CurrentHitPoints -= damageToPlayer;

                if(_player.CurrentHitPoints <= 0)
                {
                    rtb_messages.Text += "The " + _currentMonster.Name + " killed you." + Environment.NewLine;
                    MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
                }

                
            }
            ScrollToBottomOfMessages();
            UpdatePlayerStats();
        }

        private void btn_use_potion_Click(object sender, EventArgs e)
        {
            HealingPotion potion = (HealingPotion) cb_potions.SelectedItem;

            _player.CurrentHitPoints += potion.AmountToHeal;

            if(_player.CurrentHitPoints > _player.MaximumHitPoints)
            {
                _player.CurrentHitPoints = _player.MaximumHitPoints;
            }

            foreach(InventoryItem ii in _player.Inventory)
            {
                if(ii.Details.ID == potion.ID)
                {
                    ii.Quantity--;
                    break;
                }
            }

            rtb_messages.Text += "You drink a " + potion.Name + Environment.NewLine;

            int damageToPlayer = RandomNumberGenerator.NumberBetween(0, _currentMonster.MaximumHitPoints);

            rtb_messages.Text += "The " + _currentMonster.Name + " did " + damageToPlayer.ToString() + " points of damage." + Environment.NewLine;
            _player.CurrentHitPoints -= damageToPlayer;
           
            if (_player.CurrentHitPoints <= 0)
            {
                rtb_messages.Text += "The " + _currentMonster.Name + " killed you." + Environment.NewLine;
                MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
            }

            ScrollToBottomOfMessages();
            UpdateInventoryListInUI();
            UpdatePotionListInUI();
        }

        private void ScrollToBottomOfMessages()
        {
            rtb_messages.SelectionStart = rtb_messages.Text.Length;
            rtb_messages.ScrollToCaret();
        }

        private void UpdatePlayerStats()
        {
            lb_hitPoints.Text = _player.CurrentHitPoints.ToString();
            lb_gold.Text = _player.Gold.ToString();
            lb_experience.Text = _player.ExperiencePoints.ToString();
            lb_level.Text = _player.Level.ToString();
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
