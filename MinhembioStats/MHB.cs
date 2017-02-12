using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace MinhembioStats
{
    [Serializable()]
    public class MHB
    {
        private SortedDictionary<string, Review> reviews;
        private string lastUpdated;

        public MHB()
        {
            reviews = new SortedDictionary<string, Review>(Comparer<string>.Create((x, y) => y.CompareTo(x)));
        }
      
        public void addReview(string id, string name, string author, DateTime date, int visitors)
        {
            reviews.Add(id, new Review(id, name, author, date, visitors));
        }

        public void removeReview(string id)
        {
            reviews.Remove(id);
        }

        public bool containsReview(string id)
        {
            return reviews.ContainsKey(id);
        }

        public Review getReview(string id)
        {
            return reviews[id];
        }

        public SortedDictionary<string, Review>.ValueCollection getAllReviews()
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
