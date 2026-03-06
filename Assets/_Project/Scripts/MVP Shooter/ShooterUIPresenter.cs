using SpaceShooter.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter.MVPShooter
{
    public class ShooterUIPresenter
    {
        private readonly ShooterUIModel _model;
        private readonly BaseViewShooter _view;
        private readonly Shooter _shooter;

        public ShooterUIPresenter(ShooterUIModel model, BaseViewShooter view, Shooter shooter)
        {
            _model = model;
            _view = view;
            _shooter = shooter;

            _model.SetMaxLazerShots(shooter.MazLazerShots);
            _model.SetRechargeTime(shooter.RechargeTime);
            _model.UpdateLazerCount(model.CurrentLazer);

            _model.UpdateRechargeTime(0f);

            _model.OnCountLazerChanged += HandleLazerCountChanged;
            _model.OnTimeRecharge += HandleRechargeTimeChanged;
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
            _model.OnCountLazerChanged -= HandleLazerCountChanged;
            _model.OnTimeRecharge -= HandleRechargeTimeChanged;
        }
    }
}

