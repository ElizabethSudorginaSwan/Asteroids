using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter.ObjectPool
{
    public abstract class BasePool
    {
        protected GameObject[] _prefabs;
        protected int _initialPoolSize;
        protected Transform _parent;
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

        protected virtual GameObject CreateNewObject()
        {
            int randomIndex = Random.Range(0, _prefabs.Length);
            GameObject obj = Object.Instantiate(_prefabs[randomIndex]);

            if (_parent != null)
            {
                obj.transform.SetParent(_parent);
            }

            return obj;
        }

        public virtual GameObject GetObject(Vector3 position, Quaternion rotation)
        {
            GameObject obj = _objectPool.Count > 0 ? _objectPool.Dequeue() : CreateNewObject();

            obj.transform.SetPositionAndRotation(position, rotation);
            obj.SetActive(true);

            return obj;
        }

        public virtual void ReturnObject(GameObject obj)
        {
            obj.SetActive(false);
            _objectPool.Enqueue(obj);
        }

        public virtual void ClearPool()
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
