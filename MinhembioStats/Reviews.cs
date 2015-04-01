using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Collections;

namespace MinhembioStats
{
    [Serializable()]
    public class Reviews
    {
        private Dictionary<string, Game> games;
        private int nrReviews;
        private string lastUpdated;

        public Reviews()
        {
            games = new Dictionary<string, Game>();
            nrReviews = 1;
        }
      
        public void addGame(string id, string name, string author, DateTime date, int visitors)
        {
            games.Add(id, new Game(id, nrReviews++, name, author, date, visitors));
        }

        public void addInformation(string id, DateTime date, int visitors)
        {
            getGame(id).addVisitors(date, visitors);
        }

        public void removeGame(string id)
        {
            games.Remove(id);
            nrReviews--;
        }

        public bool containsGame(string id)
        {
            if (games.ContainsKey(id))
                return true;
            else return false;
        }

        public Game getGame(string id)
        {
            return games[id];
        }

        public Dictionary<string, Game> getAllGames()
        {
            return games;
        }

        public string getLastUpdated()
        {
            return lastUpdated;
        }

        public void setLastUpdated(string lastUpdated)
        {
            this.lastUpdated = lastUpdated;
        }
    }
}
