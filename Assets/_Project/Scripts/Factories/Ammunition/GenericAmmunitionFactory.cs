using SpaceShooter.ObjectPool;
using UnityEngine;

namespace SpaceShooter.Factories
{
    public class GenericAmmunitionFactory : IAmmunitionFactory 
    {
        private readonly BasePool _pool;
        private readonly float _speed;
        private readonly Transform _firePoint;

        public GenericAmmunitionFactory(BasePool pool, float speed, Transform firePoint)
        {
            _pool = pool;
            _speed = speed;
            _firePoint = firePoint;
        }

        public GameObject CreateAmmunition(Vector3 position, Quaternion rotation)
        {
            GameObject ammunition = _pool.GetObject(position, rotation);
            if (ammunition.TryGetComponent(out Rigidbody2D rb))
            {
                rb.velocity = _firePoint.up * _speed;
            }

            return ammunition;
        }
    }
}
