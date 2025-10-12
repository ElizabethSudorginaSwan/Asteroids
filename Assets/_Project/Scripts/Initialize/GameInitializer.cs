using System.Collections;
using System.Collections.Generic;
using SpaceShooter.Asteroids;
using SpaceShooter.ObjectPool;
using SpaceShooter.Pause;
using SpaceShooter.Player;
using SpaceShooter.Score;
using SpaceShooter.UFOs;
using UnityEngine;

namespace SpaceShooter.Initializer
{
    public class GameInitializer : MonoBehaviour
    {
        [field: SerializeField] public BulletPool BulletPool { get; set; }
        [field: SerializeField] public LazerPool LazerPool { get; set; }
        [field: SerializeField] public PlayerUI PlayerUI { get; set; }
        [field: SerializeField] public ScoreManagerUI ScoreUI { get; set; }
        [field: SerializeField] public PlayerMovement PlayerMovement { get; set; }
        [field: SerializeField] public PauseGame PauseGame { get; set; }
        [field: SerializeField] public Shooter Shooter { get; set; }


        private ScoreManager _scoreManager;

        private PlayerInput _playerInput;

        private void Awake()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            _scoreManager = new ScoreManager();
            _playerInput = new PlayerInput();

            BulletPool.Initialize(_scoreManager);
            LazerPool.Initialize(_scoreManager);
            PlayerUI.Initialize(_scoreManager);
            ScoreUI.Initialize(_scoreManager);

            PlayerMovement.Initialize(_playerInput, PlayerUI, PauseGame);

            Shooter.Initialize(PlayerMovement);

            StartGame();
        }

        private void StartGame()
        {
            PlayerMovement.SetLive(true);
            PlayerUI.HideGameOver();
            _scoreManager.ResetScore();
            PauseGame.SetPause(false);
        }

        private void Update()
        {
            _playerInput.UpdateInput();
        }
    }
}

