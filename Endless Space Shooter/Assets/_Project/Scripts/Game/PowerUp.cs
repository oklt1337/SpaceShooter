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
    
    public class PowerUp : MonoBehaviour
    {
        [SerializeField] private PowerUpType type = PowerUpType.Speed;
        [SerializeField] private float modifier = 1f;

        private PowerUpSpawner _powerUpSpawner;
        
        public void Init(PowerUpSpawner powerUpSpawner)
        {
            _powerUpSpawner = powerUpSpawner;
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
