using System;
using System.Collections.Generic;
using System.Linq;

namespace _Project.Scripts.Scoreboard
{
    [Serializable]
    public struct ScoreboardEntry
    {
        public string playerName;
        public int score;
        
        public ScoreboardEntry(string playerName, int score)
        {
            this.playerName = playerName;
            this.score = score;
        }
    }
    
    [Serializable]
    public class Scoreboard
    {
        public Scoreboard()
        {
            scoreboardList = new List<ScoreboardEntry>();
        }
        
        public List<ScoreboardEntry> scoreboardList;

        public bool HasScores()
        {
            return scoreboardList.Any();
        }
        
        public void AddScore(string playerName, int score)
        { 
            scoreboardList.Add(new ScoreboardEntry(playerName, score)); 
            scoreboardList.Sort((pair1,pair2) => pair1.score.CompareTo(pair2.score));
        }
    }
}
