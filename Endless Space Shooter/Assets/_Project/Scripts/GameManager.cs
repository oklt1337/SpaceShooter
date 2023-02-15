using System;
using System.Collections.Generic;
using _Project.Scripts.Game;
using UnityEngine;

namespace _Project.Scripts
{
    public enum GameState
    {
        Start,
        Playing,
        GameOver
    }
    
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        [SerializeField] private GameState gameState = GameState.Start;
        
        [Header("Player")]
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Transform playerSpawnPosition;
        
        [Header("Enemies")]
        [SerializeField] private float enemySpawnDelay = 1f;
        [SerializeField] private List<GameObject> enemyPrefabs;
        [SerializeField] private List<Transform> enemySpawnPositions;

        private Player _player;
        private float _enemySpawnTimer;

        private int _score;
        private float _lastUpdate;
        
        public GameState GameState => gameState;

        public int Score
        {
            get => _score;
            private set
            {
                _score = value;
                OnScoreChanged?.Invoke(_score);
            } 
        }
        
        public event Action<int> OnScoreChanged;
        public Action<int> OnPlayerHealthChanged;
        public event Action OnDeath;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else 
                Destroy(gameObject);
        }

        private void Start()
        {
            if (gameState != GameState.Start)
                return;

            if (playerPrefab == null)
                throw new ArgumentNullException(nameof(playerPrefab));

            _player = Instantiate(playerPrefab, playerSpawnPosition.position, Quaternion.identity).GetComponent<Player>();
            gameState = GameState.Playing;
        }

        private void Update()
        {
            if (gameState != GameState.Playing)
                return;
            
            if (Time.time - _lastUpdate >= 1f)
            {
                Score++;
                _lastUpdate = Time.time;
            }

            _enemySpawnTimer += Time.deltaTime;
            if (!(_enemySpawnTimer >= enemySpawnDelay)) 
                return;
            _enemySpawnTimer = 0f;
            SpawnEnemy();
        }
        
        

        private void SpawnEnemy()
        {
            var randomPosition = enemySpawnPositions[UnityEngine.Random.Range(0, enemySpawnPositions.Count)];
            var randomEnemy = enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Count)];
            var enemy = Instantiate(randomEnemy, randomPosition.position, Quaternion.identity).GetComponent<Enemy>();
            enemy.Init(_player.transform);
        }

        public void Death()
        {
            Destroy(_player.gameObject);
            gameState = GameState.GameOver;
            OnDeath?.Invoke();
        }

        public void IncreaseScore()
        {
            Score++;
        }
    }
}
