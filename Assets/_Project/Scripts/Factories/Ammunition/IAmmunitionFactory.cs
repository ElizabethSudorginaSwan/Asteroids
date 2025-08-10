using UnityEngine;

namespace SpaceShooter.Factories
{
    public interface IAmmunitionFactory 
    {
        public GameObject CreateAmmunition(Vector3 position, Quaternion rotation);
    }
}

