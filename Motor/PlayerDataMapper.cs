using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;

namespace Motor
{
    public static class PlayerDataMapper
    {
        private static readonly string _connectionString = "Server=(local)\\SQLEXPRESS;Database=RpgGame;Trusted_Connection=True;";
        
        public static Player CreateFromDataBase()
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    Player player;
                    int currentLocationID;
                    int currentWeapon;

                    using (SqlCommand savedGameCommand = connection.CreateCommand())
                    {
                        savedGameCommand.CommandType = CommandType.Text;
                        savedGameCommand.CommandText = "SELECT TOP 1 * FROM SavedGame";
                        SqlDataReader reader = savedGameCommand.ExecuteReader();
                        if (!reader.HasRows)
                        {
                            return null;
                        }

                        reader.Read();
                        int currentHitPoints = (int)reader["CurrentHitPoints"];
                        int maximumHitPoints = (int)reader["MaximumHitPoints"];
                        int gold = (int)reader["Gold"];
                        int experiencePoints = (int)reader["ExperiencePoints"];
                        int level = (int)reader["CurrentLevel"];
                        currentWeapon = (int)reader["CurrentWeapon"];
                        currentLocationID = (int)reader["CurrentLocationID"];

                        player = Player.CreatePlayerFromDatabase(currentHitPoints, maximumHitPoints, gold, experiencePoints, currentLocationID, level, currentWeapon);
                        reader.Close();
                    }

