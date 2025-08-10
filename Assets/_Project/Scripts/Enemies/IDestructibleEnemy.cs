
namespace SpaceShooter.Enemies
{
    public interface IDestructibleEnemy
    {
        public int ScoreValue { get; }

        public void HandleBulletHit(); 

        public void HandleLazerHit();  
    }
}

