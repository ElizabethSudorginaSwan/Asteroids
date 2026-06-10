using SpaceShooter.Enemies;
using SpaceShooter.ObjectPool;
using SpaceShooter.Score;
using UnityEngine;

namespace SpaceShooter.Ammunition
{
    public class Lazer : MonoBehaviour 
    {
        private BasePool<Lazer> _pool;
        private ScoreManager _scoreManager;

        public void Initialize(BasePool<Lazer> pool, ScoreManager scoreManager)
        {
            _pool = pool;
            _scoreManager = scoreManager;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IDestructibleEnemy enemy))
            {
                enemy.HandleLazerHit();
                _pool.ReturnObject(this);
                _scoreManager.HitEnemy(enemy.ScoreValue);
            }
        }
    }
}


