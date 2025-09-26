using SpaceShooter.Ammunition;
using SpaceShooter.Events;
using SpaceShooter.Score;
using UnityEngine;

namespace SpaceShooter.ObjectPool
{
    public class BulletPool : BasePool
    {
        [field: SerializeField] public ScoreManager ScoreManager {  get; set; }

        public void Initialize(ScoreManager scoreManager)
        {
            ScoreManager = scoreManager;
            InitializePool();
        }

        protected override void InitializePool()
        {
            base.InitializePool();
            foreach (var bullet in _objectPool)
            {
                if (bullet.TryGetComponent(out Bullet bulletComponent))
                {
                    bulletComponent.Initialize(this, ScoreManager);
                }
            }
        }
    }
}
