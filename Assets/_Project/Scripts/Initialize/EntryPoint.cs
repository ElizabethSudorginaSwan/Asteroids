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
        [field: SerializeField] public ShooterUI ShooterUI { get; set; }

        [SerializeField] private GameConfig _config;

        private Transform _ufoPoolParent;
        private Transform _asteroidPoolParent;
        private Transform _smallAsteroidPoolParent;
        private Transform _bulletPoolParent;
        private Transform _lazerPoolParent;

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
            CreatePoolContainers();
            InitializePools();
            InitializeManagers();
            StartGame();
        }

        private void CreatePoolContainers()
        {
            _ufoPoolParent = CreatePoolContainer("UFOPool");
            _asteroidPoolParent = CreatePoolContainer("AsteroidPool");
            _smallAsteroidPoolParent = CreatePoolContainer("SmallAsteroidPool");
            _bulletPoolParent = CreatePoolContainer("BulletPool");
            _lazerPoolParent = CreatePoolContainer("LazerPool");
        }

        private Transform CreatePoolContainer(string containerName)
        {
            GameObject container = new GameObject(containerName);
            container.transform.SetParent(transform);
            return container.transform;
        }

        private void InitializePools()
        {
            _ufoPool = new UFOPool();
            _asteroidPool = new AsteroidPool();
            _smallAsteroidPool = new SmallAsteroidPool();
            _bulletPool = new BulletPool();
            _lazerPool = new LazerPool();

            _ufoPool.Initialize(_config.UfoPrefabs, _config.UfoPoolSize, _ufoPoolParent);
            _asteroidPool.Initialize(_config.AsteroidPrefabs, _config.AsteroidPoolSize, _asteroidPoolParent);
            _smallAsteroidPool.Initialize(_config.SmallAsteroidPrefabs, _config.SmallAsteroidPoolSize, _smallAsteroidPoolParent);
        }

        private void InitializeManagers()
        {
            _scoreManager = new ScoreManager();

            _bulletPool.Initialize(_config.BulletPrefabs, _config.BulletPoolSize, _scoreManager, _bulletPoolParent);
            _lazerPool.Initialize(_config.LazerPrefabs, _config.LazerPoolSize, _scoreManager, _lazerPoolParent);

            _playerInput = new PlayerInput();
            _spawnerUFO = new UFOSpawner();
            _spawnerAsteroid = new AsteroidSpawner();
            _pauseGame = new PauseGame();

            PlayerUI.Initialize(_scoreManager, _pauseGame);
            ScoreUI.Initialize(_scoreManager);
            ShooterUI.Initialize(Shooter);

            PlayerMovement.Initialize(_playerInput, PlayerUI, _pauseGame);

            _spawnerUFO.Initialize(_ufoPool, _config.MinSizeUFO, _config.MaxSizeUFO, _config.SpawnIntervalUFO, PlayerMovement);
            _spawnerAsteroid.Initialize(_asteroidPool, _smallAsteroidPool, _config.MinSizeAsteroid,
                                        _config.MaxSizeAsteroid, _config.MinRotateAsteroid, _config.MaxRotateAsteroid,
                                        _config.SpawnIntervalAsteroid, PlayerMovement);

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

