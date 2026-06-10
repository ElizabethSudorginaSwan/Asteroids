using TMPro;
using UnityEngine;

namespace SpaceShooter.Player
{
    public class PlayerUIView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _speedPlayerCount;
        [SerializeField] private TMP_Text _positionPlayerCount;
        [SerializeField] private TMP_Text _rotationPlayerCount;

        private PlayerUIPresenter _presenter;

        public void Init(PlayerUIPresenter presenter)
        {
            _presenter = presenter;
        }

        public void UpdateSpeedText(float speed)
        {
            _speedPlayerCount.text = $"{speed:F2}";
        }

        public void UpdatePositionText(Vector2 position)
        {
            _positionPlayerCount.text = $"{position.x:F1} | {position.y:F1}";
        }

        public void UpdateRotationText(float rotation)
        {
            _rotationPlayerCount.text = $"{rotation:F0}°";
        }
    }
}


