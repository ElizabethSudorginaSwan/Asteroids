using UnityEngine;
using SpaceShooter.Asteroids;
using SpaceShooter.UFOs;
using SpaceShooter.Enemies;
using SpaceShooter.Pause;

namespace SpaceShooter.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Shooter))]
    public class PlayerMovement : MonoBehaviour
    {
        [field: SerializeField] public float MoveForce { get; private set; } 
        [field: SerializeField] public float MaxSpeed { get; private set; } 
        [field: SerializeField] public float Drag { get; private set; } 
        [field: SerializeField] public float RotationSpeed { get; private set; } 
        [field: SerializeField] public bool Live { get; private set; }
        [field: SerializeField] public PauseGame PauseGame { get; private set; }

        private Rigidbody2D _rb;
        private bool _shouldMoveForward;
        private float _rotationDirection;
        private Shooter _shooter;
        private PlayerInput _playerInput;
        private PlayerUI _playerUI;

        private void Awake()
        {
            _playerInput = new PlayerInput();
            _shooter = GetComponent<Shooter>();
            _playerUI = GetComponent<PlayerUI>();
            _rb = GetComponent<Rigidbody2D>();
            _rb.drag = Drag;

            if (TryGetComponent(out _playerUI))
            {
                _playerUI.SetRbPlayer(_rb);
            }

            SetLive(true);
        }

        private void Update()
        {
            if (PauseGame.IsPaused()) return;

            _playerInput.UpdateInput();

            if (_playerInput.MoveInput > 0)
            {
                Move();
            }

            Rotate(_playerInput.RotateInput);

            if (_playerInput.ShootBulletPressed)
            {
                _shooter.ShootBullet();
            }

            if (_playerInput.ShootLazerPressed)
            {
                _shooter.ShootLazer();
            }
        }

        private void FixedUpdate()
        {
            if (PauseGame.IsPaused()) return;

            if (_shouldMoveForward)
            {
                Vector2 forward = transform.up;

                if (Vector2.Dot(_rb.velocity, forward) < MaxSpeed)
                {
                    _rb.AddForce(forward * MoveForce, ForceMode2D.Force);
                }
            }

            if (_rotationDirection != 0)
            {
                _rb.MoveRotation(_rb.rotation + _rotationDirection * RotationSpeed * Time.fixedDeltaTime);
            }

            _shouldMoveForward = false;

            _playerUI.UpdateUI();
        }

        public void Move()
        {
            _shouldMoveForward = true;
        }

        public void Rotate(int direction)
        {
            _rotationDirection = direction;
        }

        public void SetLive(bool newLiveState)
        {
            Live = newLiveState;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IDestructibleEnemy>(out _))
            {
                ResetPlayer();
                _playerUI.UpdateUI();
                SetLive(false);
                PauseGame.SetPause(true);
                _playerUI.ShowGameOver();
            }
        }

        private void ResetPlayer()
        {
            _rb.velocity = Vector2.zero;
            _rb.angularVelocity = 0f;
            transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
    }
}
