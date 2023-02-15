using _Project.Scripts.Serialization;
using UnityEngine;

namespace _Project.Scripts.Scoreboard
{
    public class ScoreboardManager : MonoBehaviour
    {
        public static ScoreboardManager Instance { get; private set; }

        public Scoreboard Scoreboard { get; private set; } = new();

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else 
                Destroy(gameObject);

            GetScoreboard();
        }

        public void AddScore(string playerName, int score)
        {
            Scoreboard.AddScore(playerName, score);
            SaveScoreboard();
        }
        
        private void GetScoreboard()
        {
            Scoreboard = Deserializer.Deserialize();
        }
        
        private void SaveScoreboard()
        {
            Serializer.Serialize(Scoreboard);
        }
    }
}
