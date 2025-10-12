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
        [field: SerializeField] public BasePool BulletPool { get; set; }
        [field: SerializeField] public BasePool LazerPool { get; set; }
        [field: SerializeField] public Transform FirePoint { get; private set; }
        [field: SerializeField] public float SpeedFire { get; private set; }
        [field: SerializeField] public float BlLifetime { get; private set; }
        [field: SerializeField] public float RechargeTime { get; private set; }
        [field: SerializeField] public int MazLazerShots { get; private set; }
        [field: SerializeField] public TMP_Text LazerShotsT { get; private set; }
        [field: SerializeField] public TMP_Text RechargeT { get; private set; }

        private int _currentLazerShots;
        private float _rechargeTimer;
        private bool _isRecharging;

        private readonly List<GameObject> _bulletLazerList = new();
        private PlayerMovement _playerMovement;
        private GenericAmmunitionFactory _bulletFactory;
        private GenericAmmunitionFactory _lazerFactory;

        public void Initialize(PlayerMovement playerMovement)
        {
            _playerMovement = playerMovement;

            _bulletFactory = new GenericAmmunitionFactory(BulletPool, SpeedFire, FirePoint);
            _lazerFactory = new GenericAmmunitionFactory(LazerPool, SpeedFire, FirePoint);

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
            BulletPool.ReturnObject(bullet); 
        }

        private IEnumerator ReturnLazerToPoolAfterTime(GameObject lazer, float delay)
        {
            yield return new WaitForSeconds(delay); 
            LazerPool.ReturnObject(lazer); 
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
            LazerShotsT.text = $"{_currentLazerShots}";
        }

        private void UpdateRecharge()
        {
            if (_isRecharging)
            {
                float remainingTime = RechargeTime - _rechargeTimer;
                RechargeT.text = $"{Mathf.CeilToInt(remainingTime)}";
            }
            else
            {
                RechargeT.text = "";
            }
        }

        private void ClearAllbulletLazer()
        {
            foreach (var bulletLazer in _bulletLazerList)
            {
                if (bulletLazer.GetComponent<Bullet>())
                {
                    BulletPool.ReturnObject(bulletLazer);
                }
                else if (bulletLazer.GetComponent<Lazer>())
                {
                    LazerPool.ReturnObject(bulletLazer);
                }
            }
        }
    }
}

