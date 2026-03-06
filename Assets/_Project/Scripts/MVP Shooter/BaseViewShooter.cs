using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter.MVPShooter
{
    public abstract class BaseViewShooter : MonoBehaviour
    {
        protected ShooterUIPresenter _presenter;

        public void Init(ShooterUIPresenter presenter)
        {
            _presenter = presenter;
        }

        public abstract void UpdateLazerCountText(int currentLazer);
        public abstract void UpdateRechargeTime(float currentTime);
    }

}
