using SpaceShooter.MVPShooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter.MVPPlayer
{
    public abstract class BaseViewPlayer : MonoBehaviour
    {
        protected PlayerUIPresenter _presenter;

        public void Init(PlayerUIPresenter presenter)
        {
            _presenter = presenter;
        }

        public abstract void UpdateSpeedText(float speed);

        public abstract void UpdatePositionText(Vector2 position);

        public abstract void UpdateRotationText(float rotation);

        public abstract void InitializeRestartButton(Button button);
    }
}

