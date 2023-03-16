using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Game
{
    public enum PowerUpType
    {
        Speed,
        Health,
        Damage,
        BulletSpeed
    }

    public enum Rarity
    {
        Bronze,
        Silver,
        Gold
    }
    
    public class PowerUp : MonoBehaviour
    {
        [SerializeField] private PowerUpType type = PowerUpType.Speed;
        [SerializeField] private List<Sprite> sprites = new();
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private float modifier = 1f;

        private PowerUpSpawner _powerUpSpawner;

        private Rarity Rarity { get; set; }

        public void Init(PowerUpSpawner powerUpSpawner, Rarity rarity)
        {
            _powerUpSpawner = powerUpSpawner;
            Rarity = rarity;

            switch (Rarity)
            {
                case Rarity.Bronze:
                    spriteRenderer.sprite = sprites[0];
                    modifier *= 1.0f;
                    break;
                case Rarity.Silver:
                    spriteRenderer.sprite = sprites[1];
                    modifier *= 1.5f;
                    break;
                case Rarity.Gold:
                    spriteRenderer.sprite = sprites[2];
                    modifier *= 2.0f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) 
                return;
            
            other.GetComponent<Player>().AddPowerUp(type, modifier);
            _powerUpSpawner.RemovePowerUp(this);
        }
    }
}
