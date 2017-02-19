using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace MinhembioStats
{
    [Serializable()]
    public class Review
    {
        private int id;
        private string name;
        private string author;
        private SortedList<DateTime, int> visitors;

        public Review(int id, string name, string author, DateTime date, int visitors)
        {
            this.id = id;
            this.name = name;
            this.author = author;
            this.visitors = new SortedList<DateTime, int>();
            this.visitors.Add(date, visitors);
        }

        public int getId()
        {
            return id;
        }

        public string getName()
        {
            return name;
        }

        public string getAuthor()
        {
            return author;
        }

        public void addVisitors(DateTime date, int visitors)
        {
            this.visitors.Add(date, visitors);
        }

        public void removeVisitors(DateTime date)
        {
            this.visitors.Remove(date);
        }

        public SortedList<DateTime, int> getVisitors()
        {
            return visitors;
        }
    }
}
