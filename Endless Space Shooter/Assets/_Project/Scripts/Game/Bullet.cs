using UnityEngine;

namespace _Project.Scripts.Game
{
    public class Bullet : MonoBehaviour
    {
        private int _damage = 1;
        private float _speed = 3f;
        private Vector3 _direction;
        private float _maxLife;
        
        public void Init(Vector3 worldPosition, int damageModifier = 1, float speedModifier = 1f)
        {
            _damage *= damageModifier;
            _speed *= speedModifier;
            SetDirection(worldPosition);
        }
        
        private void SetDirection(Vector3 worldPosition)
        {
            worldPosition.z = 0;
            _direction = (worldPosition - transform.position).normalized;

            if (_direction == Vector3.zero)
                _direction = Vector3.forward;
        }

        private void Update()
        {
            var step = _speed * Time.deltaTime;
            transform.position += _direction * step;

            _maxLife += Time.deltaTime;
            if (_maxLife > 5f)
                Destroy(gameObject);
        }
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.gameObject.CompareTag("Enemy")) 
                return;
            
            col.GetComponent<Enemy>().TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}
