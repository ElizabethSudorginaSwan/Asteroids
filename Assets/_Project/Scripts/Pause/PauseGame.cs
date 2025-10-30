using System.Collections;
using System.Collections.Generic;
using SpaceShooter.Asteroids;
using SpaceShooter.Initializer;
using SpaceShooter.UFOs;
using UnityEngine;

namespace SpaceShooter.Pause
{
    public class PauseGame
    {
        private bool _isPaused = false;

        public void SetPause(bool paused)
        {
            _isPaused = paused;
        }

        public bool IsPaused()
        {
            return _isPaused;
        }
    }
}