                    using(SqlCommand questCommand = connection.CreateCommand())
                    {
                        questCommand.CommandType = CommandType.Text;
                        questCommand.CommandText = "SELECT * FROM Quest";
                        SqlDataReader reader = questCommand.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int questID = (int)reader["QuestID"];
                                bool isCompleted = (bool)reader["IsCompleted"];
                                
                                PlayerQuest playerQuest = new PlayerQuest(World.QuestByID(questID));
                                playerQuest.IsCompleted = isCompleted;
                                if (playerQuest.IsCompleted && playerQuest.Details.IDPosteriorQuest != 0)
                                {
                                    World.QuestByID(playerQuest.Details.IDPosteriorQuest).ConditionToStartQuest = true;
                                }
                                
                                player.Quests.Add(playerQuest);

                            }
                        }
                        reader.Close();
                    }

                    using(SqlCommand inventoryCommand = connection.CreateCommand())
                    {
                        inventoryCommand.CommandType = CommandType.Text;
                        inventoryCommand.CommandText = "SELECT * FROM Inventory";

                        SqlDataReader reader = inventoryCommand.ExecuteReader();

                        if (reader.HasRows)
                        {
                            List<QuestCompletionItem> items = new List<QuestCompletionItem>();

                            while (reader.Read())
                            {
                                int inventoryID = (int)reader["InventoryItemID"];
                                int quantity = (int)reader["Quantity"];

                                items.Add(new QuestCompletionItem(World.ItemByID(inventoryID), quantity));
                            }

                            player.AddItemsInventory(items);
                        }
                        reader.Close();
                    }
                    player.CurrentLocation = World.LocationByID(currentLocationID);
                    player.CurrentWeapon = (Weapon)World.ItemByID(currentWeapon);
                    return player;
                }
            }
            catch(Exception)
            {
                
            }

            return null;
        }
        
        public static void SaveToDataBase(Player player)
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // insert/update
                    using(SqlCommand existingRowCountCommand = connection.CreateCommand())
                    {
                        existingRowCountCommand.CommandType = CommandType.Text;
                        existingRowCountCommand.CommandText = "SELECT count(*) FROM SavedGame";

                        // for return one value
                        int existingRowCount = (int)existingRowCountCommand.ExecuteScalar();
 
                        if(existingRowCount == 0)
                        {
                            // insert
                            using(SqlCommand insertSavedGame = connection.CreateCommand())
                            {
                                insertSavedGame.CommandType = CommandType.Text;
                                insertSavedGame.CommandText = 
                                    "INSERT INTO SavedGame" +
                                    "(CurrentHitPoints, MaximumHitPoints, Gold, ExperiencePoints, CurrentLocationID, CurrentLevel, CurrentWeapon) " +
                                    "VALUES " +
                                    "(@CurrentHitPoints, @MaximumHitPoints, @Gold, @ExperiencePoints, @CurrentLocationID, @CurrentLevel, @CurrentWeapon)";
                                insertSavedGame.Parameters.Add("@CurrentHitPoints", SqlDbType.Int);
                                insertSavedGame.Parameters["@CurrentHitPoints"].Value = player.CurrentHitPoints;
                                insertSavedGame.Parameters.Add("@MaximumHitPoints", SqlDbType.Int);
                                insertSavedGame.Parameters["@MaximumHitPoints"].Value = player.MaximumHitPoints;
                                insertSavedGame.Parameters.Add("@Gold", SqlDbType.Int);
                                insertSavedGame.Parameters["@Gold"].Value = player.Gold;
                                insertSavedGame.Parameters.Add("@ExperiencePoints", SqlDbType.Int);
                                insertSavedGame.Parameters["@ExperiencePoints"].Value = player.ExperiencePoints;
                                insertSavedGame.Parameters.Add("@CurrentLocationID", SqlDbType.Int);
                                insertSavedGame.Parameters["@CurrentLocationID"].Value = player.CurrentLocation.ID;
                                insertSavedGame.Parameters.Add("@CurrentLevel", SqlDbType.Int);
                                insertSavedGame.Parameters["@CurrentLevel"].Value = player.Level;
                                insertSavedGame.Parameters.Add("@CurrentWeapon", SqlDbType.Int);
                                insertSavedGame.Parameters["@CurrentWeapon"].Value = player.CurrentWeapon;

                                // does not return any results
                                insertSavedGame.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            // update
                            using(SqlCommand updateSavedGame = connection.CreateCommand())
                            {
                                updateSavedGame.CommandType = CommandType.Text;
                                updateSavedGame.CommandText = 
                                    "UPDATE SavedGame " +
                                    "SET CurrentHitPoints = @CurrentHitPoints, " +
                                    "MaximumHitPoints = @MaximumHitPoints, " +
                                    "Gold = @Gold, " +
                                    "ExperiencePoints = @ExperiencePoints, " +
                                    "CurrentLocationID = @CurrentLocationID, " +
                                    "CurrentLevel = @CurrentLevel, " +
                                    "CurrentWeapon = @CurrentWeapon";
                                updateSavedGame.Parameters.Add("@CurrentHitPoints", SqlDbType.Int);
                                updateSavedGame.Parameters["@CurrentHitPoints"].Value = player.CurrentHitPoints;
                                updateSavedGame.Parameters.Add("@MaximumHitPoints", SqlDbType.Int);
                                updateSavedGame.Parameters["@MaximumHitPoints"].Value = player.MaximumHitPoints;
                                updateSavedGame.Parameters.Add("@Gold", SqlDbType.Int);
                                updateSavedGame.Parameters["@Gold"].Value = player.Gold;
                                updateSavedGame.Parameters.Add("@ExperiencePoints", SqlDbType.Int);
                                updateSavedGame.Parameters["@ExperiencePoints"].Value = player.ExperiencePoints;
                                updateSavedGame.Parameters.Add("@CurrentLocationID", SqlDbType.Int);
                                updateSavedGame.Parameters["@CurrentLocationID"].Value = player.CurrentLocation.ID;
                                updateSavedGame.Parameters.Add("@CurrentLevel", SqlDbType.Int);
                                updateSavedGame.Parameters["@CurrentLevel"].Value = player.Level;
                                updateSavedGame.Parameters.Add("@CurrentWeapon", SqlDbType.Int);
                                updateSavedGame.Parameters["@CurrentWeapon"].Value = player.CurrentWeapon.ID;

                                updateSavedGame.ExecuteNonQuery();

                            }
                        }                      
                    }

                    using(SqlCommand deleteQuestsCommand = connection.CreateCommand())
                    {
                        deleteQuestsCommand.CommandType = CommandType.Text;
                        deleteQuestsCommand.CommandText = "DELETE FROM Quest";

                        deleteQuestsCommand.ExecuteNonQuery();
                    }

                    foreach (PlayerQuest playerQuest in player.Quests)
                    {
                        using (SqlCommand insertQuestCommand = connection.CreateCommand())
                        {
                            insertQuestCommand.CommandType = CommandType.Text;
                            insertQuestCommand.CommandText =
                                "INSERT INTO Quest (QuestID, IsCompleted)" +
                                "VALUES (@QuestID, @IsCompleted)";

                            insertQuestCommand.Parameters.Add("@QuestID", SqlDbType.Int);
                            insertQuestCommand.Parameters["@QuestID"].Value = playerQuest.Details.ID;
                            insertQuestCommand.Parameters.Add("@IsCompleted", SqlDbType.Bit);
                            insertQuestCommand.Parameters["@IsCompleted"].Value = playerQuest.IsCompleted;
                            
                            insertQuestCommand.ExecuteNonQuery();
                        }
                    }

                    using(SqlCommand deleteInventoryCommand = connection.CreateCommand())
                    {
                        deleteInventoryCommand.CommandType = CommandType.Text;
                        deleteInventoryCommand.CommandText = "DELETE FROM Inventory";

                        deleteInventoryCommand.ExecuteNonQuery();
                    }

                    foreach(InventoryItem inventoryItem in player.Inventory)
                    {
                        using(SqlCommand insertInventoryCommand = connection.CreateCommand())
                        {
                            insertInventoryCommand.CommandType = CommandType.Text;
                            insertInventoryCommand.CommandText =
                                "INSERT INTO Inventory (InventoryItemID, Quantity)" +
                                "VALUES (@InventoryItemID, @Quantity)";

                            insertInventoryCommand.Parameters.Add("@InventoryItemID", SqlDbType.Int);
                            insertInventoryCommand.Parameters["@InventoryItemID"].Value = inventoryItem.Details.ID;
                            insertInventoryCommand.Parameters.Add("@Quantity", SqlDbType.Int);
                            insertInventoryCommand.Parameters["@Quantity"].Value = inventoryItem.Quantity;

                            insertInventoryCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch(Exception)
            {

            }
        }
    }
}
