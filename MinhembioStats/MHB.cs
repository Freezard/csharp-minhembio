using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace MinhembioStats
{
    [Serializable()]
    public class MHB
    {
        private SortedDictionary<int, Review> reviews;
        private string lastUpdated;
        private List<DateTime> updates;

        public MHB()
        {
            reviews = new SortedDictionary<int, Review>(Comparer<int>.Create((x, y) => y.CompareTo(x)));
            updates = new List<DateTime>();
        }

        public void addReview(int id, string name, string author, DateTime date, int visitors)
        {
            reviews.Add(id, new Review(id, name, author, date, visitors));

            if (!updates.Contains(date))
            {
                updates.Add(date);
                updates.Sort((a, b) => a.CompareTo(b));
            }
        }

        public void removeReview(int id)
        {
            reviews.Remove(id);
        }

        public void updateReview(int id, DateTime date, int visitors)
        {
            reviews[id].addVisitors(date, visitors);

            if (!updates.Contains(date))
            {
                updates.Add(date);
                updates.Sort((a, b) => a.CompareTo(b));
            }
        }

        public bool containsReview(int id)
        {
            return reviews.ContainsKey(id);
        }

        public Review getReview(int id)
        {
            return reviews[id];
        }

        public SortedDictionary<int, Review>.ValueCollection getAllReviews()
        {
            return reviews.Values;
        }

        public List<DateTime> getUpdates()
        {
            return updates;
        }

        public void removeAllReviews()
        {
            reviews.Clear();
        }

        public void setLastUpdated(string lastUpdated)
        {
            this.lastUpdated = lastUpdated;
        }

        public string getLastUpdated()
        {
            return lastUpdated;
        }
    }
}
