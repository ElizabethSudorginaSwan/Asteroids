using UnityEngine;
using System.Collections.Generic;
using SpaceShooter.Factories;
using SpaceShooter.ObjectPool;
using System.Collections;
using SpaceShooter.Ammunition;      
using SpaceShooter.Player;


namespace SpaceShooter.ShooterPlayer
{
    [RequireComponent(typeof(PlayerMovement))]
    public class Shooter : MonoBehaviour
    {
        [field: SerializeField] public Transform FirePoint { get; private set; }
        [field: SerializeField] public float SpeedFire { get; private set; }
        [field: SerializeField] public float BlLifetime { get; private set; }
        [field: SerializeField] public float RechargeTime { get; private set; }
        [field: SerializeField] public int MazLazerShots { get; private set; }

        private int _currentLazerShots;
        private float _rechargeTimer;
        private bool _isRecharging;

        private readonly List<GameObject> _bulletLazerList = new();
        
        private IPool _bulletPool;
        private IPool _lazerPool;

        private GenericAmmunitionFactory _bulletFactory;
        private GenericAmmunitionFactory _lazerFactory;

        private PlayerMovement _playerMovement;
        private ShooterUIPresenter _shooterUIPresenter;

        public void Initialize(PlayerMovement playerMovement, BulletPool bulletPool, LazerPool lazerPool,
                                ShooterUIPresenter shooterUIPresenter)
        {
            _playerMovement = playerMovement;
            _bulletPool = bulletPool;
            _lazerPool = lazerPool;

            _shooterUIPresenter = shooterUIPresenter;

            _bulletFactory = new GenericAmmunitionFactory(_bulletPool, SpeedFire, FirePoint);
            _lazerFactory = new GenericAmmunitionFactory(_lazerPool, SpeedFire, FirePoint);

            _currentLazerShots = MazLazerShots;

            _shooterUIPresenter?.UpdateLazerCount(_currentLazerShots);
            _shooterUIPresenter?.UpdateRechargeTime(0f);
        }

        private void Update()
        {
            if (!_playerMovement.Live)
            {
                ClearAllbulletLazer();
                _isRecharging = false;
                _currentLazerShots = MazLazerShots;
                _rechargeTimer = 0;
                _shooterUIPresenter?.UpdateRechargeTime(_rechargeTimer);
                _shooterUIPresenter?.UpdateLazerCount(_currentLazerShots);
            }

            if (_isRecharging)
            {
                _rechargeTimer += Time.deltaTime;

                float remainingTime = RechargeTime - _rechargeTimer;

                _shooterUIPresenter?.UpdateRechargeTime(remainingTime);

                if (_rechargeTimer >= RechargeTime)
                {
                    _isRecharging = false;
                    _currentLazerShots = MazLazerShots;
                    _rechargeTimer = 0;

                    _shooterUIPresenter?.UpdateLazerCount(_currentLazerShots);
                    _shooterUIPresenter?.UpdateRechargeTime(0f);
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

            _shooterUIPresenter?.UpdateLazerCount(_currentLazerShots);

            if (_currentLazerShots <= 0)
            {
                StartRecharge();
            }
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

