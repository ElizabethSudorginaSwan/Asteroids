using UnityEngine;

namespace SpaceShooter.Factories
{
    public interface IEnemyFactory
    {
        public GameObject CreateEnemy(Vector3 position, Quaternion rotation, Transform playerTransform);

    }
}

