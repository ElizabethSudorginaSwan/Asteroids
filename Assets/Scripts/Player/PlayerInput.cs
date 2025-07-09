using UnityEngine;

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
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) // Движение вперёд
        {
            playerMovement.Move();
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) // Ващение вправо
        {
            playerMovement.Rotate(-1);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) // Вращение влево
        {
            playerMovement.Rotate(1);
        }
        else
        {
            playerMovement.Rotate(0); // Вращения нет
        }

        if (Input.GetMouseButtonDown(0)) // Стрельба пулями
        {
            shooter.ShootBullet();
        }

        if (Input.GetMouseButtonDown(1)) // Стрельба лазером
        {
            shooter.ShootLazer();
        }    
    }
}
