using System;
using System.Collections.Generic;

namespace EmployeeApp
{
    public class EmployeePromotion
    {
        private List<string> promotionList = new List<string>();

        // Add employee to promotion list
        public void AddToPromotionList(string name)
        {
            promotionList.Add(name);
        }

        // Display current promotion list
        public void DisplayPromotionList()
        {
            Console.WriteLine("\nPromotion List:");
            foreach (string name in promotionList)
            {
                Console.WriteLine(name);
            }
        }

        // Find promotion position by name (1-based)
        public int GetPromotionPosition(string name)
        {
            int index = promotionList.IndexOf(name);
            return index >= 0 ? index + 1 : -1;
        }

        // Get capacity of the underlying list
        public int GetCapacity()
        {
            return promotionList.Capacity;
        }

        // Trim excess memory in list
        public void TrimList()
        {
            promotionList.TrimExcess();
        }

        // Return a sorted copy of the promotion list
        public List<string> GetSortedPromotionList()
        {
            List<string> sortedList = new List<string>(promotionList);
            sortedList.Sort();
            return sortedList;
        }

        // Get count of items in promotion list
        public int Count()
        {
            return promotionList.Count;
        }
    }
}
