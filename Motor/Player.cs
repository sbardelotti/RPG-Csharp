using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.ComponentModel;

namespace Motor
{
    public class Player : LivingCreature
    {
        private Monster _currentMonster;
        private int _gold;
        private int _experiencePoints;
        private int _level;
        private Location _currentLocation;

        public int Gold {
            get { return _gold; }
            set
            {
                _gold = value;
                OnPropertyChanged("Gold");
            }
        }
        public int ExperiencePoints {
            get { return _experiencePoints; } 
            private set
            {
                _experiencePoints = value;
                OnPropertyChanged("ExperiencePoints");
            }
        }
        public int Level
        {
            get { return _level; }
            private set
            {
                _level = value;
                OnPropertyChanged("Level");
            }
        }
        public Weapon CurrentWeapon { get; set; }
        public Location CurrentLocation
        {
            get { return _currentLocation; }
            set
            {
                _currentLocation = value;
                OnPropertyChanged("CurrentLocation");
            }
        }
        public BindingList<InventoryItem> Inventory { get; set; }
        public BindingList<PlayerQuest> Quests { get; set; }
        public List<Weapon> Weapons
        {
            get
            {
                return Inventory.Where(x => x.Details is Weapon).Select(x => x.Details as Weapon).ToList();
            }
        }
        public List<HealingPotion> Potions
        {
            get
            {
                return Inventory.Where(x => x.Details is HealingPotion).Select(x => x.Details as HealingPotion).ToList();
            }
        }

        private Player(int currentHitPoints, int maximumHitPoints, int gold, int experiencePoints) : base(currentHitPoints, maximumHitPoints)
        {
            Gold = gold;
            ExperiencePoints = experiencePoints;
            Inventory = new BindingList<InventoryItem>();
            Quests = new BindingList<PlayerQuest>();
        }

        public static Player CreateDefaultPlayer()
        {
            Player player = new Player(10, 10, 20, 0);
            player.Inventory.Add(new InventoryItem(World.ItemByID(World.ITEM_ID_RUSTY_SWORD), 1));
            player.CurrentLocation = World.LocationByID(World.LOCATION_ID_HOME);

            return player;
        }

        public void AddExperiencePoints(int experiencePointsToAdd)
        {
            ExperiencePoints += experiencePointsToAdd;
            MaximumHitPoints = (10 + (2*Level));
            int levelBefore = Level;
            if((ExperiencePoints / (100 * (Level + 1))) >= 1)
            {
                Level++;
            }
            if(levelBefore != Level)
            {
                ExperiencePoints -= 100 * Level;
            }
        }

        public static Player CreatePlayerFromXmlString(string xmlPlayerData)
        {
            try
            {
                XmlDocument playerData = new XmlDocument();
                playerData.LoadXml(xmlPlayerData);

                int currentHitPoints = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentHitPoints").InnerText);
                int maximumHitPoints = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/MaximumHitPoints").InnerText);
                int gold = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/Gold").InnerText);
                int experiencePoints = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/ExperiencePoints").InnerText);
                int level = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/Level").InnerText);

                Player player = new Player(currentHitPoints, maximumHitPoints, gold, experiencePoints);

                int currentLocationID = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentLocation").InnerText);
                player.CurrentLocation = World.LocationByID(currentLocationID);
                player.Level = level;

                if (playerData.SelectSingleNode("/Player/Stats/CurrentWeapon") != null)
                {
                    int currentWeaponID = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentWeapon").InnerText);
                    player.CurrentWeapon = (Weapon)World.ItemByID(currentWeaponID);
                }

                List<QuestCompletionItem> itemsToAdd = new List<QuestCompletionItem>();

                foreach (XmlNode node in playerData.SelectNodes("/Player/InventoryItems/InventoryItem"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);
                    int quantity = Convert.ToInt32(node.Attributes["Quantity"].Value);

                    itemsToAdd.Add(new QuestCompletionItem(World.ItemByID(id), quantity));
                }

                player.AddItemsInventory(itemsToAdd);

                foreach(XmlNode node in playerData.SelectNodes("/Player/PlayerQuests/PlayerQuest"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);
                    bool isCompleted = Convert.ToBoolean(node.Attributes["IsCompleted"].Value);

                    PlayerQuest playerQuest = new PlayerQuest(World.QuestByID(id));
                    playerQuest.IsCompleted = isCompleted;
                    if(playerQuest.IsCompleted&&playerQuest.Details.IDPosteriorQuest!=0)
                    {
                        World.QuestByID(playerQuest.Details.IDPosteriorQuest).ConditionToStartQuest = true;
                    }

                    player.Quests.Add(playerQuest);
                }

                return player;
            }
            catch
            {
                return Player.CreateDefaultPlayer();
            }

        }

