using SpaceShooter.Asteroids;
using SpaceShooter.ObjectPool;
using SpaceShooter.Player;
using UnityEngine;

namespace SpaceShooter.Factories
{
    public class AsteroidFactory : IEnemyFactory 
    {
        [field: SerializeField] public BasePool AsteroidPool {  get; set; }

        private readonly float _minSize;
        private readonly float _maxSize;
        private readonly float _minRotation;
        private readonly float _maxRotation;

        public AsteroidFactory(BasePool asteroidPool, float minSize, float maxSize, float minRotation, float maxRotation)
        {
            AsteroidPool = asteroidPool;
            _minSize = minSize;
            _maxSize = maxSize;
            _minRotation = minRotation;
            _maxRotation = maxRotation;
        }

        public GameObject CreateEnemy(Vector3 position, Quaternion rotation, Transform playerTransform)
        {
            GameObject asteroid = AsteroidPool.GetObject(position, rotation);

            float randomSize = Random.Range(_minSize, _maxSize);
            asteroid.transform.localScale = new Vector2(randomSize, randomSize);

            float randomRotation = Random.Range(_minRotation, _maxRotation);
            asteroid.transform.rotation = Quaternion.Euler(0f, 0f, randomRotation);

            if (asteroid.TryGetComponent(out AsteroidsEnemy asteroidsEnemy))
            {
                asteroidsEnemy.SetPlayer(playerTransform.GetComponent<PlayerMovement>());
            }

            return asteroid;
        }
    }
}
