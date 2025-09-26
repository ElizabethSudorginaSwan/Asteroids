using SpaceShooter.Events;
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
        [field: SerializeField] public TMP_Text SpeedText { get; private set; }
        [field: SerializeField] public TMP_Text PositionText { get; private set; }
        [field: SerializeField] public TMP_Text RotationText { get; private set; }
        [field: SerializeField] public PlayerMovement PlayerMovement { get; private set; }
        [field: SerializeField] public GameObject GameOverScreen { get; private set; }
        [field: SerializeField] public GameObject GameScreen { get; private set; }
        [field: SerializeField] public Button RestartButton { get; private set; }
        [field: SerializeField] public PauseGame PauseGame { get; private set; }

        private ScoreManager _scoreManager;
        private Rigidbody2D _rb;
        
        private void Awake()
        {
            RestartButton.onClick.AddListener(RestartGame);
        }

        public void Initialize(ScoreManager scoreManager)
        {
            _scoreManager = scoreManager;
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
            GameOverScreen.SetActive(true);
            GameScreen.SetActive(false);
        }

        public void HideGameOver()
        {
            GameOverScreen.SetActive(false);
            GameScreen.SetActive(true);
        }


        private void OnDestroy()
        {
            RestartButton.onClick.RemoveListener(RestartGame);
        }
        
        private void RestartGame()
        {
            PauseGame.SetPause(false);
            HideGameOver();
            _scoreManager.ResetScore();
            PlayerMovement.SetLive(true);
        }

        private void UpdateSpeed()
        {
            float speed = _rb.velocity.magnitude;
            SpeedText.text = $"{speed:F2}";
        }

        private void UpdatePosition()
        {
            Vector2 pos = _rb.position;
            PositionText.text = $"{pos.x:F1} | {pos.y:F1}";
        }

        private void UpdateRotation()
        {
            float angle = _rb.rotation;
            RotationText.text = $"{angle:F0}°";
        }
    }
}


