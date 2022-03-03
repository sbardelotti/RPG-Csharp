using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motor
{
    public class Quest
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int RewardExperiencePoints { get; set; }
        public int RewardGold { get; set; }
        public int IDPosteriorQuest { get; set; }
        public bool ConditionToStartQuest { get; set; }
        public List<QuestCompletionItem> RewardItem { get; set; }
        public List<QuestCompletionItem> QuestCompletionItems { get; set; }

        public Quest(int id, string name, string description, int rewardExperiencePoints, int rewardGold, int idPosteriorQuest = 0, bool conditionToStartQuest = true)
        {
            ID = id;
            Name = name;
            Description = description;
            RewardExperiencePoints = rewardExperiencePoints;
            RewardGold = rewardGold;
            IDPosteriorQuest = idPosteriorQuest;
            ConditionToStartQuest = conditionToStartQuest;

            QuestCompletionItems = new List<QuestCompletionItem>();
            RewardItem = new List<QuestCompletionItem>();
        }
    }
}
