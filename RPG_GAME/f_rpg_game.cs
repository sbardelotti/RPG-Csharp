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

        }

        private void btn_east_Click(object sender, EventArgs e)
        {

        }

        private void btn_south_Click(object sender, EventArgs e)
        {

        }

        private void btn_west_Click(object sender, EventArgs e)
        {

        }

        private void MoveTo(Location newLocation)
        {
            if(newLocation.ItemRequiredToEnter != null)
            {
                bool playerHasRequiredItem = false;

                foreach(InventoryItem ii in _player.Inventory)
                {
                    if(ii.Details.ID == newLocation.ItemRequiredToEnter.ID)
                    {
                        playerHasRequiredItem = true;
                        break;
                    }
                }

                if (!playerHasRequiredItem)
                {
                    rtb_messages.Text += "You must have a " + newLocation.ItemRequiredToEnter.Name + "to enter this location" + Environment.NewLine;
                    return;
                }
            }

            _player.CurrentLocation = newLocation;

            btn_north.Visible = (newLocation.LocationToNorth != null);
            btn_east.Visible = (newLocation.LocationToEast != null);
            btn_south.Visible = (newLocation.LocationToSouth != null);
            btn_west.Visible = (newLocation.LocationToWest != null);

            rtb_location.Text = newLocation.Name + Environment.NewLine;
            rtb_location.Text = newLocation.Description + Environment.NewLine;

            _player.CurrentHitPoints = _player.MaximumHitPoints;
            lb_hitPoints.Text = _player.CurrentHitPoints.ToString();

            if(newLocation.QuestAvaibleHere != null)
            {
                bool playerAlreadyHasQuest = false;
                bool playerAlreadyCompletedQuest = false;

                foreach (PlayerQuest playerQuest in _player.Quests)
                {
                    if(playerQuest.Details.ID == newLocation.QuestAvaibleHere.ID)
                    {
                        playerAlreadyHasQuest = true;

                        if (playerQuest.IsCompleted)
                        {
                            playerAlreadyCompletedQuest = true;
                        }
                    }
                }

                if(playerAlreadyHasQuest)
                {
                    if(!playerAlreadyCompletedQuest)
                    {
                        bool playerHasAllItemsToCompleteQuest = true;

                        foreach (QuestCompletionItem qci in newLocation.QuestAvaibleHere.QuestCompletionItems)
                        {
                            bool foundItemInPlayersInventory = false;

                            foreach(InventoryItem ii in _player.Inventory)
                            {
                                if(ii.Details.ID == qci.Details.ID)
                                {
                                    foundItemInPlayersInventory = true;

                                    if(ii.Quantity < qci.Quantity)
                                    {
                                        playerHasAllItemsToCompleteQuest = false;
                                        break;
                                    }

                                    break;
                                }
                            }

                            if(!foundItemInPlayersInventory)
                            {
                                playerHasAllItemsToCompleteQuest = false;
                                break;
                            }
                        }

                        if(playerHasAllItemsToCompleteQuest)
                        {
                            rtb_messages.Text += Environment.NewLine;
                            rtb_messages.Text += "You complete the " + newLocation.QuestAvaibleHere.Name + Environment.NewLine;
                        
                            foreach(QuestCompletionItem qci in newLocation.QuestAvaibleHere.QuestCompletionItems)
                            {
                                foreach(InventoryItem ii in _player.Inventory)
                                {
                                    if(ii.Details.ID == qci.Details.ID)
                                    {
                                        ii.Quantity -= qci.Quantity;
                                        break;
                                    }
                                }
                            }

                            rtb_messages.Text += "You receive: " + Environment.NewLine;
                            rtb_messages.Text += newLocation.QuestAvaibleHere.RewardExperiencePoints.ToString() + " experience points" + Environment.NewLine;
                            rtb_messages.Text += newLocation.QuestAvaibleHere.RewardGold.ToString() + " gold" + Environment.NewLine;

                        }
                    }
                }
            }
        }

    }
}
