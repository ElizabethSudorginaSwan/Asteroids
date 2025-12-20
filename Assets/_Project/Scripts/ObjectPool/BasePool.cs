using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter.ObjectPool
{
    public abstract class BasePool
    {
        private GameObject[] _prefabs;
        private int _initialPoolSize;
        private Transform _parent;
        protected Queue<GameObject> _objectPool = new();

        public virtual void Initialize(GameObject[] prefabs, int initialPoolSize, Transform parent = null)
        {
            _prefabs = prefabs;
            _initialPoolSize = initialPoolSize;
            _parent = parent;
            InitializePool();

        }

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
            int randomIndex = Random.Range(0, _prefabs.Length);
            GameObject obj = Object.Instantiate(_prefabs[randomIndex]);

            if (_parent != null)
            {
                obj.transform.SetParent(_parent);
            }

            return obj;
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

        public void ClearPool()
        {
            foreach (var obj in _objectPool)
            {
                if (obj != null)
                    Object.Destroy(obj);
            }
            _objectPool.Clear();
        }
    }
}
