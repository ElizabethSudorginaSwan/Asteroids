using System.Collections;
using System.Collections.Generic;
using SpaceShooter.ObjectPool;
using SpaceShooter.Player;
using SpaceShooter.Score;
using UnityEngine;

namespace SpaceShooter.Events
{
    public class UIEventManager : MonoBehaviour
    {
        [field: SerializeField] public PlayerUI PlayerUI { get; set; }
        [field: SerializeField] public ScoreManagerUI ScoreUI { get; set; }

        private void Awake()
        {
            ScoreEventManager.OnScoreManagerCreated += OnScoreManagerCreated;
        }

        private void OnScoreManagerCreated(ScoreManager scoreManager)
        {
            if (scoreManager == null)
            {
                Debug.LogError("ScoreManager is null!");
                return;
            }
            ScoreUI.Initialize(scoreManager);
            PlayerUI.Initialize(scoreManager);
        }

        private void OnDestroy()
        {
            ScoreEventManager.OnScoreManagerCreated -= OnScoreManagerCreated;
        }
    }
}

