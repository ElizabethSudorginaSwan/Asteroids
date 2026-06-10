using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter.ObjectPool
{
    public abstract class BasePool<T> : IPool where T : Component
    {
        private T[] _prefabs;
        private int _initialPoolSize;
        private Transform _parent;
        protected Queue<T> _objectPool = new();

        public void Initialize(T[] prefabs, int initialPoolSize, Transform parent = null)
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
                T obj = CreateNewObject();
                obj.gameObject.SetActive(false);
                _objectPool.Enqueue(obj);
            }
        }

        protected T CreateNewObject()
        {
            int randomIndex = Random.Range(0, _prefabs.Length);
            T obj = Object.Instantiate(_prefabs[randomIndex]);

            if (_parent != null)
            {
                obj.transform.SetParent(_parent);
            }

            return obj;
        }

        public T GetObject(Vector3 position, Quaternion rotation)
        {
            T obj = _objectPool.Count > 0 ? _objectPool.Dequeue() : CreateNewObject();

            obj.transform.SetPositionAndRotation(position, rotation);
            obj.gameObject.SetActive(true);

            return obj;
        }

        // явна€ реализаци€ интерфейса IPool
        GameObject IPool.GetObject(Vector3 position, Quaternion rotation)
        {
            T obj = GetObject(position, rotation);
            return obj.gameObject;
        }

        public void ReturnObject(T obj)
        {
            obj.gameObject.SetActive(false);
            _objectPool.Enqueue(obj);
        }

        // явна€ реализаци€ интерфейса IPool
        void IPool.ReturnObject(GameObject obj)
        {
            if (obj.TryGetComponent<T>(out T component))
            {
                ReturnObject(component);
            }
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
