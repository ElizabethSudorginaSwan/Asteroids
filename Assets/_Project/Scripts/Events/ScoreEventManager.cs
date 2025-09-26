using System;
using System.Data;
using SpaceShooter.ObjectPool;
using SpaceShooter.Player;
using SpaceShooter.Score;
using UnityEngine;

namespace SpaceShooter.Events
{
    public class ScoreEventManager : MonoBehaviour
    {
        public delegate void ScoreManagerCreatedHandler(ScoreManager scoreManager);
        public static event ScoreManagerCreatedHandler OnScoreManagerCreated;

        [field: SerializeField] public BulletPool BulletPool { get; set; }
        [field: SerializeField] public LazerPool LazerPool { get; set; }
        
        private ScoreManager _scoreManager;
        private bool _isInitialized = false;

        private void Awake()
        {
            _scoreManager = new ScoreManager();
        }

        private void Start()
        {
            if (_isInitialized) 
            { 
                return; 
            } 

            Initialize();
        }

        public void Initialize()
        {
            if (_isInitialized) return;

            BulletPool.Initialize(_scoreManager);
            LazerPool.Initialize(_scoreManager);

            OnScoreManagerCreated?.Invoke(_scoreManager);
            _isInitialized = true;
        }
    }
}

