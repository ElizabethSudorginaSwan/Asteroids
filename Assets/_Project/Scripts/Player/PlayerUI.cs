using SpaceShooter.Initializer;
using SpaceShooter.ObjectPool;
using SpaceShooter.Pause;
using SpaceShooter.Score;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter.Player
{
    public class PlayerUI : MonoBehaviour
    {
        [field: SerializeField] public TMP_Text SpeedPlayerCount { get; private set; }
        [field: SerializeField] public TMP_Text PositionPlayerCount { get; private set; }
        [field: SerializeField] public TMP_Text RotationPlayerCount { get; private set; }

        private ScoreManager _scoreManager;
        private PauseGame _pauseGame;
        private PlayerMovement _playerMovement;

        private GameObject _canvasGameOver;
        private GameObject _canvasGame;

        private Button _buttonPlayAgainComponent;
        private Rigidbody2D _rb;

        public void InitializeButton(Button button)
        {
            _buttonPlayAgainComponent = button;
            _buttonPlayAgainComponent.onClick.AddListener(RestartGame);
        }

        public void Initialize(ScoreManager scoreManager, PauseGame pauseGame, GameObject canvasGameOver,
                                GameObject canvasGame, PlayerMovement playerMovement)
        {
            _scoreManager = scoreManager;
            _pauseGame = pauseGame;
            _canvasGameOver = canvasGameOver;
            _canvasGame = canvasGame;
            _playerMovement = playerMovement;
        }

        public void SetRbPlayer(Rigidbody2D playerRb)
        {
            _rb = playerRb;
        }

        public void UpdateUI()
        {
            UpdateSpeed();
            UpdatePosition();
            UpdateRotation();
        }

        public void ShowGameOver()
        {
            _canvasGameOver.SetActive(true);
            _canvasGame.SetActive(false);
        }

        public void HideGameOver()
        {
            _canvasGameOver.SetActive(false);
            _canvasGame.SetActive(true);
        }

        private void OnDestroy()
        {
            _buttonPlayAgainComponent.onClick.RemoveListener(RestartGame);

        }

        private void RestartGame()
        {
            _pauseGame.SetPause(false);
            HideGameOver();
            _scoreManager.ResetScore();
            _playerMovement.SetLive(true);
        }

        private void UpdateSpeed()
        {
            float speed = _rb.velocity.magnitude;
            SpeedPlayerCount.text = $"{speed:F2}";
        }

        private void UpdatePosition()
        {
            Vector2 pos = _rb.position;
            PositionPlayerCount.text = $"{pos.x:F1} | {pos.y:F1}";
        }

        private void UpdateRotation()
        {
            float angle = _rb.rotation;
            RotationPlayerCount.text = $"{angle:F0}°";
        }
    }
}


