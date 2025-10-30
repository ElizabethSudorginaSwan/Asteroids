using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceShooter.Player;
using SpaceShooter.Factories;
using SpaceShooter.ObjectPool;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

namespace SpaceShooter.UFOs
{
    public class UFOSpawner
    {
        private float _minSizeUFO;
        private float _maxSizeUFO;
        private float _spawnIntervalUFO;

        private IEnemyFactory _ufoFactory;
        
        private BasePool _ufoPool;
        private Transform[] _spawnPoints;
        private PlayerMovement _playerMovement;
        private CancellationTokenSource _spawnCts;
        private readonly List<GameObject> _spawnedUfos = new();

        public void Initialize(BasePool ufoPool, Transform[] spawnPoints, float minSizeUFO, float maxSizeUFO, 
                                float spawnIntervalUFO, PlayerMovement playerMovement)
        {
            _ufoPool = ufoPool;
            _spawnPoints = spawnPoints;
            _minSizeUFO = minSizeUFO;
            _maxSizeUFO = maxSizeUFO;
            _spawnIntervalUFO = spawnIntervalUFO;
            _playerMovement = playerMovement;

            _ufoFactory = new UFOFactory(_ufoPool, _minSizeUFO, _maxSizeUFO);
        }

        public void UpdateUFOSpawner()
        {
            if (!_playerMovement.Live)
            {
                ClearAllUfo();
            }
        }

        private void ClearAllUfo()
        {
            foreach (var ufo in _spawnedUfos)
            {
                if (ufo != null)
                {
                    _ufoPool.ReturnObject(ufo);
                }
            }

            _spawnedUfos.Clear();
        }

        public async UniTask StartSpawning()
        {
            _spawnCts = new CancellationTokenSource();

            while (!_spawnCts.Token.IsCancellationRequested)
            {
                await UniTask.Delay((int)(_spawnIntervalUFO * 1000), cancellationToken: _spawnCts.Token);

                if (!_playerMovement.Live)
                {
                    continue;    
                }

                SpawnUFO();
            }
        }

        private void SpawnUFO()
        {
            int spawnIndex = UnityEngine.Random.Range(0, _spawnPoints.Length);
            var spawnPoint = _spawnPoints[spawnIndex];

            var ufo = _ufoFactory.CreateEnemy(spawnPoint.position, Quaternion.identity, _playerMovement.transform);

            if (ufo.TryGetComponent(out UFOEnemy ufoScript))
            {
                ufoScript.SetUfoPool(_ufoPool);
            }

            _spawnedUfos.Add(ufo);
        }
    }
}


