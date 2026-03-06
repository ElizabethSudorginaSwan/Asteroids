using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

namespace SpaceShooter.MVPShooter
{
    public class ShooterUIModel 
    {
        private int _currentLazerShots;
        private float _remainingTime;
        private int _maxLazerShots;
        private float _rechargeTime;

        public delegate void LazerShot(int lazerCount);
        public event LazerShot OnCountLazerChanged;

        public delegate void TimeRecharge(float timeCount);
        public event TimeRecharge OnTimeRecharge;

        public int CurrentLazer => _currentLazerShots;

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

        public void SetMaxLazerShots(int max)
        {
            _maxLazerShots = max;
        }

        public void SetRechargeTime(float time)
        {
            _rechargeTime = time;
        }
    }
}

