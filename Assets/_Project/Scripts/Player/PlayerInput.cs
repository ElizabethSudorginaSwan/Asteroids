using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Shooter))]
public class PlayerInput : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Shooter shooter;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        shooter = GetComponent<Shooter>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) 
        {
            playerMovement.Move();
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) 
        {
            playerMovement.Rotate(-1);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) 
        {
            playerMovement.Rotate(1);
        }
        else
        {
            playerMovement.Rotate(0);
        }

        if (Input.GetMouseButtonDown(0)) 
        {
            shooter.ShootBullet();
        }

        if (Input.GetMouseButtonDown(1)) 
        {
            shooter.ShootLazer();
        }    
    }
}