        public bool HasRequiredItemToEnterThisLocation(Location location)
        {
            if(location.ItemRequiredToEnter == null)
            {
                return true;
            }

            return Inventory.Any(ii => ii.Details.ID == location.ItemRequiredToEnter.ID);
        }

        public bool HasThisQuest(Quest quest)
        {
            return Quests.Any(pq => pq.Details.ID == quest.ID);
        }

        public bool CompletedThisQuest(Quest quest)
        {
            foreach(PlayerQuest playerQuest in Quests)
            {
                if(playerQuest.Details.ID == quest.ID)
                {
                    return playerQuest.IsCompleted;
                }
            }

            return false;
        }

        public bool HasAllQuestCompletionItems(Quest quest)
        {
            foreach(QuestCompletionItem qci in quest.QuestCompletionItems)
            {            
                if(!Inventory.Any(ii => ii.Details.ID == qci.Details.ID && ii.Quantity >= qci.Quantity))
                {
                    return false;
                }
            }

            return true;
        }

        public void RemoveQuestCompletionItems(Quest quest)
        {
            foreach(QuestCompletionItem qci in quest.QuestCompletionItems)
            {
                InventoryItem item = Inventory.SingleOrDefault(ii => ii.Details.ID == qci.Details.ID);
                if(item != null)
                {
                    RemoveItemFromInventory(item.Details, qci.Quantity);
                }
            }
        }

        public void AddItemsInventory(List<QuestCompletionItem> listItem)
        {
            foreach(QuestCompletionItem itemToAdd in listItem) 
            {
                InventoryItem item = Inventory.SingleOrDefault(ii => ii.Details.ID == itemToAdd.Details.ID);
                if(item == null)
                {
                    Inventory.Add(new InventoryItem(itemToAdd.Details, itemToAdd.Quantity));
                }
                else
                {
                    item.Quantity += itemToAdd.Quantity;
                }

                RaiseInventoryChangedEvent(itemToAdd.Details);
            }
        }

        public void MarkQuestCompleted(Quest quest)
        {
            PlayerQuest playerQuest = Quests.SingleOrDefault(pq => pq.Details.ID == quest.ID);
            if(playerQuest != null)
            {
                playerQuest.IsCompleted = true;
                if(playerQuest.Details.IDPosteriorQuest != 0)
                {
                    World.QuestByID(playerQuest.Details.IDPosteriorQuest).ConditionToStartQuest = true;
                }
            }
        }

        private void RaiseInventoryChangedEvent(Item item)
        {
            if (item is Weapon)
            {
                OnPropertyChanged("Weapons");
            }
            if (item is HealingPotion)
            {
                OnPropertyChanged("Potions");
            }
        }

        public void RemoveItemFromInventory(Item itemToRemove, int quantity = 1)
        {
            InventoryItem item = Inventory.SingleOrDefault(ii => ii.Details.ID == itemToRemove.ID);

            if (item == null)
            {
            
            }
            else
            {
                item.Quantity -= quantity;

                if (item.Quantity < 0)
                {
                    item.Quantity = 0;
                }

                if (item.Quantity == 0)
                {
                    Inventory.Remove(item);
                }

                RaiseInventoryChangedEvent(itemToRemove);
            }
        }

