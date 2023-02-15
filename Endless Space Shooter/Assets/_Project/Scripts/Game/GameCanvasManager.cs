using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Scoreboard;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace _Project.Scripts.Game
{
    public class GameCanvasManager : MonoBehaviour
    {
        [SerializeField] private GameObject gamePanel;
        [SerializeField] private GameObject scoreBoardPanel;
        
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text timeText;
        [SerializeField] private TMP_Text healthText;
        
        [SerializeField] private GameObject entityPrefab;
        [SerializeField] private TMP_InputField nameInputField;
        [SerializeField] private Button submitButton;
        [SerializeField] private Button backButton;
        [SerializeField] private ScrollRect scoreboardScrollView;
        [SerializeField] private TMP_Text finalScoreText;

        private readonly List<GameObject> _scoreboard = new();

        private float _time;
        
        private void Awake()
        {
            gamePanel.SetActive(true);
            scoreBoardPanel.SetActive(false);
        }

        private void Start()
        {
            GameManager.Instance.OnScoreChanged += (i => scoreText.text = i.ToString());
            GameManager.Instance.OnPlayerHealthChanged += (i => healthText.text = i.ToString());
            GameManager.Instance.OnDeath += () =>
            {
                gamePanel.SetActive(false);
                nameInputField.gameObject.SetActive(true);
                submitButton.gameObject.SetActive(true);
                backButton.gameObject.SetActive(false);
                finalScoreText.text = scoreText.text;
                scoreBoardPanel.SetActive(true);
                UpdateScoreboard();
            };
        }

        private void Update()
        {
            if (GameManager.Instance.GameState != GameState.Playing) 
                return;
            _time += Time.deltaTime;
            timeText.text = _time.ToString("F2");
        }
        
        public void SubmitScore()
        {
            if (nameInputField.text == "")
                return;
            
            ScoreboardManager.Instance.AddScore(nameInputField.text, GameManager.Instance.Score);
            nameInputField.text = "";
            nameInputField.gameObject.SetActive(false);
            submitButton.gameObject.SetActive(false);
            backButton.gameObject.SetActive(true);
            scoreboardScrollView.gameObject.SetActive(true);
            UpdateScoreboard();
        }
        
        public void BackToMenu()
        {
            SceneManager.LoadScene(0);
        }

        private void UpdateScoreboard()
        {
            if (_scoreboard.Any())
            {
                foreach (var entry in _scoreboard)
                {
                    Destroy(entry);
                }
                _scoreboard.Clear();
            }
            
            var list = ScoreboardManager.Instance.Scoreboard.scoreboardList;
            var rank = 1;
            for (var i = list.Count - 1; i >= 0; i--)
            {
                var entry = Instantiate(entityPrefab, scoreboardScrollView.content).GetComponent<Entry>();
                entry.Init(rank,list[i].playerName, list[i].score);
                rank++;
                
                _scoreboard.Add(entry.gameObject);
            }
        }
    }
}
