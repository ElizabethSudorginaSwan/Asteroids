using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter.MVPPlayer
{
    public class PlayerUIModel 
    {
        private float _currentSpeed;
        private Vector2 _currentPosition;
        private float _currentRotation;

        public delegate void SpeedChanged(float speed);
        public event SpeedChanged OnSpeedChanged;

        public delegate void PositionChanged(Vector2 position);
        public event PositionChanged OnPositionChanged;

        public delegate void RotationChanged(float rotation);
        public event RotationChanged OnRotationChanged;

        public float Speed => _currentSpeed;
        public Vector2 Position => _currentPosition;
        public float Rotation => _currentRotation;

        public void UpdateSpeed(float speedUpdate)
        {
            _currentSpeed = speedUpdate;
            OnSpeedChanged?.Invoke(_currentSpeed);
        }

        public void UpdatePosition(Vector2 positionUpdate)
        {
            _currentPosition = positionUpdate;
            OnPositionChanged?.Invoke(_currentPosition);
        }

        public void UpdateRotation(float rotationUpdate)
        {
            _currentRotation = rotationUpdate;
            OnRotationChanged?.Invoke(_currentRotation);
        }
    }
}