        public void MoveNorth()
        {
            if (CurrentLocation.LocationToNorth != null)
            {
                MoveTo(CurrentLocation.LocationToNorth);
            }
        }
        public void MoveEast()
        {
            if (CurrentLocation.LocationToEast != null)
            {
                MoveTo(CurrentLocation.LocationToEast);
            }
        }
        public void MoveSouth()
        {
            if (CurrentLocation.LocationToSouth != null)
            {
                MoveTo(CurrentLocation.LocationToSouth);
            }
        }
        public void MoveWest()
        {
            if (CurrentLocation.LocationToWest != null)
            {
                MoveTo(CurrentLocation.LocationToWest);
            }
        }

        private void MoveTo(Location newLocation)
        {
            if (!HasRequiredItemToEnterThisLocation(newLocation))
            {
                RaiseMessage("You must have a " + newLocation.ItemRequiredToEnter.Name + "to enter this location", true);
                return;
            }

            CurrentLocation = newLocation;

            CurrentHitPoints = MaximumHitPoints;

            if (newLocation.QuestAvaibleHere != null)
            {
                foreach (Quest questAvaibleHere in newLocation.QuestAvaibleHere)
                {
                    bool playerAlreadyHasQuest = HasThisQuest(questAvaibleHere);
                    bool playerAlreadyCompletedQuest = CompletedThisQuest(questAvaibleHere);
                    bool playerHasAllItemsToCompleteQuest = HasAllQuestCompletionItems(questAvaibleHere);

                    if (playerAlreadyHasQuest)
                    {
                        if (!playerAlreadyCompletedQuest)
                        {
                            if (playerHasAllItemsToCompleteQuest)
                            {
                                RaiseMessage("");
                                RaiseMessage("You complete the " + questAvaibleHere.Name);

                                RemoveQuestCompletionItems(questAvaibleHere);

                                RaiseMessage("You receive: ");
                                RaiseMessage(questAvaibleHere.RewardExperiencePoints.ToString() + " experience points");
                                RaiseMessage(questAvaibleHere.RewardGold.ToString() + " gold");

                                if (questAvaibleHere.RewardItem != null)
                                {
                                    foreach (QuestCompletionItem qci in questAvaibleHere.RewardItem)
                                    {
                                        if (qci.Quantity > 1)
                                        {
                                            RaiseMessage(qci.Details.NamePlural + " x " + qci.Quantity);
                                        }
                                        else
                                        {
                                            RaiseMessage(qci.Details.Name);
                                        }

                                    }
                                }

                                RaiseMessage("");

                                AddExperiencePoints(questAvaibleHere.RewardExperiencePoints);
                                Gold += questAvaibleHere.RewardGold;
                                if (questAvaibleHere.RewardItem != null)
                                {
                                    AddItemsInventory(questAvaibleHere.RewardItem);
                                }

                                MarkQuestCompleted(questAvaibleHere);
                            }
                        }
                    }
                    else
                    {
                        if (questAvaibleHere.ConditionToStartQuest)
                        {

                            RaiseMessage("You receive the " + questAvaibleHere.Name + " quest.");
                            RaiseMessage(questAvaibleHere.Description);
                            RaiseMessage("To complete it, return with:");

                            foreach (QuestCompletionItem qci in questAvaibleHere.QuestCompletionItems)
                            {
                                if (qci.Quantity == 1)
                                {
                                    RaiseMessage(qci.Quantity.ToString() + " " + qci.Details.Name);
                                }
                                else
                                {
                                    RaiseMessage(qci.Quantity.ToString() + " " + qci.Details.NamePlural);
                                }
                            }

                            RaiseMessage("");
                            Quests.Add(new PlayerQuest(questAvaibleHere));
                        }
                    }
                }
            }

            if (newLocation.MonsterLivingHere != null)
            {
                RaiseMessage("You see a " + newLocation.MonsterLivingHere.Name, true);

                Monster standardMonster = World.MonsterByID(newLocation.MonsterLivingHere.ID);
                _currentMonster = new Monster(standardMonster.ID, standardMonster.Name, standardMonster.MaximumDamage, standardMonster.RewardExperiencePoints, standardMonster.RewardGold, standardMonster.CurrentHitPoints, standardMonster.MaximumDamage);

                foreach (LootItem lootItem in standardMonster.LootTable)
                {
                    _currentMonster.LootTable.Add(lootItem);
                }
            }
            else
            {
                _currentMonster = null;
            }
        }

