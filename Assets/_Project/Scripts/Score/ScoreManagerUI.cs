using UnityEngine;
using TMPro;

namespace SpaceShooter.Score
{
    public class ScoreManagerUI : MonoBehaviour
    {
        [field: SerializeField] public TMP_Text FinalScoreText { get; set; }

        private ScoreManager _scoreManager;

        public void Initialize(ScoreManager scoreManager)
        {
            _scoreManager = scoreManager;
            scoreManager.OnScoreChanged += UpdateFinalTextScore;
            UpdateFinalTextScore(_scoreManager.CurrentScore);
        }

        private void OnDestroy()
        {
            _scoreManager.OnScoreChanged -= UpdateFinalTextScore;
        }

        private void UpdateFinalTextScore(int currentScore)
        {
            FinalScoreText.text = $"{currentScore}";
        }
    }
}
