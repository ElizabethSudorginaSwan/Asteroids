using SpaceShooter.Pause;
using SpaceShooter.Score;
using UnityEngine;

namespace SpaceShooter.Player
{
    public class PlayerUIModel 
    {
        private float _currentSpeed;
        private Vector2 _currentPosition;
        private float _currentRotation;

        private readonly PauseGame _pauseGame;
        private readonly ScoreManager _scoreManager;
        private readonly PlayerMovement _playerMovement;
        private readonly GameObject _canvasGameOver;
        private readonly GameObject _canvasGame;

        public delegate void SpeedChanged(float speed);
        public event SpeedChanged OnSpeedChanged;

        public delegate void PositionChanged(Vector2 position);
        public event PositionChanged OnPositionChanged;

        public delegate void RotationChanged(float rotation);
        public event RotationChanged OnRotationChanged;

        public PlayerUIModel(PauseGame pauseGame, ScoreManager scoreManager,
                            PlayerMovement playerMovement, GameObject canvasGameOver, GameObject canvasGame)
        {
            _pauseGame = pauseGame;
            _scoreManager = scoreManager;
            _playerMovement = playerMovement;
            _canvasGameOver = canvasGameOver;
            _canvasGame = canvasGame;
        }

        public void UpdateSpeed(float speedUpdate)
        {
            _currentSpeed = speedUpdate;
            OnSpeedChanged?.Invoke(_currentSpeed);
        }

        public void UpdatePosition(Vector2 positionUpdate)
        {
            _currentPosition = positionUpdate;
            OnPositionChanged?.Invoke(_currentPosition);
        }

        public void UpdateRotation(float rotationUpdate)
        {
            _currentRotation = rotationUpdate;
            OnRotationChanged?.Invoke(_currentRotation);
        }

        public void RestartGame()
        {
            _pauseGame.SetPause(false);
            _scoreManager.ResetScore();
            _playerMovement.SetLive(true);

            SetGameOverCanvasState(false);
            UpdatePlayerData();
        }

        public void ShowGameOver()
        {
            SetGameOverCanvasState(true);
        }

        public void UpdatePlayerData()
        {
            Rigidbody2D rb = _playerMovement.GetComponent<Rigidbody2D>();
            UpdateSpeed(rb.velocity.magnitude);
            UpdatePosition(rb.position);
            UpdateRotation(rb.rotation);
        }

        public void SetGameOverCanvasState(bool isGameOver)
        {
            _canvasGameOver.SetActive(isGameOver);
            _canvasGame.SetActive(!isGameOver);
        }

        public void HideGameOverCanvas()
        {
            SetGameOverCanvasState(false);
        }
    }
}
