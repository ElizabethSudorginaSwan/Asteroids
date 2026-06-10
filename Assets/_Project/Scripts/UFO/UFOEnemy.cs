using UnityEngine;
using SpaceShooter.Enemies;
using SpaceShooter.ObjectPool;

namespace SpaceShooter.UFOs
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class UFOEnemy : MonoBehaviour, IDestructibleEnemy
    {
        [field: SerializeField] public float MoveForce { get; private set; }
        [field: SerializeField] public float MaxSpeed { get; private set; }
        [field: SerializeField] public float Drag { get; private set; }
        [field: SerializeField] public int ScoreValue { get; private set; }

        private Transform _player;
        private IPool _ufoPool;
        private Rigidbody2D _rb;
        private Vector2 _moveDirection;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.drag = Drag;
        }

        private void Update()
        {
            if (_player != null)
            {
                _moveDirection = (_player.position - transform.position).normalized;
            }
        }

        private void FixedUpdate()
        {
            if (_player == null) return;
        
            if (_rb.velocity.magnitude < MaxSpeed)
            {
                _rb.AddForce(_moveDirection * MoveForce, ForceMode2D.Force);
            }
        }

        public void SetUfoPool(IPool ufoPool)
        {
            _ufoPool = ufoPool;
        }
        
        public void SetPlayer(Transform playerTransform)
        {
            _player = playerTransform;
        }

        public void HandleBulletHit() 
        { 
            _ufoPool.ReturnObject(gameObject);
        }

        public void HandleLazerHit() 
        {
            _ufoPool.ReturnObject(gameObject);
        }
    }
}

