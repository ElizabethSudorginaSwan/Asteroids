using SpaceShooter.Ammunition;
using SpaceShooter.Score;
using UnityEngine;

namespace SpaceShooter.ObjectPool
{
    public class LazerPool : BasePool
    {
        [field: SerializeField] public ScoreManager ScoreManager { get; set; }

        public void Initialize(ScoreManager scoreManager)
        {
            ScoreManager = scoreManager;
            InitializePool();
        }
        protected override void InitializePool()
        {
            base.InitializePool();
            foreach (var lazer in _objectPool)
            {
                if (lazer.TryGetComponent(out Lazer lazerComponent))
                {
                    lazerComponent.Initialize(this, ScoreManager);
                }
            }
        }
    }
}

