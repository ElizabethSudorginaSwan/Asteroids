using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceShooter.Player;
using SpaceShooter.Factories;
using SpaceShooter.ObjectPool;
using System.Threading;
using Cysharp.Threading.Tasks;
using SpaceShooter.UFOs;

namespace SpaceShooter.Asteroids
{
    public class AsteroidSpawner
    {
        private float _minSizeAsteroid;
        private float _maxSizeAsteroid;
        private float _minRotateAsteroid;
        private float _maxRotateAsteroid;
        private float _spawnIntervalAsteroid;

        private IEnemyFactory _asteroidFactory;

        private BasePool _asteroidPool;
        private BasePool _smallAsteroidPool;
        private Transform[] _spawnPoints;
        private PlayerMovement _playerMovement;
        private CancellationTokenSource _spawnCts;
        private readonly List<GameObject> _asteroidList = new();

        public void Initialize(BasePool asteroidPool, BasePool smallAsteroidPool, Transform[] spawnPoints, 
                               float minSizeAsteroid, float maxSizeAsteroid, float minRotateAsteroid, 
                               float maxRotateSateroid, float spawnIntervalAsteroid, PlayerMovement playerMovement)
        {
            _asteroidPool = asteroidPool;
            _smallAsteroidPool = smallAsteroidPool;
            _spawnPoints = spawnPoints;
            _minSizeAsteroid = minSizeAsteroid;
            _maxSizeAsteroid = maxSizeAsteroid;
            _minRotateAsteroid = minRotateAsteroid;
            _maxRotateAsteroid = maxRotateSateroid;
            _spawnIntervalAsteroid = spawnIntervalAsteroid;
            _playerMovement = playerMovement;

            _asteroidFactory = new AsteroidFactory(_asteroidPool, _minSizeAsteroid, _maxSizeAsteroid, _minRotateAsteroid, _maxRotateAsteroid);
        }

        public void UpdateAsteroidSpawner()
        {
            if (!_playerMovement.Live)
            {
                ClearAllAsteroids();
            }
        }

        private void ClearAllAsteroids()
        {
            foreach (var asteroid in _asteroidList)
            {
                if (asteroid != null)
                {
                    _asteroidPool.ReturnObject(asteroid);
                }
            }
            _asteroidList.Clear();
        }

        public async UniTask StartSpawning()
        {
            _spawnCts = new CancellationTokenSource();

            while (!_spawnCts.Token.IsCancellationRequested)
            {
                await UniTask.Delay((int)(_spawnIntervalAsteroid * 1000), cancellationToken: _spawnCts.Token);

                if (!_playerMovement.Live)
                {
                    continue; 
                }

                SpawnAsteroid();
            }
        }

        private void SpawnAsteroid()
        {
            int spawnIndex = UnityEngine.Random.Range(0, _spawnPoints.Length);
            var spawnPoint = _spawnPoints[spawnIndex];

            var asteroid = _asteroidFactory.CreateEnemy(spawnPoint.position, Quaternion.identity, _playerMovement.transform);

            if (asteroid.TryGetComponent(out AsteroidsEnemy asteroidScript))
            {
                asteroidScript.SetAsteroidPool(_asteroidPool);
                asteroidScript.SetSmallAsteroidPool(_smallAsteroidPool);
            }

            _asteroidList.Add(asteroid);
        }
    }
}

