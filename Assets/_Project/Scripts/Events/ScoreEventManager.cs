using SpaceShooter.ObjectPool;
using SpaceShooter.Player;
using SpaceShooter.Score;
using UnityEngine;

namespace SpaceShooter.Events
{
    public class ScoreEventManager : MonoBehaviour
    {
        [field: SerializeField] public LazerPool LazerPool {  get; set; }
        [field: SerializeField] public BulletPool BulletPool {  get; set; }
        [field: SerializeField] public ScoreManagerUI ScoreManagerUI {  get; set; }
        [field: SerializeField] public PlayerUI PlayerUI {  get; set; }

        private IGameEventPublisher _eventPublisher;
        private ScoreManager _scoreManager;

        private void Awake()
        {
            _eventPublisher = new GameEventPublisher();
            _scoreManager = new ScoreManager(_eventPublisher);

            PlayerUI.Initialize(_scoreManager);
            LazerPool.Initialize(_eventPublisher);
            BulletPool.Initialize(_eventPublisher);
            ScoreManagerUI.Initialize(_scoreManager);
        }

        private void OnDestroy()
        {
            _scoreManager.Dispose();
        }
    }
}

