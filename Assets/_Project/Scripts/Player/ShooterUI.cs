using System.Collections;
using System.Collections.Generic;
using SpaceShooter.Score;
using TMPro;
using UnityEngine;

namespace SpaceShooter.Player
{
    public class ShooterUI : MonoBehaviour
    {
        [field: SerializeField] public TMP_Text LazerShotsT { get; private set; }
        [field: SerializeField] public TMP_Text RechargeT { get; private set; }

        private Shooter _shooter;

        public void Initialize(Shooter shooter)
        {
            _shooter = shooter;

            shooter.OnCountLazerChanged += UpdateLazerCountText;
            UpdateLazerCountText(_shooter.CurrentLazer);

            shooter.OnTimeRecharge += UpdateRechargeTime;
            UpdateRechargeTime(_shooter.RechargeTimer);
        }

        private void OnDestroy()
        {
            _shooter.OnCountLazerChanged -= UpdateLazerCountText;
            _shooter.OnTimeRecharge -= UpdateRechargeTime;
        }

        public void UpdateLazerCountText(int currentLazer)
        {
            LazerShotsT.text = $"{currentLazer}"; 
        }

        public void UpdateRechargeTime(float currentTime)
        {
            RechargeT.text = $"{Mathf.CeilToInt(currentTime)}";
        }
    }
}


