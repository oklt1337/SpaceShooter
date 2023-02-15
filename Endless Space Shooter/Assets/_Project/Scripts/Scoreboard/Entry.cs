using TMPro;
using UnityEngine;

namespace _Project.Scripts.Scoreboard
{
    public class Entry : MonoBehaviour
    {
        [SerializeField] private TMP_Text rankText;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text scoreText;

        public void Init(int rank, string playerName, int score)
        {
            rankText.text = rank + ".";
            scoreText.text = score.ToString();
            nameText.text = playerName;
        }
    }
}
