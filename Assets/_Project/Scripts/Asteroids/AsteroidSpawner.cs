using System.Collections.Generic;
using UnityEngine;
using SpaceShooter.Player;
using SpaceShooter.Factories;
using SpaceShooter.ObjectPool;
using System.Threading;
using Cysharp.Threading.Tasks;

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

        private IPool _asteroidPool;
        private IPool _smallAsteroidPool;

        private PlayerMovement _playerMovement;
        private CancellationTokenSource _spawnCts;
        private readonly List<GameObject> _asteroidList = new();

        public void Initialize(IPool asteroidPool, IPool smallAsteroidPool,
                               float minSizeAsteroid, float maxSizeAsteroid, float minRotateAsteroid, 
                               float maxRotateSateroid, float spawnIntervalAsteroid, PlayerMovement playerMovement)
        {
            _asteroidPool = asteroidPool;
            _smallAsteroidPool = smallAsteroidPool;
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
            Vector3 spawnPosition = GetRandomPositionOutsideScreen();

            var asteroid = _asteroidFactory.CreateEnemy(spawnPosition, Quaternion.identity, _playerMovement.transform);

            if (asteroid.TryGetComponent(out AsteroidsEnemy asteroidScript))
            {
                asteroidScript.SetAsteroidPool(_asteroidPool);
                asteroidScript.SetSmallAsteroidPool(_smallAsteroidPool);
            }

            _asteroidList.Add(asteroid);
        }

        private Vector3 GetRandomPositionOutsideScreen()
        {
            Camera mainCamera = Camera.main;
            float padding = 0.2f;
            int side = Random.Range(0, 4);

            Vector3 viewportPosition = Vector3.zero;

            switch (side)
            {
                case 0: 
                    viewportPosition = new Vector3(-padding, Random.Range(0f, 1f), mainCamera.nearClipPlane);
                    break;

                case 1: 
                    viewportPosition = new Vector3(1f + padding, Random.Range(0f, 1f), mainCamera.nearClipPlane);
                    break;

                case 2: 
                    viewportPosition = new Vector3(Random.Range(0f, 1f), -padding, mainCamera.nearClipPlane);
                    break;

                case 3: 
                    viewportPosition = new Vector3(Random.Range(0f, 1f), 1f + padding, mainCamera.nearClipPlane);
                    break;
            }

            Vector3 worldPosition = mainCamera.ViewportToWorldPoint(viewportPosition);
            worldPosition.z = 0f;

            return worldPosition;
        }
    }
}

