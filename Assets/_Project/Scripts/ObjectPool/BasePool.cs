using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter.ObjectPool
{
    public abstract class BasePool : MonoBehaviour
    {
        [SerializeField] protected GameObject[] _prefabs;
        [SerializeField] protected int _initialPoolSize;

        protected Queue<GameObject> _objectPool = new();

        protected virtual void InitializePool()
        {
            for (int i = 0; i < _initialPoolSize; i++)
            {
                GameObject obj = CreateNewObject();
                obj.SetActive(false);
                _objectPool.Enqueue(obj);
            }
        }

        protected GameObject CreateNewObject()
        {
            if (_prefabs.Length > 1)
            {
                int randomIndex = Random.Range(0, _prefabs.Length);
                return Instantiate(_prefabs[randomIndex], transform);
            }

            return Instantiate(_prefabs[0], transform);
        }

        public GameObject GetObject(Vector3 position, Quaternion rotation)
        {
            GameObject obj = _objectPool.Count > 0 ? _objectPool.Dequeue() : CreateNewObject();

            obj.transform.SetPositionAndRotation(position, rotation);
            obj.SetActive(true);

            return obj;
        }

        public void ReturnObject(GameObject obj)
        {
            obj.SetActive(false);
            _objectPool.Enqueue(obj);
        }
    }
}
