using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motor
{
    public static class World
    {
        //initializing lists
        public static readonly List<Item> Items = new List<Item>();
        public static readonly List<Monster> Monsters = new List<Monster>();
        public static readonly List<Quest> Quests = new List<Quest>();
        public static readonly List<Location> Locations = new List<Location>();
        //declaring items
        public const int ITEM_ID_RUSTY_SWORD = 1;
        public const int ITEM_ID_RAT_TAIL = 2;
        public const int ITEM_ID_PIECE_OF_FUR = 3;
        public const int ITEM_ID_SNAKE_FANG = 4;
        public const int ITEM_ID_SNAKE_SKIN = 5;
        public const int ITEM_ID_CLUB = 6;
        public const int ITEM_ID_HEALING_POTION = 7;
        public const int ITEM_ID_SPIDER_FANG = 8;
        public const int ITEM_ID_SPIDER_SILK = 9;
        public const int ITEM_ID_ADVENTURER_PASS = 10;
        public const int ITEM_ID_WOLF_TOOTH = 11;
        public const int ITEM_ID_WOLF_CLAW = 12;
        public const int ITEM_ID_DAGGER = 13;
        public const int ITEM_ID_BEAR_SKIN = 14;
        public const int ITEM_ID_PANTHER_SKIN = 15;
        public const int ITEM_ID_IRON_SWORD = 16;
        public const int ITEM_ID_GIGANT_SPIDER_FANG = 17;
        public const int ITEM_ID_SLIME_GEL = 18;
        //declaring monsters
        public const int MONSTER_ID_RAT = 1;
        public const int MONSTER_ID_SNAKE = 2;
        public const int MONSTER_ID_GIGANT_SPIDER = 3;
        public const int MONSTER_ID_SLIME = 4;
        public const int MONSTER_ID_WOLF = 5;
        public const int MONSTER_ID_BANDIT = 6;
        public const int MONSTER_ID_BEAR = 7;
        public const int MONSTER_ID_PANTHER = 8;
        public const int MONSTER_ID_QUEEN_SPIDER = 9;
        //declaring quests
        public const int QUEST_ID_CLEAR_ALCHEMIST_GARDEN = 1;
        public const int QUEST_ID_CLEAR_FARMERS_FIELD = 2;
        public const int QUEST_ID_CLEAR_FOREST = 3;
        //declaring locations
        public const int LOCATION_ID_HOME = 1;
        public const int LOCATION_ID_TOWN_SQUARE = 2;
        public const int LOCATION_ID_GUARD_POST = 3;
        public const int LOCATION_ID_ALCHEMIST_HUT = 4;
        public const int LOCATION_ID_ALCHEMIST_GARDEN = 5;
        public const int LOCATION_ID_FARMHOUSE = 6;
        public const int LOCATION_ID_FARM_FIELD = 7;
        public const int LOCATION_ID_BRIDGE = 8;
        public const int LOCATION_ID_SPIDER_FIELD = 9;
        public const int LOCATION_ID_WOOD = 10;
        public const int LOCATION_ID_INSIDE_WOOD = 11;
        public const int LOCATION_ID_CLEARING = 12;
        public const int LOCATION_ID_CAVE = 13;
        public const int LOCATION_ID_INSIDE_CAVE = 14;
        public const int LOCATION_ID_STREAM = 15;
        public const int LOCATION_ID_FOREST1 = 16;
        public const int LOCATION_ID_FOREST2 = 17;
        public const int LOCATION_ID_FOREST3 = 18;

        static World()
        {
            PopulateItems();
            PopulateMonsters();
            PopulateQuests();
            PopulateLocations();
        }

        private static void PopulateItems()
        {
            //adding declared items to list
            Items.Add(new Weapon(ITEM_ID_RUSTY_SWORD, "Rusty sword", "Rusty swords", 0, 5));
            Items.Add(new Item(ITEM_ID_RAT_TAIL, "Rat tail", "Rat tails"));
            Items.Add(new Item(ITEM_ID_PIECE_OF_FUR, "Piece of fur", "Pieces of fur"));
            Items.Add(new Item(ITEM_ID_SNAKE_FANG, "Snake fang", "Snake fangs"));
            Items.Add(new Item(ITEM_ID_SNAKE_SKIN, "Snake skin", "Snake skins"));
            Items.Add(new Weapon(ITEM_ID_CLUB, "Club", "Clubs", 3, 10));
            Items.Add(new HealingPotion(ITEM_ID_HEALING_POTION, "Healing potion", "Healing potions", 5));
            Items.Add(new Item(ITEM_ID_SPIDER_FANG, "Spider fang", "Spider fangs"));
            Items.Add(new Item(ITEM_ID_SPIDER_SILK, "Spider silk", "Spider silks"));
            Items.Add(new Item(ITEM_ID_ADVENTURER_PASS, "Adventurer pass", "Adventurer passes"));
            Items.Add(new Item(ITEM_ID_WOLF_TOOTH, "Wolf tooth", "Wolf tooths"));
            Items.Add(new Item(ITEM_ID_WOLF_CLAW, "Wolf claw", "Wolf claws"));
            Items.Add(new Weapon(ITEM_ID_DAGGER, "Dagger", "Daggers", 5, 12));
            Items.Add(new Item(ITEM_ID_BEAR_SKIN, "Bear skin", "Bear skins"));
            Items.Add(new Item(ITEM_ID_PANTHER_SKIN, "Panther skin", "Panther skins"));
            Items.Add(new Weapon(ITEM_ID_IRON_SWORD, "Iron sword", "Iron swords", 10, 15));
            Items.Add(new Item(ITEM_ID_GIGANT_SPIDER_FANG, "Gigant spider fang", "Gigant spider fangs"));
            Items.Add(new Item(ITEM_ID_SLIME_GEL, "Slime gel", "Slime gels"));
        }

        private static void PopulateMonsters()
        {
            //creating monsters
            Monster rat = new Monster(MONSTER_ID_RAT, "Rat", 5, 3, 10, 3, 3);
            rat.LootTable.Add(new LootItem(ItemByID(ITEM_ID_RAT_TAIL), 75, false, 2));
            rat.LootTable.Add(new LootItem(ItemByID(ITEM_ID_PIECE_OF_FUR), 75, true, 3));

            Monster snake = new Monster(MONSTER_ID_SNAKE, "Snake", 5, 3, 10, 3, 3);
            snake.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SNAKE_FANG), 75, false, 2));
            snake.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SNAKE_SKIN), 75, true, 1));

            Monster gigantSpider = new Monster(MONSTER_ID_GIGANT_SPIDER, "Gigant spider", 10, 5, 10, 10, 10);
            gigantSpider.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SPIDER_FANG), 75, true, 2));
            gigantSpider.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SPIDER_SILK), 25, false, 5));

            Monster slime = new Monster(MONSTER_ID_SLIME, "Slime", 2, 1, 0, 3, 3);
            slime.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SLIME_GEL), 50, false, 3));

            Monster wolf = new Monster(MONSTER_ID_WOLF, "Wolf", 8, 15, 5, 8, 8);
            wolf.LootTable.Add(new LootItem(ItemByID(ITEM_ID_WOLF_TOOTH), 50, false, 2));
            wolf.LootTable.Add(new LootItem(ItemByID(ITEM_ID_WOLF_CLAW), 75, true, 4));

            Monster bandit = new Monster(MONSTER_ID_BANDIT, "Bandit", 12, 15, 20, 14, 14);
            bandit.LootTable.Add(new LootItem(ItemByID(ITEM_ID_DAGGER), 20, false, 1));

            Monster bear = new Monster(MONSTER_ID_BEAR, "Bear", 8, 13, 5, 20, 20);
            bear.LootTable.Add(new LootItem(ItemByID(ITEM_ID_BEAR_SKIN), 75, true, 3));

            Monster panther = new Monster(MONSTER_ID_PANTHER, "Panther", 15, 18, 10, 8, 8);
            panther.LootTable.Add(new LootItem(ItemByID(ITEM_ID_PANTHER_SKIN), 75, true, 2));

            Monster queenSpider = new Monster(MONSTER_ID_QUEEN_SPIDER, "Queen spider", 20, 40, 35, 25, 25);
            queenSpider.LootTable.Add(new LootItem(ItemByID(ITEM_ID_GIGANT_SPIDER_FANG), 75, false, 2));
            //adding monsters to list
            Monsters.Add(rat);
            Monsters.Add(snake);
            Monsters.Add(gigantSpider);
            Monsters.Add(slime);
            Monsters.Add(wolf);
            Monsters.Add(bandit);
            Monsters.Add(bear);
            Monsters.Add(panther);
            Monsters.Add(queenSpider);
        }

        private static void PopulateQuests()
        {
            //creating quests
            Quest clearAlchemistGarden = new Quest(QUEST_ID_CLEAR_ALCHEMIST_GARDEN, "Clear alchemist's garden", "Kill rats in the alchemist's garden and bring back 3 rat tails and 5 pieces of fur. You will receive 2 healing potion, 1 club and 10 gold pieces.", 20, 10);
            clearAlchemistGarden.QuestCompletionItems.Add(new QuestCompletionItem(ItemByID(ITEM_ID_RAT_TAIL), 3));
            clearAlchemistGarden.QuestCompletionItems.Add(new QuestCompletionItem(ItemByID(ITEM_ID_PIECE_OF_FUR), 5));
            clearAlchemistGarden.RewardItem.Add(new QuestCompletionItem(ItemByID(ITEM_ID_HEALING_POTION), 2));
            clearAlchemistGarden.RewardItem.Add(new QuestCompletionItem(ItemByID(ITEM_ID_CLUB), 1));

            Quest clearFarmersFild = new Quest(QUEST_ID_CLEAR_FARMERS_FIELD, "Clear farmer's fild", "Kill snakes in the farmer's field and bring back 3 snake fangs. You will receive an adventurer's pass an 20 gold piecies.", 20, 20, QUEST_ID_CLEAR_FOREST);
            clearFarmersFild.QuestCompletionItems.Add(new QuestCompletionItem(ItemByID(ITEM_ID_SNAKE_FANG), 3));
            clearFarmersFild.RewardItem.Add(new QuestCompletionItem(ItemByID(ITEM_ID_ADVENTURER_PASS), 1));

            Quest clearForest = new Quest(QUEST_ID_CLEAR_FOREST, "Clear Forest", "Kill spiders in the forest and bring back 2 silks and 1 spider fang. You will receive 50 gold piecies", 60, 50, 0, false);
            clearForest.QuestCompletionItems.Add(new QuestCompletionItem(ItemByID(ITEM_ID_SPIDER_SILK), 2));
            clearForest.QuestCompletionItems.Add(new QuestCompletionItem(ItemByID(ITEM_ID_SPIDER_FANG), 2));

            //adding quests to list
            Quests.Add(clearAlchemistGarden);
            Quests.Add(clearFarmersFild);
            Quests.Add(clearForest);
        }

        private static void PopulateLocations()
        {
            //create each location
            Location home = new Location(LOCATION_ID_HOME, "Home", "Your house.");

            Location stream = new Location(LOCATION_ID_STREAM, "Stream", "A small stream.");

            Location woods = new Location(LOCATION_ID_WOOD, "Wood", "Tree and bushes, different shades of green fill your field of vision.");

            Location insideWoods = new Location(LOCATION_ID_INSIDE_WOOD, "Inside the woods", "You see traces of battle around the area.");

            Location clearing = new Location(LOCATION_ID_CLEARING, "Clearing", "An open field among the trees.");

            Location cave = new Location(LOCATION_ID_CAVE, "Cave", "You see the entrance to a cave between some bushes.");

            Location insideCave = new Location(LOCATION_ID_INSIDE_CAVE, "Inside cave", "The environment is stuffy and with little light, but it is still possible to see with some difficulty.");

            Location townSquare = new Location(LOCATION_ID_TOWN_SQUARE, "Town square", "You see a fountain");
            townSquare.QuestAvaibleHere.Add(QuestByID(QUEST_ID_CLEAR_FOREST));

            Location alchemistHut = new Location(LOCATION_ID_ALCHEMIST_HUT, "Alchemist's hut", "There are many strange plants on the shelves.");
            alchemistHut.QuestAvaibleHere.Add(QuestByID(QUEST_ID_CLEAR_ALCHEMIST_GARDEN));

            Location alchemistGarden = new Location(LOCATION_ID_ALCHEMIST_GARDEN, "Alchemist's garden", "Many plants are growing here.");
            alchemistGarden.MonsterLivingHere = MonsterByID(MONSTER_ID_RAT);

            Location farmhouse = new Location(LOCATION_ID_FARMHOUSE, "Farmhouse", "There is a small farmhouse, with a farmer in front.");
            farmhouse.QuestAvaibleHere.Add(QuestByID(QUEST_ID_CLEAR_FARMERS_FIELD));

            Location farmersField = new Location(LOCATION_ID_FARM_FIELD, "Farm's field", "You see a row of vegetables growing here.");
            farmersField.MonsterLivingHere = MonsterByID(MONSTER_ID_SNAKE);

            Location guardPost = new Location(LOCATION_ID_GUARD_POST, "Guard post", "There is a large, tough-looking guard here.", ItemByID(ITEM_ID_ADVENTURER_PASS));

            Location bridge = new Location(LOCATION_ID_BRIDGE, "Bridge", "A stone bridge crosses a wide river.");

            Location spiderField = new Location(LOCATION_ID_SPIDER_FIELD, "Forest", "You see spider webs covering the trees in this florest.");
            spiderField.MonsterLivingHere = MonsterByID(MONSTER_ID_GIGANT_SPIDER);

            Location forest1 = new Location(LOCATION_ID_FOREST1, "Forest", "The treetops block most of the sunlight.");

            Location forest2 = new Location(LOCATION_ID_FOREST2, "Forest", "Movement is difficult due to the amount of vegetation. You fell watched.");

            Location forest3 = new Location(LOCATION_ID_FOREST3, "Forest", "You see large web cocoons in human form.");
            //link locations
            home.LocationToNorth = townSquare;
            home.LocationToEast = stream;
            home.LocationToWest = woods;

            stream.LocationToWest = home;

            woods.LocationToEast = home;
            woods.LocationToWest = insideWoods;

            insideWoods.LocationToEast = woods;
            insideWoods.LocationToWest = clearing;
            insideWoods.LocationToSouth = cave;

            cave.LocationToNorth = insideWoods;
            cave.LocationToSouth = insideCave;

            insideCave.LocationToNorth = cave;

            townSquare.LocationToNorth = alchemistHut;
            townSquare.LocationToSouth = home;
            townSquare.LocationToEast = guardPost;
            townSquare.LocationToWest = farmhouse;

            farmhouse.LocationToEast = townSquare;
            farmhouse.LocationToWest = farmersField;

            farmersField.LocationToEast = farmhouse;

            alchemistHut.LocationToSouth = townSquare;
            alchemistHut.LocationToNorth = alchemistGarden;

            alchemistGarden.LocationToSouth = alchemistHut;

            guardPost.LocationToEast = bridge;
            guardPost.LocationToWest = townSquare;

            bridge.LocationToWest = guardPost;
            bridge.LocationToEast = spiderField;

            spiderField.LocationToWest = bridge;
            spiderField.LocationToNorth = forest1;

            forest1.LocationToSouth = spiderField;
            forest1.LocationToNorth = forest2;

            forest2.LocationToSouth = forest1;
            forest2.LocationToNorth = forest3;

            forest3.LocationToSouth = forest2;
            //adding location to list
            Locations.Add(home);
            Locations.Add(townSquare);
            Locations.Add(guardPost);
            Locations.Add(alchemistHut);
            Locations.Add(alchemistGarden);
            Locations.Add(farmhouse);
            Locations.Add(farmersField);
            Locations.Add(bridge);
            Locations.Add(spiderField);
            Locations.Add(stream);
            Locations.Add(woods);
            Locations.Add(insideWoods);
            Locations.Add(clearing);
            Locations.Add(cave);
            Locations.Add(insideCave);
            Locations.Add(forest1);
            Locations.Add(forest2);
            Locations.Add(forest3);
        }

        //takes the id and returns an item object
        public static Item ItemByID(int id)
        {
            foreach(Item item in Items)
            {
                if(item.ID == id)
                {
                    return item;
                }
            }
            return null;
        }

        public static Monster MonsterByID(int id)
        {
            foreach(Monster monster in Monsters)
            {
                if(monster.ID == id)
                {
                    return monster;
                }
            }
            return null;
        }

        public static Quest QuestByID(int id)
        {
            foreach(Quest quest in Quests)
            {
                if(quest.ID == id)
                {
                    return quest;
                }
            }
            return null;
        }

        public static Location LocationByID(int id)
        {
            foreach(Location location in Locations)
            {
                if(location.ID == id)
                {
                    return location;
                }
            }
            return null;
        }
    }
}
