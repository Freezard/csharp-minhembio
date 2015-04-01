using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace MinhembioStats
{
    [Serializable()]
    public class Game
    {
        private string id;
        private int nr;
        private string name;
        private string author;
        private SortedList<DateTime, int> visitors;

        public Game(string id, int nr, string name, string author, DateTime date, int visitors)
        {
            this.id = id;
            this.nr = nr;
            this.name = name;
            this.author = author;
            this.visitors = new SortedList<DateTime, int>();
            this.visitors.Add(date, visitors);
        }

        public int getNr()
        {
            return nr;
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

        public SortedList<DateTime, int> getVisitors()
        {
            return visitors;
        }
    }
}
