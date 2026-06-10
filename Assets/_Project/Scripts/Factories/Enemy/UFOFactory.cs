using SpaceShooter.ObjectPool;
using SpaceShooter.UFOs;
using UnityEngine;

namespace SpaceShooter.Factories
{
    public class UFOFactory : IEnemyFactory
    {
        [field: SerializeField] public IPool UfoPool {  get; set; }

        private readonly float _minSize;
        private readonly float _maxSize;

        public UFOFactory(IPool ufoPool, float minSize, float maxSize)
        {
            UfoPool = ufoPool;
            _minSize = minSize;
            _maxSize = maxSize;
        }
        
        public GameObject CreateEnemy(Vector3 position, Quaternion rotation, Transform playerTransform)
        {
            GameObject ufo = UfoPool.GetObject(position, rotation);

            float randomSize = Random.Range(_minSize, _maxSize);
            ufo.transform.localScale = new Vector3(randomSize, randomSize, 1f);

            if (ufo.TryGetComponent(out UFOEnemy ufoEnemy))
            {
                ufoEnemy.SetPlayer(playerTransform);
            }

            return ufo;
        }
    }
}

