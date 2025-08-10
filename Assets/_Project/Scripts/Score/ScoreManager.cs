using SpaceShooter.Events;
using UnityEngine;

namespace SpaceShooter.Score
{
    public class ScoreManager
    {
        private int _currentScore;
        private readonly IGameEventPublisher _eventPublisher;

        public int CurrentScore => _currentScore;

        public event System.Action<int> OnScoreChanged;

        public ScoreManager(IGameEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
            _eventPublisher.OnEnemyDestroyed += AddScore;
        }

        public void Dispose()
        {
            _eventPublisher.OnEnemyDestroyed -= AddScore;
        }

        public void ResetScore()
        {
            _currentScore = 0;
            OnScoreChanged.Invoke(_currentScore);
        }

        private void AddScore(int scoreValue)
        {
            _currentScore += scoreValue;
            Debug.Log(_currentScore);
            OnScoreChanged.Invoke(_currentScore);
        }
    }
}
