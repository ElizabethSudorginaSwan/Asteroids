using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceShooter.Player;
using SpaceShooter.Factories;
using SpaceShooter.ObjectPool;

namespace SpaceShooter.Asteroids
{
    public class AsteroidSpawner : MonoBehaviour
    {
        [field: SerializeField] public BasePool AsteroidPool {  get; set; }
        [field: SerializeField] public BasePool SmallAsteroidPool { get; set; }
        [field: SerializeField] public Transform[] SpawnPoints { get; private set; }
        [field: SerializeField] public float MinSizeAsteroid { get; private set; }
        [field: SerializeField] public float MaxSizeAsteroid { get; private set; }
        [field: SerializeField] public float MinRotateAsteroid { get; private set; }
        [field: SerializeField] public float MaxRotateAsteroid { get; private set; }
        [field: SerializeField] public float SpawnInterval { get; private set; }
        [field: SerializeField] public PlayerMovement PlayerMovement { get; private set; }

        private IEnemyFactory _asteroidFactory;
        private readonly List<GameObject> _asteroidList = new();
        private bool _isPaused = false;

        private void Start()
        {
            _asteroidFactory = new AsteroidFactory(AsteroidPool, MinSizeAsteroid, MaxSizeAsteroid, MinRotateAsteroid, MaxRotateAsteroid)/*, SpawnPoints)*/;

            StartCoroutine(DelayedAction());
        }

        private void Update()
        {
            if (!PlayerMovement.Live)
            {
                ClearAllAsteroids();
            }
        }

        private IEnumerator DelayedAction()
        {
            while (true)
            {
                yield return new WaitForSeconds(SpawnInterval);

                int spawnIndex = Random.Range(0, SpawnPoints.Length);
                var spawnPoint = SpawnPoints[spawnIndex];

                var asteroid = _asteroidFactory.CreateEnemy(spawnPoint.position, Quaternion.identity);
                _asteroidFactory.ConfigureEnemy(asteroid, PlayerMovement.transform);

                if (asteroid.TryGetComponent(out AsteroidsEnemy asteroidScript))
                {
                    asteroidScript.SetSmallAsteroidPool(SmallAsteroidPool);
                    asteroidScript.SetAsteroidPool(AsteroidPool);
                }

                _asteroidList.Add(asteroid);
            }
        }

        public void SetPause(bool paused)
        {
            _isPaused = paused;

            if (paused)
            {
                StopAllCoroutines();
            }
            else
            {
                StartCoroutine(DelayedAction());
            }
        }

        private void ClearAllAsteroids()
        {
            foreach (var asteroid in _asteroidList)
            {
                if (asteroid != null)
                {
                    AsteroidPool.ReturnObject(asteroid);
                }
            }
            _asteroidList.Clear();
        }
    }
}

