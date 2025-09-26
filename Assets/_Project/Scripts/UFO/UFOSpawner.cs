using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceShooter.Player;
using SpaceShooter.Factories;
using SpaceShooter.ObjectPool;

namespace SpaceShooter.UFOs
{
    public class UFOSpawner : MonoBehaviour
    {
        [field: SerializeField] public BasePool UfoPool {  get; set; }
        [field: SerializeField] public Transform[] SpawnPoints { get; private set; }
        [field: SerializeField] public float MinSizeUFO { get; private set; }
        [field: SerializeField] public float MaxSizeUFO { get; private set; }
        [field: SerializeField] public float SpawnInterval { get; private set; }
        [field: SerializeField] public PlayerMovement PlayerMovement { get; private set; }
        
        private IEnemyFactory _ufoFactory;
        private readonly List<GameObject> _spawnedUfos = new ();
        private bool _isPaused = false;

        private void Start()
        {
            _ufoFactory = new UFOFactory(UfoPool, MinSizeUFO, MaxSizeUFO);
            StartCoroutine(DelayedAction());
        }

        private void Update()
        {
            if (!PlayerMovement.Live)
            {
                ClearAllUfo();
            }
        }

        private IEnumerator DelayedAction()
        {
            while (true)
            {
                yield return new WaitForSeconds(SpawnInterval);

                int spawnIndex = Random.Range(0, SpawnPoints.Length);
                var spawnPoint = SpawnPoints[spawnIndex];

                var ufo = _ufoFactory.CreateEnemy(spawnPoint.position, Quaternion.identity, PlayerMovement.transform);

                if (ufo.TryGetComponent(out UFOEnemy ufoScript))
                {
                    ufoScript.SetUfoPool(UfoPool);
                }

                _spawnedUfos.Add(ufo);
            }
        }

        private void ClearAllUfo()
        {
            foreach (var ufo in _spawnedUfos)
            {
                if (ufo != null)
                {
                    UfoPool.ReturnObject(ufo);
                }
            }

            _spawnedUfos.Clear();
        }

        public void SetPause(bool paused)
        {
            _isPaused = paused;
 
            if (paused)
            {
                StopAllCoroutines();  
            }
            else
            {
                StartCoroutine(DelayedAction()); 
            }
        }
    }
}


