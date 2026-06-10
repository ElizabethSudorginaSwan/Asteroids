namespace SpaceShooter.ShooterPlayer
{
    public class ShooterUIPresenter
    {
        private readonly ShooterUIModel _model;
        private readonly ShooterUIView _view;

        public ShooterUIPresenter(ShooterUIModel model, ShooterUIView view)
        {
            _model = model;
            _view = view;
            
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            _model.OnCountLazerChanged += HandleLazerCountChanged;
            _model.OnTimeRecharge += HandleRechargeTimeChanged;
        }

        private void UnsubscribeFromEvents()
        {
            _model.OnCountLazerChanged -= HandleLazerCountChanged;
            _model.OnTimeRecharge -= HandleRechargeTimeChanged;
        }

        private void HandleLazerCountChanged(int count)
        {
            _view.UpdateLazerCountText(count);
        }

        private void HandleRechargeTimeChanged(float time)
        {
            _view.UpdateRechargeTime(time);
        }

        public void UpdateLazerCount(int count)
        {
            _model.UpdateLazerCount(count);
        }

        public void UpdateRechargeTime(float time)
        {
            _model.UpdateRechargeTime(time);
        }

        public void OnShooterDestroyed()
        {
            UnsubscribeFromEvents();
        }
    }
}

