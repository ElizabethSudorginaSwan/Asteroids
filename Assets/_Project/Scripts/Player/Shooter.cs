using UnityEngine;
using TMPro;
using System.Collections.Generic;
using SpaceShooter.Factories;
using SpaceShooter.ObjectPool;
using System.Collections;
using SpaceShooter.Ammunition;

namespace SpaceShooter.Player
{
    [RequireComponent(typeof(PlayerMovement))]
    public class Shooter : MonoBehaviour
    {
        [field: SerializeField] public Transform FirePoint { get; private set; }
        [field: SerializeField] public float SpeedFire { get; private set; }
        [field: SerializeField] public float BlLifetime { get; private set; }
        [field: SerializeField] public float RechargeTime { get; private set; }
        [field: SerializeField] public int MazLazerShots { get; private set; }

        public delegate void LazerShot(int lazerCount);
        public event LazerShot OnCountLazerChanged;
        public int CurrentLazer => _currentLazerShots;

        public delegate void TimeRecharge(float timeCount);
        public event TimeRecharge OnTimeRecharge;
        public float RechargeTimer => _remainingTime;

        private int _currentLazerShots;
        private float _rechargeTimer;
        private bool _isRecharging;
        private float _remainingTime;

        private readonly List<GameObject> _bulletLazerList = new();
        private PlayerMovement _playerMovement;
        private BulletPool _bulletPool;
        private LazerPool _lazerPool;
        private GenericAmmunitionFactory _bulletFactory;
        private GenericAmmunitionFactory _lazerFactory;

        public void Initialize(PlayerMovement playerMovement, BulletPool bulletPool, LazerPool lazerPool)
        {
            _playerMovement = playerMovement;
            _bulletPool = bulletPool;
            _lazerPool = lazerPool;

            _bulletFactory = new GenericAmmunitionFactory(_bulletPool, SpeedFire, FirePoint);
            _lazerFactory = new GenericAmmunitionFactory(_lazerPool, SpeedFire, FirePoint);

            _currentLazerShots = MazLazerShots;
            UpdateLazerShots();
            UpdateRecharge();
        }

        private void Update()
        {
            if (!_playerMovement.Live)
            {
                ClearAllbulletLazer();
                _isRecharging = false;
                _currentLazerShots = MazLazerShots;
                _rechargeTimer = 0;
                UpdateLazerShots();
                UpdateRecharge();
            }

            if (_isRecharging)
            {
                _rechargeTimer += Time.deltaTime;
                UpdateRecharge();

                if (_rechargeTimer >= RechargeTime)
                {
                    _isRecharging = false;
                    _currentLazerShots = MazLazerShots;
                    _rechargeTimer = 0;

                    UpdateLazerShots();
                    UpdateRecharge();
                }
            }
        }

        public void ShootBullet()
        {
            GameObject currentBullet = _bulletFactory.CreateAmmunition(FirePoint.position, Quaternion.identity);
            _bulletLazerList.Add(currentBullet);

            StartCoroutine(ReturnBulletToPoolAfterTime(currentBullet, BlLifetime));
        }

        public void ShootLazer()
        {
            if (_isRecharging)
            {
                return;
            }

            if (_currentLazerShots <= 0)
            {
                StartRecharge();
                return;
            }

            GameObject currentLazer = _lazerFactory.CreateAmmunition(FirePoint.position, FirePoint.rotation);
            _bulletLazerList.Add(currentLazer);

            StartCoroutine(ReturnLazerToPoolAfterTime(currentLazer, BlLifetime));

            _currentLazerShots--;

            if (_currentLazerShots <= 0)
            {
                StartRecharge();
            }

            UpdateLazerShots();
            UpdateRecharge();
        }

        private IEnumerator ReturnBulletToPoolAfterTime(GameObject bullet, float delay)
        {
            yield return new WaitForSeconds(delay); 
            _bulletPool.ReturnObject(bullet); 
        }

        private IEnumerator ReturnLazerToPoolAfterTime(GameObject lazer, float delay)
        {
            yield return new WaitForSeconds(delay); 
            _lazerPool.ReturnObject(lazer); 
        }

        private void StartRecharge()
        {
            _isRecharging = true;
            _rechargeTimer = 0;

            UpdateLazerShots();
            UpdateRecharge();
        }

        private void UpdateLazerShots()
        {
            OnCountLazerChanged?.Invoke(_currentLazerShots);
        }

        private void UpdateRecharge()
        {
            if (_isRecharging)
            {
                _remainingTime = RechargeTime - _rechargeTimer;
                OnTimeRecharge?.Invoke(_remainingTime);
            }
            else
            {
                _remainingTime = 0f;
                OnTimeRecharge?.Invoke(_remainingTime);
            }
        }

        private void ClearAllbulletLazer()
        {
            foreach (var bulletLazer in _bulletLazerList)
            {
                if (bulletLazer.GetComponent<Bullet>())
                {
                    _bulletPool.ReturnObject(bulletLazer);
                }
                else if (bulletLazer.GetComponent<Lazer>())
                {
                    _lazerPool.ReturnObject(bulletLazer);
                }
            }
        }
    }
}

