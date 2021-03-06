using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motor
{
    public class LootItem
    {
        public Item Details { get; set; }
        public int DropPercentage { get; set; }
        public bool IsDefaultItem { get; set; }
        public int MaximumDrop { get; set; }

        public LootItem(Item details, int dropPercentage, bool isDefaultItem, int maximumDrop)
        {
            Details = details;
            DropPercentage = dropPercentage;
            IsDefaultItem = isDefaultItem;
            MaximumDrop = maximumDrop;
        }
    }
}
