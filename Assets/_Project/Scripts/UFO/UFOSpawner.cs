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
        
        private IPool _ufoPool;
        private PlayerMovement _playerMovement;
        private CancellationTokenSource _spawnCts;
        private readonly List<GameObject> _spawnedUfos = new();

        public void Initialize(IPool ufoPool, float minSizeUFO, float maxSizeUFO, 
                                float spawnIntervalUFO, PlayerMovement playerMovement)
        {
            _ufoPool = ufoPool;
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
            Vector3 spawnPosition = GetRandomPositionOutsideScreen();

            var ufo = _ufoFactory.CreateEnemy(spawnPosition, Quaternion.identity, _playerMovement.transform);

            if (ufo.TryGetComponent(out UFOEnemy ufoScript))
            {
                ufoScript.SetUfoPool(_ufoPool);
            }

            _spawnedUfos.Add(ufo);
        }

        private Vector3 GetRandomPositionOutsideScreen()
        {
            Camera mainCamera = Camera.main;
            float padding = 0.2f;
            int side = UnityEngine.Random.Range(0, 4);

            Vector3 viewportPosition = Vector3.zero;

            switch (side)
            {
                case 0:
                    viewportPosition = new Vector3(-padding, UnityEngine.Random.Range(0f, 1f), mainCamera.nearClipPlane);
                    break;

                case 1:
                    viewportPosition = new Vector3(1f + padding, UnityEngine.Random.Range(0f, 1f), mainCamera.nearClipPlane);
                    break;

                case 2:
                    viewportPosition = new Vector3(UnityEngine.Random.Range(0f, 1f), -padding, mainCamera.nearClipPlane);
                    break;

                case 3:
                    viewportPosition = new Vector3(UnityEngine.Random.Range(0f, 1f), 1f + padding, mainCamera.nearClipPlane);
                    break;
            }

            Vector3 worldPosition = mainCamera.ViewportToWorldPoint(viewportPosition);
            worldPosition.z = 0f;

            return worldPosition;
        }
    }
}


