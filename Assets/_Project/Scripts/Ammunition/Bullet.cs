using SpaceShooter.Enemies;
using SpaceShooter.Events;
using SpaceShooter.ObjectPool;
using UnityEngine;

namespace SpaceShooter.Ammunition
{
    public class Bullet : MonoBehaviour 
    {
        private BasePool _pool;
        private IGameEventPublisher _eventPublisher;

        public void Initialize(BasePool pool, IGameEventPublisher eventPublisher)
        {
            _pool = pool;
            _eventPublisher = eventPublisher;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IDestructibleEnemy enemy))
            {
                enemy.HandleBulletHit();
                _pool.ReturnObject(gameObject);
                _eventPublisher.PublishEnemyDestroyed(enemy.ScoreValue);
            }
        }
    }
}

