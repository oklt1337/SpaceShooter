using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Game
{
    public class PowerUpSpawner : MonoBehaviour
    {
        [SerializeField] private List<GameObject> powerUpPrefabs = new();
        private readonly List<PowerUp> _powerUps = new();
        private Camera _camera;
        private float _timer;

        public void RemovePowerUp(PowerUp powerUp)
        {
            if (!_powerUps.Contains(powerUp))
                return;

            _powerUps.Remove(powerUp);
            Destroy(powerUp.gameObject);
        }

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Start()
        {
            SetTimer();
        }

        private void Update()
        {
            switch (GameManager.Instance.GameState)
            {
                case GameState.Playing:
                    SpawnPowerUps();
                    break;
                case GameState.GameOver:
                    DestroyPowerUps();
                    break;
                case GameState.Start:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SpawnPowerUps()
        {
            if (_timer <= 0f)
            {
                SetTimer();
                var powerUp = Random.Range(0, powerUpPrefabs.Count - 1);
                Vector2 randomPositionOnScreen =
                    _camera.ViewportToWorldPoint(new Vector2(Random.value, Random.value));
                var power = Instantiate(powerUpPrefabs[powerUp], randomPositionOnScreen, Quaternion.identity)
                    .GetComponent<PowerUp>();

                
                var rarityChance = Random.Range(0, 100);
                var rarity = rarityChance switch
                {
                    < 3 => Rarity.Gold,
                    >= 3 and < 30 => Rarity.Silver,
                    _ => Rarity.Bronze
                };
                power.Init(this, rarity);
                _powerUps.Add(power);
            }
            else
            {
                _timer -= Time.deltaTime;
            }
        }

        private void SetTimer()
        {
            _timer = Random.Range(2, 10);
        }

        private void DestroyPowerUps()
        {
            if (_powerUps.Count == 0)
                return;
            for (var i = _powerUps.Count - 1; i >= 0; i--)
            {
                Destroy(_powerUps[i].gameObject);
                _powerUps.Remove(_powerUps.Last());
            }
        }
    }
}