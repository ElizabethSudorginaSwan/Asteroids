using UnityEngine;

namespace SpaceShooter.Player
{
    public class PlayerUIPresenter 
    {
        private readonly PlayerUIModel _model;
        private readonly PlayerUIView _view;
        private bool _isSubscribed;

        public PlayerUIPresenter(PlayerUIModel model, PlayerUIView view)
        {
            _model = model;
            _view = view;
           
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            if (_isSubscribed) return;

            _model.OnSpeedChanged += SpeedChanged;
            _model.OnPositionChanged += PositionChanged;
            _model.OnRotationChanged += RotationChanged;
            _isSubscribed = true;
        }

        private void UnsubscribeFromEvents()
        {
            if (!_isSubscribed) return;

            _model.OnSpeedChanged -= SpeedChanged;
            _model.OnPositionChanged -= PositionChanged;
            _model.OnRotationChanged -= RotationChanged;
            _isSubscribed = false;
        }

        private void SpeedChanged(float speed)
        {
            _view.UpdateSpeedText(speed);  
        }

        private void PositionChanged(Vector2 position)
        {
            _view.UpdatePositionText(position);
        }

        private void RotationChanged(float rotation)
        {
            _view.UpdateRotationText(rotation);
        }

        public void OnPlayerDestroyed()
        {
            UnsubscribeFromEvents();
        }

        public void RestartGame()
        {
            SubscribeToEvents();
            _model.RestartGame();
        }

        public void ShowGameOver()
        {
            _model.ShowGameOver();
        }

        public void HideGameOverCanvas()
        {
            _model.HideGameOverCanvas();
        }

        public void UpdatePlayerData()
        {
            _model.UpdatePlayerData();
        }
    }
}

