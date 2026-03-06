using Cysharp.Threading.Tasks;
using SpaceShooter.Asteroids;
using SpaceShooter.MVPPlayer;
using SpaceShooter.MVPShooter;
using SpaceShooter.ObjectPool;
using SpaceShooter.Pause;
using SpaceShooter.Player;
using SpaceShooter.Score;
using SpaceShooter.UFOs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        private PlayerUIView _playerUIView;
        private ShooterUIView _shooterUIView;

        private PlayerMovement _playerMovement;
        private Shooter _shooter;

        private GameObject _playerPrefab;

        private PlayerUIPresenter _playerUIPresenter;
        private ShooterUIPresenter _shooterUIPresenter;


        private void Awake()
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
            _playerPrefab = CreatAndName("Player", null, _config.PlayerPrefab);
            _playerMovement = _playerPrefab.GetComponent<PlayerMovement>();
            _shooter = _playerPrefab.GetComponent<Shooter>();
        }

        private void CreateUIPrefabs()
        {
            _playerUIView = CreatAndName<PlayerUIView>("UIPlayer", _canvasGameUITransform, _config.UIPlayerPrefab);

            _shooterUIView = CreatAndName<ShooterUIView>("UIShooter", _canvasGameUITransform, _config.UIShooterPrefab);

            _backgroundInstance = CreatAndName("Background", _canvasGameOverTransform, _config.BackgroundPrefab);

            _scoreManagerUI = CreatAndName<ScoreManagerUI>("ScoreManagerUI", _canvasGameOverTransform, _config.ScoreManagerPrefab);

            Button button = CreatAndName<Button>("ButtonPlayAgain", _canvasGameOverTransform, _config.ButtonPlayAgainPrefab);
            _playerUIView.InitializeRestartButton(button);  
        }

        private T CreatAndName<T>(string prefabName, Transform parentCanvas, T prefabComponent) where T : Component
        {
            GameObject prefabGameObject = prefabComponent.gameObject;
            GameObject instance = Instantiate(prefabGameObject, parentCanvas);
            instance.name = prefabName;
            return instance.GetComponent<T>();
        }

        private GameObject CreatAndName(string prefabName, Transform parentCanvas, GameObject prefab)
        {
            GameObject instance = Instantiate(prefab, parentCanvas);
            instance.name = prefabName;
            return instance;
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

            _scoreManagerUI.Initialize(_scoreManager);

            InitializeShooterUIMVP();

            _playerMovement.Initialize(_playerInput, _pauseGame, _playerUIPresenter);

            _spawnerUFO.Initialize(_ufoPool, _config.MinSizeUFO, _config.MaxSizeUFO, _config.SpawnIntervalUFO, _playerMovement);
            _spawnerAsteroid.Initialize(_asteroidPool, _smallAsteroidPool, _config.MinSizeAsteroid,
                                        _config.MaxSizeAsteroid, _config.MinRotateAsteroid, _config.MaxRotateAsteroid,
                                        _config.SpawnIntervalAsteroid, _playerMovement);

            _shooter.Initialize(_playerMovement, _bulletPool, _lazerPool, _shooterUIPresenter);

         
        }

        private void InitializeShooterUIMVP()
        {
            ShooterUIModel shooterUIModel = new ();
            _shooterUIPresenter = new ShooterUIPresenter(shooterUIModel, _shooterUIView, _shooter);

            _shooterUIView.Init(_shooterUIPresenter);

            PlayerUIModel playerUIModel = new ();
            _playerUIPresenter = new PlayerUIPresenter(playerUIModel, _playerUIView, _pauseGame, _scoreManager, _playerMovement,
                                                    _canvasGameOver, _canvasGame);
            _playerUIView.Init(_playerUIPresenter);
        }

        private void StartGame()
        {
            _playerMovement.SetLive(true);
            _playerUIPresenter.HideGameOverCanvas();
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

            _shooterUIPresenter?.OnShooterDestroyed();
            _playerUIPresenter?.OnPlayerDestroyed();
        }
    }
}

