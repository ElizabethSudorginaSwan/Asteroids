using UnityEngine;

namespace SpaceShooter.Factories
{
    public interface IEnemyFactory
    {
        public GameObject CreateEnemy(Vector3 position, Quaternion rotation);

        public void ConfigureEnemy(GameObject enemy, Transform playerTransform);
    }
}

