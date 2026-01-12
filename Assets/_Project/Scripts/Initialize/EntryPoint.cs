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
using UnityEngine.UI;

namespace SpaceShooter.Initializer
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private GameConfig _config;

        [SerializeField] private Transform _canvasGameOverTransform;
        [SerializeField] private Transform _canvasGameUITransform;
        
        [SerializeField] private GameObject _canvasGameOver;
        [SerializeField] private GameObject _canvasGame;

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

        private GameObject _backgroundInstance;
        private ScoreManagerUI _scoreManagerUI; 
        private GameObject _buttonPlayAgainInstance;

        private PlayerUI _playerUI;
        private ShooterUI _shooterUI;

        private PlayerMovement _playerMovement;
        private Shooter _shooter;

        private void Awake()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            CreatePlayerPrefab();
            CreateUIPrefabs();
            CreatePoolContainers();
            InitializePools();
            InitializeManagers(); 
            StartGame();
        }

        private void CreatePlayerPrefab()
        {
            GameObject player = Instantiate(_config.PlayerPrefab);
            player.name = "Player";
            _playerMovement = player.GetComponent<PlayerMovement>();
            _shooter = player.GetComponent<Shooter>();
        }

        private void CreateUIPrefabs()
        {
            GameObject playerUI = Instantiate(_config.UIPlayerPrefab, _canvasGameUITransform);
            playerUI.name = "UIPlayer";
            _playerUI = playerUI.GetComponent<PlayerUI>();

            GameObject shooterUI = Instantiate(_config.UIShooterPrefab, _canvasGameUITransform);
            shooterUI.name = "UIShooter";
            _shooterUI = shooterUI.GetComponent<ShooterUI>();

            _backgroundInstance = Instantiate(_config.BackgroundPrefab, _canvasGameOverTransform);
            _backgroundInstance.name = "Background";

            GameObject scoreUIObject = Instantiate(_config.ScoreManagerPrefab, _canvasGameOverTransform);
            scoreUIObject.name = "ScoreManagerUI";
            _scoreManagerUI = scoreUIObject.GetComponent<ScoreManagerUI>();

            _buttonPlayAgainInstance = Instantiate(_config.ButtonPlayAgainPrefab, _canvasGameOverTransform);
            _buttonPlayAgainInstance.name = "ButtonPlayAgain";
            Button button = _buttonPlayAgainInstance.GetComponent<Button>();
            _playerUI.InitializeButton(button);  
        }

        private void CreatePoolContainers() 
        {
            _ufoPoolParent = CreatePoolContainer("UFOPool", transform);
            _asteroidPoolParent = CreatePoolContainer("AsteroidPool", transform);
            _smallAsteroidPoolParent = CreatePoolContainer("SmallAsteroidPool", transform);
            _bulletPoolParent = CreatePoolContainer("BulletPool", transform);
            _lazerPoolParent = CreatePoolContainer("LazerPool", transform);
        }

        private Transform CreatePoolContainer(string containerName, Transform parent)
        {
            GameObject container = new (containerName);
            container.transform.SetParent(parent);
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

            _playerUI.Initialize(_scoreManager, _pauseGame, _canvasGameOver, _canvasGame, _playerMovement);
            _scoreManagerUI.Initialize(_scoreManager);
            _shooterUI.Initialize(_shooter);

            _playerMovement.Initialize(_playerInput, _playerUI, _pauseGame);

            _spawnerUFO.Initialize(_ufoPool, _config.MinSizeUFO, _config.MaxSizeUFO, _config.SpawnIntervalUFO, _playerMovement);
            _spawnerAsteroid.Initialize(_asteroidPool, _smallAsteroidPool, _config.MinSizeAsteroid,
                                        _config.MaxSizeAsteroid, _config.MinRotateAsteroid, _config.MaxRotateAsteroid,
                                        _config.SpawnIntervalAsteroid, _playerMovement);

            _shooter.Initialize(_playerMovement, _bulletPool, _lazerPool);

         
        }

        private void StartGame()
        {
            _playerMovement.SetLive(true);
            _playerUI.HideGameOver();
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

