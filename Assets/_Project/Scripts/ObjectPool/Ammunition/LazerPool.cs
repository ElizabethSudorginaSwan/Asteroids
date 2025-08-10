using SpaceShooter.Ammunition;
using SpaceShooter.Events;
using UnityEngine;

namespace SpaceShooter.ObjectPool
{
    public class LazerPool : BasePool
    {
        [field: SerializeField] public IGameEventPublisher EventPublisher {  get; set; }

        public void Initialize(IGameEventPublisher eventPublisher)
        {
            EventPublisher = eventPublisher;
            InitializePool(); 
        }

        protected override void InitializePool()
        {
            base.InitializePool();
            foreach (var lazer in _objectPool)
            {
                if (lazer.TryGetComponent(out Lazer lazerComponent))
                {
                    lazerComponent.Initialize(this, EventPublisher);
                }
            }
        }
    }
}

