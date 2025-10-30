using SpaceShooter.Ammunition;
using SpaceShooter.Score;
using UnityEngine;

namespace SpaceShooter.ObjectPool
{
    public class LazerPool : BasePool
    {
        private ScoreManager _scoreManager;

        public void Initialize(GameObject[] prefabs, int poolSize, ScoreManager scoreManager, Transform parent = null)
        {
            _scoreManager = scoreManager;

            base.Initialize(prefabs, poolSize, parent);
            InitializePool();
        }
        protected override void InitializePool()
        {
            base.InitializePool();
            foreach (var lazer in _objectPool)
            {
                if (lazer.TryGetComponent(out Lazer lazerComponent))
                {
                    lazerComponent.Initialize(this, _scoreManager);
                }
            }
        }
    }
}

