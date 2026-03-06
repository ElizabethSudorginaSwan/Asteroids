using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter.MVPPlayer
{
    public class PlayerUIView : BaseViewPlayer
    {
        [SerializeField] private TMP_Text _speedPlayerCount;
        [SerializeField] private TMP_Text _positionPlayerCount;
        [SerializeField] private TMP_Text _rotationPlayerCount;

        private Button _restartButton;

        public override void InitializeRestartButton(Button button)
        {
            _restartButton = button;
            _restartButton.onClick.AddListener(RestartGame);
        }

        public override void UpdateSpeedText(float speed)
        {
            _speedPlayerCount.text = $"{speed:F2}";
        }

        public override void UpdatePositionText(Vector2 position)
        {
            _positionPlayerCount.text = $"{position.x:F1} | {position.y:F1}";
        }

        public override void UpdateRotationText(float rotation)
        {
            _rotationPlayerCount.text = $"{rotation:F0}°";
        }

        private void RestartGame()
        {
            _presenter.RestartGame();
        }

        private void OnDestroy()
        {
            _restartButton.onClick.RemoveListener(RestartGame);

        }
    }
}


