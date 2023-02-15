using UnityEngine;

namespace _Project.Scripts.Game
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float speed = 2f;
        [SerializeField] private int health = 1;
        [SerializeField] private int damage = 1;
        
        private Transform _target;

        public void Init(Transform target)
        {
            if (target == null)
                Destroy(gameObject);

            _target = target;
        }

        public void Update()
        {
            if (GameManager.Instance.GameState != GameState.Playing)
            {
                Destroy(gameObject);
                return;
            }
            
            var step =  speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _target.position, step);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.gameObject.CompareTag("Player")) 
                return;
            col.GetComponent<Player>().TakeDamage(damage);
            Destroy(gameObject);
        }

        public void TakeDamage(int dmg)
        {
            health -= dmg;
            if (health > 0) return;
            GameManager.Instance.IncreaseScore();
            Destroy(gameObject);
        }
    }
}
