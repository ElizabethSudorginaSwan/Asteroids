using System.Collections;
using System.Collections.Generic;
using SpaceShooter.ObjectPool;
using SpaceShooter.Player;
using SpaceShooter.Score;
using UnityEngine;

namespace SpaceShooter.Initializer
{
    public class GameInitializer : MonoBehaviour
    {
        [field: SerializeField] public BulletPool BulletPool { get; set; }
        [field: SerializeField] public LazerPool LazerPool { get; set; }
        [field: SerializeField] public PlayerUI PlayerUI { get; set; }
        [field: SerializeField] public ScoreManagerUI ScoreUI { get; set; }

        private ScoreManager _scoreManager;
        private void Awake()
        {
            InitializeGame();
        }
        private void InitializeGame()
        {
            _scoreManager = new ScoreManager();

            BulletPool.Initialize(_scoreManager);
            LazerPool.Initialize(_scoreManager);
            PlayerUI.Initialize(_scoreManager);
            ScoreUI.Initialize(_scoreManager);
        }
    }
}

