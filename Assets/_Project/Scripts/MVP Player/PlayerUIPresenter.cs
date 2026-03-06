using SpaceShooter.Pause;
using SpaceShooter.Player;
using SpaceShooter.Score;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter.MVPPlayer
{
    public class PlayerUIPresenter 
    {
        private readonly PlayerUIModel _model;
        private readonly PlayerUIView _view;

        private readonly PauseGame _pauseGame;
        private readonly ScoreManager _scoreManager;
        private readonly PlayerMovement _playerMovement;

        private readonly GameObject _canvasGameOver;
        private readonly GameObject _canvasGame;

        public PlayerUIPresenter(PlayerUIModel model, PlayerUIView view, PauseGame pauseGame, ScoreManager scoreManager,
                                    PlayerMovement playerMovement, GameObject canvasGameOver, GameObject canvasGame)
        {
            _model = model;
            _view = view;
            _pauseGame = pauseGame;
            _scoreManager = scoreManager;
            _playerMovement = playerMovement;
            _canvasGameOver = canvasGameOver;
            _canvasGame = canvasGame;

            SubscribeToEvents();

            _playerMovement = playerMovement;
        }

        private void SubscribeToEvents()
        {
            _model.OnSpeedChanged += SpeedChanged;
            _model.OnPositionChanged += PositionChanged;
            _model.OnRotationChanged += RotationChanged;
        }

        private void UnsubscribeFromEvents()
        {
            _model.OnSpeedChanged -= SpeedChanged;
            _model.OnPositionChanged -= PositionChanged;
            _model.OnRotationChanged -= RotationChanged;
        }

        private void SpeedChanged(float speed)
        {
            _view.UpdateSpeedText(_model.Speed);
           
        }

        private void PositionChanged(Vector2 position)
        {
            _view.UpdatePositionText(_model.Position);
        }

        private void RotationChanged(float rotation)
        {
            _view.UpdateRotationText(_model.Rotation);
        }

        public void OnPlayerDestroyed()
        {
            UnsubscribeFromEvents();
        }

        public void RestartGame()
        {
            SubscribeToEvents();
            _pauseGame.SetPause(false);
            HideGameOverCanvas();
            _scoreManager.ResetScore();
            _playerMovement.SetLive(true);
        }

        public void ShowGameOver()
        { 
            ShowGameOverCanvas();
        }

        public void UpdatePlayerData()
        {
            Rigidbody2D rb = _playerMovement.GetComponent<Rigidbody2D>();

            _model.UpdateSpeed(rb.velocity.magnitude);
            _model.UpdatePosition(rb.position);
            _model.UpdateRotation(rb.rotation);
        }

        public void ShowGameOverCanvas()
        {
            _canvasGameOver.SetActive(true);
            _canvasGame.SetActive(false);
        }

        public void HideGameOverCanvas()
        {
            _canvasGameOver.SetActive(false);
            _canvasGame.SetActive(true);
        }
    }
}

