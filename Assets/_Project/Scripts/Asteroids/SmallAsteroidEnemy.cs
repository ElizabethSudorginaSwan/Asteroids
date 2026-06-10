using System.Collections.Generic;
using UnityEngine;
using SpaceShooter.Player;
using SpaceShooter.Enemies;
using SpaceShooter.ObjectPool;

namespace SpaceShooter.Asteroids
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SmallAsteroidEnemy : MonoBehaviour, IDestructibleEnemy
    {
        [field: SerializeField] public float InitialImpulseForce { get; private set; } 
        [field: SerializeField] public float MoveForce { get; private set; }
        [field: SerializeField] public float MaxSpeed { get; private set; }
        [field: SerializeField] public float Drag { get; private set; }
        [field: SerializeField] public float RotationSpeed { get; private set; }
        [field: SerializeField] public int CountScoreSmallAsteroid { get; private set; }
        [field: SerializeField] public int ScoreValue { get; private set; }

        private IPool _smallAsteroidPool;
        private List<GameObject> _parentList;
        private PlayerMovement _playerMovement;
        private Rigidbody2D _rb;
        private Vector2 _randomDirection;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.drag = Drag;
        }

        private void Start()
        {
            _randomDirection = Random.insideUnitCircle.normalized;
            _rb.AddForce(_randomDirection * InitialImpulseForce, ForceMode2D.Impulse);
        }

        private void Update()
        {
            if (!_playerMovement.Live)
            {
                HandleDestruction();
            }
        }

        private void FixedUpdate()
        {
            if (_rb.velocity.magnitude < MaxSpeed)
            {
                _rb.AddForce(_randomDirection * MoveForce, ForceMode2D.Force);
            }
        }

        public void SetSmallAsteroidPool(IPool smallAsteroidPool)
        {
            _smallAsteroidPool = smallAsteroidPool;
        }

        public void SetPlayer(PlayerMovement player)
        {
            _playerMovement = player;
        }

        public void SetParentList(List<GameObject> parentList)
        {
            _parentList = parentList;
        }

        public void HandleBulletHit() 
        {
            HandleDestruction();
        }

        public void HandleLazerHit() 
        {
            HandleDestruction();
        }

        private void HandleDestruction()
        {
            if (_parentList != null && _parentList.Contains(gameObject))
            {
                _parentList.Remove(gameObject);
            }

            _smallAsteroidPool.ReturnObject(gameObject);
        }
    }
}   


