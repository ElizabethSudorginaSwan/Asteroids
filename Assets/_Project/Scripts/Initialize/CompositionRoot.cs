using SpaceShooter.Asteroids;
using SpaceShooter.Player;
using SpaceShooter.ShooterPlayer;
using SpaceShooter.ObjectPool;
using SpaceShooter.Pause;
using SpaceShooter.Score;
using SpaceShooter.UFOs;
using UnityEngine;

namespace SpaceShooter.Initializer
{
    public class CompositionRoot : MonoBehaviour
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

        public ScoreManager ScoreManager { get; private set; }
        public PlayerInput PlayerInput { get; private set; }
        public UFOSpawner SpawnerUFO { get; private set; }
        public AsteroidSpawner SpawnerAsteroid { get; private set; }
        public PauseGame PauseGame { get; private set; }

        private BulletPool _bulletPool;
        private LazerPool _lazerPool;
        private UFOPool _ufoPool;
        private AsteroidPool _asteroidPool;
        private SmallAsteroidPool _smallAsteroidPool;

        private GameObject _backgroundInstance;
        private ScoreManagerUI _scoreManagerUI;

        private PlayerUIView _playerUIView;
        private ShooterUIView _shooterUIView;
        private PlayerGameOverUIView _gameOverUIView;

        public PlayerMovement PlayerMovement { get; private set; }
        private Shooter _shooter;

        public PlayerUIPresenter PlayerUIPresenter { get; private set; }
        private ShooterUIPresenter _shooterUIPresenter;

        private void Awake()
        {
            CreatePlayerPrefab();
            CreateUIPrefabs();
            CreatePoolContainers();
            InitializePools();
            InitializeManagers();
        }

        private void CreatePlayerPrefab()
        {
            PlayerMovement = CreatAndName<PlayerMovement>("Player", null, _config.PlayerPrefab);
            _shooter = PlayerMovement.GetComponent<Shooter>();
        }

        private void CreateUIPrefabs()
        {
            _playerUIView = CreatAndName<PlayerUIView>("UIPlayer", _canvasGameUITransform, _config.UIPlayerPrefab);

            _shooterUIView = CreatAndName<ShooterUIView>("UIShooter", _canvasGameUITransform, _config.UIShooterPrefab);

            _backgroundInstance = CreatAndName("Background", _canvasGameOverTransform, _config.BackgroundPrefab);

            _scoreManagerUI = CreatAndName<ScoreManagerUI>("ScoreManagerUI", _canvasGameOverTransform, _config.ScoreManagerPrefab);

            _gameOverUIView = CreatAndName<PlayerGameOverUIView>("GameOverUI", _canvasGameOverTransform, _config.GameOverUIPrefab);

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
            GameObject container = new(containerName);
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
            ScoreManager = new ScoreManager();

            _bulletPool.Initialize(_config.BulletPrefabs, _config.BulletPoolSize, ScoreManager, _bulletPoolParent);
            _lazerPool.Initialize(_config.LazerPrefabs, _config.LazerPoolSize, ScoreManager, _lazerPoolParent);

            PlayerInput = new PlayerInput();
            SpawnerUFO = new UFOSpawner();
            SpawnerAsteroid = new AsteroidSpawner();
            PauseGame = new PauseGame();

            _scoreManagerUI.Initialize(ScoreManager);

            InitializeShooterUIMVP();

            PlayerMovement.Initialize(PlayerInput, PauseGame, PlayerUIPresenter);

            SpawnerUFO.Initialize(_ufoPool, _config.MinSizeUFO, _config.MaxSizeUFO, _config.SpawnIntervalUFO, PlayerMovement);
            SpawnerAsteroid.Initialize(_asteroidPool, _smallAsteroidPool, _config.MinSizeAsteroid,
                                        _config.MaxSizeAsteroid, _config.MinRotateAsteroid, _config.MaxRotateAsteroid,
                                        _config.SpawnIntervalAsteroid, PlayerMovement);

            _shooter.Initialize(PlayerMovement, _bulletPool, _lazerPool, _shooterUIPresenter);


        }

        private void InitializeShooterUIMVP()
        {
            ShooterUIModel shooterUIModel = new(_shooter);
            _shooterUIPresenter = new ShooterUIPresenter(shooterUIModel, _shooterUIView);

            _shooterUIView.Init(_shooterUIPresenter);

            PlayerUIModel playerUIModel = new(PauseGame, ScoreManager, PlayerMovement, _canvasGameOver, _canvasGame);
            PlayerUIPresenter = new PlayerUIPresenter(playerUIModel, _playerUIView);
            _playerUIView.Init(PlayerUIPresenter);

            _gameOverUIView.Init(PlayerUIPresenter);
        }

         private void OnDestroy()
        {
            _ufoPool?.ClearPool();
            _asteroidPool?.ClearPool();
            _smallAsteroidPool?.ClearPool();
            _bulletPool?.ClearPool();
            _lazerPool?.ClearPool();

            _shooterUIPresenter?.OnShooterDestroyed();
            PlayerUIPresenter?.OnPlayerDestroyed();
        }
    }
}

