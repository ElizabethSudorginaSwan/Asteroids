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
        [field: SerializeField] public PlayerMovement PlayerMovement { get; private set; }
        [field: SerializeField] public GameObject CanvasGameOver { get; private set; }
        [field: SerializeField] public GameObject CanvasGame { get; private set; }

        private Button _buttonPlayAgainComponent;
        private ScoreManager _scoreManager;
        private PauseGame _pauseGame;
        private Rigidbody2D _rb;

        public void InitializeButton(Button button)
        {
            _buttonPlayAgainComponent = button;
            _buttonPlayAgainComponent.onClick.AddListener(RestartGame);
    
        }

        public void Initialize(ScoreManager scoreManager, PauseGame pauseGame)
        {
            _scoreManager = scoreManager;
            _pauseGame = pauseGame;
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
            CanvasGameOver.SetActive(true);
            CanvasGame.SetActive(false);
        }

        public void HideGameOver()
        {
            CanvasGameOver.SetActive(false);
            CanvasGame.SetActive(true);
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
            PlayerMovement.SetLive(true);
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


