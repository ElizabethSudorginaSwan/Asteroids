
namespace SpaceShooter.Events
{
    public class GameEventPublisher : IGameEventPublisher
    {
        public event System.Action<int> OnEnemyDestroyed;

        public void PublishEnemyDestroyed(int scoreValue)
        {
            OnEnemyDestroyed.Invoke(scoreValue);
        }
    }
}


