using UnityEngine;
using TMPro;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerMovement))]

public class Shooter : MonoBehaviour
{
    [field: SerializeField] public GameObject Bullet { get; private set; } 
    [field: SerializeField] public GameObject Lazer { get; private set; } 
    [field: SerializeField] public Transform FirePoint { get; private set; } 

    [field: SerializeField] public float SpeedFire { get; private set; } 
    [field: SerializeField] public float BlLifetime { get; private set; } 
    [field: SerializeField] public float RechargeTime { get; private set; } 
    [field: SerializeField] public int MazLazerShots { get; private set; } 

    [field: SerializeField] public TMP_Text LazerShotsT { get; private set; } 
    [field: SerializeField] public TMP_Text RechargeT { get; private set; } 

    private int currentLazerShots; 
    private float rechargeTimer; 
    private bool isRacharging; 

    private List<GameObject> bulletLazerList = new List<GameObject>(); 
    private PlayerMovement playerMovement; 

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        
        currentLazerShots = MazLazerShots; 

        UpdateLazerShots();
        UpdateRecharge();
    }

    private void Update()
    {
        
        if (playerMovement != null && !playerMovement.live) 
        {
            ClearAllbulletLazer(); 
            isRacharging = false;
            currentLazerShots = MazLazerShots; 
            rechargeTimer = 0; 
            UpdateLazerShots();
            UpdateRecharge();
        }

        if (isRacharging)
        {
            rechargeTimer += Time.deltaTime; 
            UpdateRecharge();

            if (rechargeTimer >= RechargeTime) 
            {
                isRacharging = false;
                currentLazerShots = MazLazerShots; 
                rechargeTimer = 0; 

                UpdateLazerShots();
                UpdateRecharge();
            }
        }
    }

    public void ShootBullet() 
    {
        GameObject currentBullet = Instantiate(Bullet, FirePoint.position, Quaternion.identity);
        Rigidbody2D rbB = currentBullet.GetComponent<Rigidbody2D>();

        bulletLazerList.Add(currentBullet);

        if (rbB != null)
        {
            rbB.velocity = FirePoint.up * SpeedFire; 
        }

        Destroy(currentBullet, BlLifetime); 
    }

    public void ShootLazer() 
    {
        if (isRacharging) 
        {
            return;
        }

        if (currentLazerShots <= 0) 
        {
            StartRecharge();
            return;
        }

        GameObject currentLazer = Instantiate(Lazer, FirePoint.position, FirePoint.rotation);
        Rigidbody2D rbL = currentLazer.GetComponent<Rigidbody2D>();

        bulletLazerList.Add(currentLazer);

        if (rbL != null)
        {
            rbL.velocity = FirePoint.up * SpeedFire; 
        }

        Destroy (currentLazer, BlLifetime); 

        currentLazerShots--; 

        if (currentLazerShots <= 0) 
        {
            StartRecharge();
        }

        UpdateLazerShots();
        UpdateRecharge();
    }

    private void StartRecharge() 
    {
        isRacharging = true; 
        rechargeTimer = 0; 

        UpdateLazerShots();
        UpdateRecharge();
    }

    private void UpdateLazerShots() 
    {
        LazerShotsT.text = $"{currentLazerShots}";
    }

    private void UpdateRecharge() 
    {
        if (isRacharging)
        {
            float remainingTime = RechargeTime - rechargeTimer;
            RechargeT.text = $"{Mathf.CeilToInt(remainingTime)}"; 
        }
        else
        {
            RechargeT.text = ""; 
        }
    }

    private void ClearAllbulletLazer()
    {

        foreach (var bulletLazer in bulletLazerList)
        {
            if (bulletLazer != null) 
            {
                Destroy(bulletLazer); 
            }
        }
        bulletLazerList.Clear(); 
    }
}
