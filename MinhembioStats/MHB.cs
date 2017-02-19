using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace MinhembioStats
{
    [Serializable()]
    public class MHB
    {
        private SortedDictionary<int, Review> reviews;
        private string lastUpdated;

        public MHB()
        {
            reviews = new SortedDictionary<int, Review>(Comparer<int>.Create((x, y) => y.CompareTo(x)));
        }

        public void addReview(int id, string name, string author, DateTime date, int visitors)
        {
            reviews.Add(id, new Review(id, name, author, date, visitors));
        }

        public void removeReview(int id)
        {
            reviews.Remove(id);
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
