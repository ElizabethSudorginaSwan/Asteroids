using System.Collections;
using System.Collections.Generic;
using SpaceShooter.Asteroids;
using SpaceShooter.UFOs;
using UnityEngine;

namespace SpaceShooter.Pause
{
    public class PauseGame : MonoBehaviour
    {
        [field: SerializeField] public AsteroidSpawner AsteroidSpawner { get; private set; }
        [field: SerializeField] public UFOSpawner UfoSpawner { get; private set; }

        private bool _isPaused = false;

        public void SetPause(bool paused)
        {
            _isPaused = paused;

            AsteroidSpawner.SetPause(paused);
            UfoSpawner.SetPause(paused);
        }

        public bool IsPaused()
        {
            return _isPaused;
        }
    }

}

