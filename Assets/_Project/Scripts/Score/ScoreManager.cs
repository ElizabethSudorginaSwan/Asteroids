using System;
using UnityEngine;

namespace SpaceShooter.Score
{
    public class ScoreManager
    {
        private int _currentScore;
        public int CurrentScore => _currentScore;

        public delegate void HitByAmmunition(int score);
        public event HitByAmmunition OnScoreChanged;
      
        public void HitEnemy(int scoreValue)
        {
            _currentScore += scoreValue;
            Debug.Log(_currentScore);
            OnScoreChanged?.Invoke(_currentScore);
        }
        public void ResetScore()
        {
            _currentScore = 0;
            OnScoreChanged?.Invoke(_currentScore);
        }
    }
}
