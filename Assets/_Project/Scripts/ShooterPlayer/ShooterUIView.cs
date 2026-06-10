using TMPro;
using UnityEngine;

namespace SpaceShooter.ShooterPlayer
{
    public class ShooterUIView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nowLazerShotsCount;
        [SerializeField] private TMP_Text _rechargeCount;

        private ShooterUIPresenter _presenter;

        public void Init(ShooterUIPresenter presenter)
        {
            _presenter = presenter;
        }

        public void UpdateLazerCountText(int currentLazer)
        {
            _nowLazerShotsCount.text = $"{currentLazer}";
        }

        public void UpdateRechargeTime(float currentTime)
        {
            _rechargeCount.text = $"{Mathf.CeilToInt(currentTime)}";
        }
    }

}
