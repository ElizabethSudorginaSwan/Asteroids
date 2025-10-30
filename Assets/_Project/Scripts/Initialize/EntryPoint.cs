using System.Collections;
using System.Collections.Generic;
using SpaceShooter.Asteroids;
using SpaceShooter.ObjectPool;
using SpaceShooter.Pause;
using SpaceShooter.Player;
using SpaceShooter.Score;
using SpaceShooter.UFOs;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace SpaceShooter.Initializer
{
    public class EntryPoint : MonoBehaviour
    {
        [field: SerializeField] public PlayerUI PlayerUI { get; set; }
        [field: SerializeField] public ScoreManagerUI ScoreUI { get; set; }
        [field: SerializeField] public PlayerMovement PlayerMovement { get; set; }
        [field: SerializeField] public Shooter Shooter { get; set; }
        
        [field: SerializeField] public Transform[] SpawnPoints { get; private set; }
        [field: SerializeField] public float MinSizeUFO { get; private set; }
        [field: SerializeField] public float MaxSizeUFO { get; private set; }
        [field: SerializeField] public float SpawnIntervalUFO { get; private set; }

        [field: SerializeField] public float MinSizeAsteroid { get; private set; }
        [field: SerializeField] public float MaxSizeAsteroid { get; private set; }
        [field: SerializeField] public float MinRotateAsteroid { get; private set; }
        [field: SerializeField] public float MaxRotateAsteroid { get; private set; }
        [field: SerializeField] public float SpawnIntervalAsteroid { get; private set; }

        [field: SerializeField] public GameObject[] BulletPrefabs { get; private set; }
        [field: SerializeField] public GameObject[] LazerPrefabs { get; private set; }
        [field: SerializeField] public GameObject[] UfoPrefabs { get; private set; }
        [field: SerializeField] public GameObject[] AsteroidPrefabs { get; private set; }
        [field: SerializeField] public GameObject[] SmallAsteroidPrefabs { get; private set; }

        [field: SerializeField] public int BulletPoolSize { get; private set; }
        [field: SerializeField] public int LazerPoolSize { get; private set; }
        [field: SerializeField] public int UfoPoolSize { get; private set; }
        [field: SerializeField] public int AsteroidPoolSize { get; private set; }
        [field: SerializeField] public int SmallAsteroidPoolSize { get; private set; }

        [field: SerializeField] public Transform UfoPoolParent { get; private set; }
        [field: SerializeField] public Transform AsteroidPoolParent { get; private set; }
        [field: SerializeField] public Transform SmallAsteroidPoolParent { get; private set; }
        [field: SerializeField] public Transform BulletPoolParent { get; private set; }
        [field: SerializeField] public Transform LazerPoolParent { get; private set; }

        private ScoreManager _scoreManager;
        private PlayerInput _playerInput;
        private UFOSpawner _spawnerUFO;
        private AsteroidSpawner _spawnerAsteroid;
        private PauseGame _pauseGame;

        private BulletPool _bulletPool;
        private LazerPool _lazerPool;
        private UFOPool _ufoPool;
        private AsteroidPool _asteroidPool;
        private SmallAsteroidPool _smallAsteroidPool;

        private void Awake()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            InitializePools();
            InitializeManagers();
            StartGame();
        }

        private void InitializePools()
        {
            _ufoPool = new UFOPool();
            _asteroidPool = new AsteroidPool();
            _smallAsteroidPool = new SmallAsteroidPool();
            _bulletPool = new BulletPool();
            _lazerPool = new LazerPool();

            _ufoPool.Initialize(UfoPrefabs, UfoPoolSize, UfoPoolParent);
            _asteroidPool.Initialize(AsteroidPrefabs, AsteroidPoolSize, AsteroidPoolParent);
            _smallAsteroidPool.Initialize(SmallAsteroidPrefabs, SmallAsteroidPoolSize, SmallAsteroidPoolParent);
        }

        private void InitializeManagers()
        {
            _scoreManager = new ScoreManager();

            _bulletPool.Initialize(BulletPrefabs, BulletPoolSize, _scoreManager, BulletPoolParent);
            _lazerPool.Initialize(LazerPrefabs, LazerPoolSize, _scoreManager, LazerPoolParent);

            _playerInput = new PlayerInput();
            _spawnerUFO = new UFOSpawner();
            _spawnerAsteroid = new AsteroidSpawner();
            _pauseGame = new PauseGame();

            PlayerUI.Initialize(_scoreManager, _pauseGame);
            ScoreUI.Initialize(_scoreManager);

            PlayerMovement.Initialize(_playerInput, PlayerUI, _pauseGame);

            _spawnerUFO.Initialize(_ufoPool, SpawnPoints, MinSizeUFO, MaxSizeUFO, SpawnIntervalUFO, PlayerMovement);
            _spawnerAsteroid.Initialize(_asteroidPool, _smallAsteroidPool, SpawnPoints, MinSizeAsteroid,
                                        MaxSizeAsteroid, MinRotateAsteroid, MaxRotateAsteroid,
                                        SpawnIntervalAsteroid, PlayerMovement);

            Shooter.Initialize(PlayerMovement, _bulletPool, _lazerPool);
        }

        private void StartGame()
        {
            PlayerMovement.SetLive(true);
            PlayerUI.HideGameOver();
            _scoreManager.ResetScore();
            _pauseGame.SetPause(false);
            _spawnerUFO.StartSpawning().Forget();
            _spawnerAsteroid.StartSpawning().Forget();
        }

        private void Update()
        {
            _playerInput.UpdateInput();
            _spawnerUFO.UpdateUFOSpawner();
            _spawnerAsteroid.UpdateAsteroidSpawner();
        }

        private void OnDestroy()
        {
            _ufoPool?.ClearPool();
            _asteroidPool?.ClearPool();
            _smallAsteroidPool?.ClearPool();
            _bulletPool?.ClearPool();
            _lazerPool?.ClearPool();
        }
    }
}

