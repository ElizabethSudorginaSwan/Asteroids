using SpaceShooter.Asteroids;
using SpaceShooter.ObjectPool;
using SpaceShooter.Player;
using UnityEngine;

namespace SpaceShooter.Factories
{
    public class SmallAsteroidFactory : IEnemyFactory
    {
        [field: SerializeField] IPool SmallAsteroidPool {  get; set; }

        private readonly float _minSize;
        private readonly float _maxSize;
        private readonly float _minRotation;
        private readonly float _maxRotation;

        public SmallAsteroidFactory(IPool smallAsteroidPool, float minSize, float maxSize, float minRotation, float maxRotation)
        {
            SmallAsteroidPool = smallAsteroidPool;
            _minSize = minSize;
            _maxSize = maxSize;
            _minRotation = minRotation;
            _maxRotation = maxRotation;
        }

        public GameObject CreateEnemy(Vector3 position, Quaternion rotation, Transform playerTransform)
        {
            GameObject smallAsteroid = SmallAsteroidPool.GetObject(position, rotation);

            float randomSize = Random.Range(_minSize, _maxSize);
            smallAsteroid.transform.localScale = new Vector2(randomSize, randomSize);

            float randomRotation = Random.Range(_minRotation, _maxRotation);
            smallAsteroid.transform.rotation = Quaternion.Euler(0f, 0f, randomRotation);

            if (smallAsteroid.TryGetComponent(out SmallAsteroidEnemy smallAsteroidEnemy))
            {
                smallAsteroidEnemy.SetPlayer(playerTransform.GetComponent<PlayerMovement>());
            }

            return smallAsteroid;
        }
    }
}

