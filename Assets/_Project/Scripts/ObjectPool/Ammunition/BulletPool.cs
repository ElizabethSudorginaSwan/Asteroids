using SpaceShooter.Ammunition;
using SpaceShooter.Score;
using UnityEngine;

namespace SpaceShooter.ObjectPool
{
    public class BulletPool : BasePool<Bullet>
    {
        private ScoreManager _scoreManager;

        public void Initialize(Bullet[] prefabs, int poolSize, ScoreManager scoreManager, Transform parent = null)
        {
            _scoreManager = scoreManager;

            base.Initialize(prefabs, poolSize, parent);
            InitializePool();
        }

        protected override void InitializePool()
        {
            base.InitializePool();
            foreach (var bullet in _objectPool)
            {
                bullet.Initialize(this, _scoreManager);
                //if (bullet.TryGetComponent(out Bullet bulletComponent))
                //{
                //    bulletComponent.Initialize(this, _scoreManager);
                //}
            }
        }
    }
}
