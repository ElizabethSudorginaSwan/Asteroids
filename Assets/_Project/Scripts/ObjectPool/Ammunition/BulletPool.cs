using SpaceShooter.Ammunition;
using SpaceShooter.Events;
using UnityEngine;

namespace SpaceShooter.ObjectPool
{
    public class BulletPool : BasePool
    {
        [field: SerializeField] public IGameEventPublisher EventPublisher { get; set; }

        public void Initialize(IGameEventPublisher eventPublisher)
        {
            EventPublisher = eventPublisher;
            InitializePool();
        }

        protected override void InitializePool()
        {
            base.InitializePool();
            foreach (var bullet in _objectPool)
            {
                if (bullet.TryGetComponent(out Bullet bulletComponent))
                {
                    bulletComponent.Initialize(this, EventPublisher);
                }
            }
        }
    }
}
