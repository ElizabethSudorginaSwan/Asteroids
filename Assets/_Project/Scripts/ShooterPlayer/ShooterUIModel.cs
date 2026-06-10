namespace SpaceShooter.ShooterPlayer
{
    public class ShooterUIModel 
    {
        private int _currentLazerShots;
        private float _remainingTime;
        private readonly int _maxLazerShots;
        private readonly float _rechargeTime;

        private readonly Shooter _shooter;

        public delegate void LazerShot(int lazerCount);
        public event LazerShot OnCountLazerChanged;

        public delegate void TimeRecharge(float timeCount);
        public event TimeRecharge OnTimeRecharge;

        public int CurrentLazer => _currentLazerShots;

        public ShooterUIModel(Shooter shooter)
        {
            _shooter = shooter;
            _maxLazerShots = shooter.MazLazerShots;
            _rechargeTime = shooter.RechargeTime;
            UpdateLazerCount(_maxLazerShots);
            UpdateRechargeTime(0f);
        }

        public void UpdateLazerCount(int count)
        {
            _currentLazerShots = count;
            OnCountLazerChanged?.Invoke(_currentLazerShots);
        }

        public void UpdateRechargeTime(float time)
        {
            _remainingTime = time;
            OnTimeRecharge?.Invoke(_remainingTime);
        }
    }
}

