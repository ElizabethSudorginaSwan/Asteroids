using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SpaceShooter.MVPShooter
{
    public class ShooterUIView : BaseViewShooter
    {
        [SerializeField] private TMP_Text _nowLazerShotsCount;
        [SerializeField] private TMP_Text _rechargeCount;

        public override void UpdateLazerCountText(int currentLazer)
        {
            _nowLazerShotsCount.text = $"{currentLazer}";
        }

        public override void UpdateRechargeTime(float currentTime)
        {
            _rechargeCount.text = $"{Mathf.CeilToInt(currentTime)}";
        }
    }

}
