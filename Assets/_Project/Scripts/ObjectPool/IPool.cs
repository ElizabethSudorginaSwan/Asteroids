using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter.ObjectPool
{
    public interface IPool 
    {
        GameObject GetObject(Vector3 position, Quaternion rotation);
        void ReturnObject(GameObject obj);
        void ClearPool();
    }
}