        public void UseWeapon(Weapon weapon)
        {
            int damageToMonster = RandomNumberGenerator.NumberBetween(weapon.MinimumDamage, weapon.MaximumDamage);

            _currentMonster.CurrentHitPoints -= damageToMonster;

            RaiseMessage("You hit the " + _currentMonster.Name + " for " + damageToMonster.ToString() + " points.");

            if (_currentMonster.CurrentHitPoints <= 0)
            {
                RaiseMessage("");
                RaiseMessage("You defeated the " + _currentMonster.Name);

                AddExperiencePoints(_currentMonster.RewardExperiencePoints);
                RaiseMessage("You receive " + _currentMonster.RewardExperiencePoints + " experience points.");
                Gold += _currentMonster.RewardGold;
                RaiseMessage("You receive " + _currentMonster.RewardGold + " gold.");

                List<QuestCompletionItem> lootedItems = new List<QuestCompletionItem>();

                foreach (LootItem lootItem in _currentMonster.LootTable)
                {
                    if (RandomNumberGenerator.NumberBetween(1, 100) <= lootItem.DropPercentage)
                    {
                        lootedItems.Add(new QuestCompletionItem(lootItem.Details, RandomNumberGenerator.NumberBetween(1, lootItem.MaximumDrop)));
                    }
                }

                if (lootedItems.Count == 0)
                {
                    foreach (LootItem lootItem in _currentMonster.LootTable)
                    {
                        if (lootItem.IsDefaultItem)
                        {
                            lootedItems.Add(new QuestCompletionItem(lootItem.Details, 1));
                        }
                    }
                }

                if (lootedItems.Count != 0)
                {
                    AddItemsInventory(lootedItems);

                    foreach (QuestCompletionItem qci in lootedItems)
                    {
                        if (qci.Quantity == 1)
                        {
                            RaiseMessage("You loot 1 " + qci.Details.Name);
                        }
                        else
                        {
                            RaiseMessage("You loot " + qci.Quantity.ToString() + " " + qci.Details.NamePlural);
                        }
                    }
                }
                else
                {
                    RaiseMessage("You loot nothing");
                }

                RaiseMessage("");
                MoveTo(CurrentLocation);

            }
            else
            {
                int damageToPlayer = RandomNumberGenerator.NumberBetween(0, _currentMonster.MaximumHitPoints);

                RaiseMessage("The " + _currentMonster.Name + " did " + damageToPlayer.ToString() + " points of damage.");
                CurrentHitPoints -= damageToPlayer;

                if (CurrentHitPoints <= 0)
                {
                    RaiseMessage("The " + _currentMonster.Name + " killed you.");
                    MoveHome();
                }
            }
        }

        public void UsePotion(HealingPotion potion)
        {
            CurrentHitPoints += potion.AmountToHeal;

            if (CurrentHitPoints > MaximumHitPoints)
            {
                CurrentHitPoints = MaximumHitPoints;
            }

            RemoveItemFromInventory(potion, 1);

            RaiseMessage("You drink a " + potion.Name);

            int damageToPlayer = RandomNumberGenerator.NumberBetween(0, _currentMonster.MaximumHitPoints);

            RaiseMessage("The " + _currentMonster.Name + " did " + damageToPlayer.ToString() + " points of damage.");
            CurrentHitPoints -= damageToPlayer;

            if (CurrentHitPoints <= 0)
            {
                RaiseMessage("The " + _currentMonster.Name + " killed you.");
                MoveHome();
            }
        }

        private void MoveHome()
        {
            MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
        }

        public void MoveRefresh()
        {
            MoveTo(CurrentLocation);
        }

        public event EventHandler<MessageEventArgs> OnMessage;

