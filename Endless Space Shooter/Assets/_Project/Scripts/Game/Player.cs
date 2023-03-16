using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Game
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> audioClips = new();

        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform bulletSpawnPosition;
        
        private Camera _camera;
        private Vector3 _worldPosition;
        private Vector3 _worldPositionShoot;
        
        private float _speed = 3f;
        private int _health = 3;
        
        private bool _moving;
        private bool _shooting;

        private const float BulletDelay = .1f;
        private int _bulletDamageModifier = 1;
        private int _bulletSpeedModifier = 1;
        private float _bulletTimer;
        
        private const float MaxTouchTime = 0.2f;
        private float _touchTimer;
        private bool _touchStarted;

        private int Health
        {
            get => _health;
            set
            {
                _health = value;
                GameManager.Instance.OnPlayerHealthChanged?.Invoke(_health);
                
                if (_health <= 0)
                    GameManager.Instance.Death();
            }
        }

        private void Start()
        {
            _camera = Camera.main;
            Health = 3;
        }

        private void Update()
        {
            if (_shooting)
                _bulletTimer -= Time.deltaTime;
            
            if (Input.touchSupported)
            {
                TouchInputs();
            }
            else
            {
                MouseInputs();  
            }
        }

        private void TouchInputs()
        {
            //Move with touch
            if (Input.touchCount == 1)
            {
                var touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                    {
                        _touchStarted = true;
                        GetWorldPos();
                        break;
                    }
                    case TouchPhase.Ended:
                    {
                        // move
                        if (_touchTimer < MaxTouchTime)
                        {
                            _moving = true;
                        }
            
                        _touchStarted = false;
                        _touchTimer = 0f;
                        _shooting = false;
                        break;
                    }
                    case TouchPhase.Stationary:
                    {
                        //shoot
                        if (_touchTimer >= MaxTouchTime)
                        {
                            _shooting = true;
                        }
                        break;
                    }
                    case TouchPhase.Moved:
                        if (_touchTimer >= MaxTouchTime)
                        {
                            _shooting = true;
                            GetWorldPos();
                        }
                        break;
                    case TouchPhase.Canceled:
                        _shooting = false;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            
                if (_touchStarted)
                {
                    _touchTimer += Time.deltaTime;
                }
            }
            else
            {
                Move();
                Shoot();
            }
        }

        private void MouseInputs()
        {
            if (Input.GetButtonDown("Fire1") && _camera.pixelRect.Contains(Input.mousePosition))
            {
                _worldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                _worldPosition.z = 0;
                _moving = true;
                
                AudioManager.Instance.PlayClip(audioClips[4]);
            }
            Move();

            if (Input.GetButtonDown("Fire2"))
            {
                _shooting = true;
                _worldPositionShoot = _camera.ScreenToWorldPoint(Input.mousePosition);
                _worldPositionShoot.z = 0;
                Shoot();
            }
            else if (Input.GetButton("Fire2"))
            {
                _worldPositionShoot = _camera.ScreenToWorldPoint(Input.mousePosition);
                _worldPositionShoot.z = 0;
                Shoot();
            }
            else if (Input.GetButtonUp("Fire2"))
            {
                _shooting = false;
            }
        }
        
        private void Shoot()
        {
            if (!_shooting)
                return;
            if (!(_bulletTimer <= 0f)) 
                return;
            
            var bullet = Instantiate(bulletPrefab, bulletSpawnPosition.position, Quaternion.identity)
                .GetComponent<Bullet>();
            bullet.Init(_worldPositionShoot, _bulletDamageModifier, _bulletSpeedModifier);
            
            _bulletTimer = BulletDelay;

            var rnd = Random.Range(0, 1);
            AudioManager.Instance.PlayClip(audioClips[rnd]);
        }

        private void GetWorldPos()
        {
            var touch = Input.GetTouch(0);
            _worldPosition = _camera.ScreenToWorldPoint(touch.position);
            _worldPosition.z = 0;
        }

        private void Move()
        {
            if (!_moving)
                return;

            var step = _speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position,_worldPosition, step);
            
            if (Vector3.Distance(transform.position, _worldPosition) < 0.001f)
                _moving = false;
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            AudioManager.Instance.PlayClip(audioClips.Last());
        }

        public void AddPowerUp(PowerUpType type, float modifier)
        {
            var rnd = Random.Range(2, 3);
            AudioManager.Instance.PlayClip(audioClips[rnd]);
            
            switch (type)
            {
                case PowerUpType.Speed:
                    _speed += modifier;
                    break;
                case PowerUpType.Health:
                    Health += (int) modifier;
                    break;
                case PowerUpType.Damage:
                    _bulletDamageModifier += (int) modifier;
                    break;
                case PowerUpType.BulletSpeed:
                    _bulletSpeedModifier += (int) modifier;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
