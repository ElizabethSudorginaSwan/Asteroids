using System.Collections.Generic;
using UnityEngine;
using SpaceShooter.Player;
using SpaceShooter.Factories;
using SpaceShooter.Enemies;
using SpaceShooter.ObjectPool;

namespace SpaceShooter.Asteroids
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class AsteroidsEnemy : MonoBehaviour, IDestructibleEnemy
    {
        [field: SerializeField] public float MoveForce { get; private set; }
        [field: SerializeField] public float MaxSpeed { get; private set; }
        [field: SerializeField] public float Drag { get; private set; }
        [field: SerializeField] public float MinSizeSmallAsteroid { get; private set; } 
        [field: SerializeField] public float MaxSizeSmallAsteroid { get; private set; }
        [field: SerializeField] public float MinRotateSmallAsteroid { get; private set; }
        [field: SerializeField] public float MaxRotateSmallAsteroid { get; private set; }
        [field: SerializeField] public int ScoreValue { get; private set; }

        private BasePool _smallAsteroidPool;
        private BasePool _asteroidPool;
        private PlayerMovement _playerMovement;
        private IEnemyFactory _smallAsteroidFactory;
        private readonly List<GameObject> _allSmallAsteroids = new ();
        private Rigidbody2D _rb;
        private Vector2 _randomDirection;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.drag = Drag;
        }

        private void Start()
        {
            _smallAsteroidFactory = new SmallAsteroidFactory(_smallAsteroidPool, MinSizeSmallAsteroid, MaxSizeSmallAsteroid, MinRotateSmallAsteroid, MaxRotateSmallAsteroid);
            _randomDirection = Random.insideUnitCircle.normalized;
        }

        private void FixedUpdate()
        {
            if (_rb.velocity.magnitude < MaxSpeed)
            {
                _rb.AddForce(_randomDirection * MoveForce, ForceMode2D.Force);
            }
        }

        public void SetSmallAsteroidPool(BasePool smallAsteroidPool)
        {
            _smallAsteroidPool = smallAsteroidPool;
        }

        public void SetAsteroidPool(BasePool asteroidPool)
        {
            _asteroidPool = asteroidPool;
        }

        public void SetPlayer(PlayerMovement player)
        {
            _playerMovement = player;
        }

        public void HandleBulletHit()
        {
            SpawnSmallAsteroids();
            _asteroidPool.ReturnObject(gameObject);
        }

        public void HandleLazerHit() 
        {
            _asteroidPool.ReturnObject(gameObject);
        }
        
        private void SpawnSmallAsteroids()
        {
            for (int i = 0; i < 2; i++)
            {
                var smallAsteroid = _smallAsteroidFactory.CreateEnemy(transform.position, Quaternion.identity);
                _smallAsteroidFactory.ConfigureEnemy(smallAsteroid, _playerMovement.transform);

                if (smallAsteroid.TryGetComponent(out SmallAsteroidEnemy smallEnemy))
                {
                    smallEnemy.SetPlayer(_playerMovement);
                    smallEnemy.SetParentList(_allSmallAsteroids);
                    smallEnemy.SetSmallAsteroidPool(_smallAsteroidPool);
                }
            }
        }
    }
}