        private void RaiseMessage(string message, bool addExtraNewLine = false)
        {
            if (OnMessage != null)
            {
                OnMessage(this, new MessageEventArgs(message, addExtraNewLine));
            }
        }

        public string ToXmlString()
        {
            XmlDocument playerData = new XmlDocument();

            XmlNode player = playerData.CreateElement("Player");
            playerData.AppendChild(player);


            XmlNode stats = playerData.CreateElement("Stats");
            player.AppendChild(stats);

            XmlNode currentHitPoints = playerData.CreateElement("CurrentHitPoints");
            currentHitPoints.AppendChild(playerData.CreateTextNode(this.CurrentHitPoints.ToString()));
            stats.AppendChild(currentHitPoints);

            XmlNode maximumHitPoints = playerData.CreateElement("MaximumHitPoints");
            maximumHitPoints.AppendChild(playerData.CreateTextNode(this.MaximumHitPoints.ToString()));
            stats.AppendChild(maximumHitPoints);

            XmlNode gold = playerData.CreateElement("Gold");
            gold.AppendChild(playerData.CreateTextNode(this.Gold.ToString()));
            stats.AppendChild(gold);

            XmlNode experiencePoints = playerData.CreateElement("ExperiencePoints");
            experiencePoints.AppendChild(playerData.CreateTextNode(this.ExperiencePoints.ToString()));
            stats.AppendChild(experiencePoints);

            XmlNode level = playerData.CreateElement("Level");
            level.AppendChild(playerData.CreateTextNode(this.Level.ToString()));
            stats.AppendChild(level);

            XmlNode currentLocation = playerData.CreateElement("CurrentLocation");
            currentLocation.AppendChild(playerData.CreateTextNode(this.CurrentLocation.ID.ToString()));
            stats.AppendChild(currentLocation);

            if(CurrentWeapon != null)
            {
                XmlNode currentWeapon = playerData.CreateElement("CurrentWeapon");
                currentWeapon.AppendChild(playerData.CreateTextNode(this.CurrentWeapon.ID.ToString()));
                stats.AppendChild(currentWeapon);
            }


            XmlNode inventoryItems = playerData.CreateElement("InventoryItems");
            player.AppendChild(inventoryItems);

            foreach(InventoryItem item in this.Inventory)
            {
                XmlNode inventoryItem = playerData.CreateElement("InventoryItem");

                XmlAttribute idAttribute = playerData.CreateAttribute("ID");
                idAttribute.Value = item.Details.ID.ToString();
                inventoryItem.Attributes.Append(idAttribute);

                XmlAttribute quantityAttribute = playerData.CreateAttribute("Quantity");
                quantityAttribute.Value = item.Quantity.ToString();
                inventoryItem.Attributes.Append(quantityAttribute);

                inventoryItems.AppendChild(inventoryItem);
            }

            XmlNode playerQuests = playerData.CreateElement("PlayerQuests");
            player.AppendChild(playerQuests);

            foreach(PlayerQuest quest in this.Quests)
            {
                XmlNode playerQuest = playerData.CreateElement("PlayerQuest");

                XmlAttribute idAttribute = playerData.CreateAttribute("ID");
                idAttribute.Value = quest.Details.ID.ToString();
                playerQuest.Attributes.Append(idAttribute);

                XmlAttribute isCompletedAttribute = playerData.CreateAttribute("IsCompleted");
                isCompletedAttribute.Value = quest.IsCompleted.ToString();
                playerQuest.Attributes.Append(isCompletedAttribute);

                playerQuests.AppendChild(playerQuest);
            }

            return playerData.InnerXml;
        }

        public static Player CreatePlayerFromDatabase(int currentHitPoints, int maximumHitPoints, int gold, int experiencePoints, int currentLocationID, int level, int currentWeapon)
        {
            Player player = new Player(currentHitPoints, maximumHitPoints, gold, experiencePoints);
            player.Level = level;
            //player.CurrentWeapon = (Weapon) World.ItemByID(currentWeapon);
            //player.MoveTo(World.LocationByID(currentLocationID));
            return player;
        }
    }
}
