
namespace SpaceShooter.Events
{
    public interface IGameEventPublisher
    {
        public event System.Action<int> OnEnemyDestroyed;

        public void PublishEnemyDestroyed(int scoreValue);
    }
}

