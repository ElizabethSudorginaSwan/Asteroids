using UnityEngine;

public class UFOEnemy : MonoBehaviour
{
    [SerializeField] private float speed; // Скорость движения НЛО

    private Transform player; // Ссылка на трансформ игрока

    void Start()
    {
        // Поиск игрока по тегу "Player" и получение компонента Transform
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        // Перемещение НЛО к позиции игрока
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Lazer") || collision.CompareTag("Bullet"))
        {
            Destroy(gameObject); // Удалить НЛО
            Destroy(collision.gameObject); // Удалить лазер или пулю 

            ScoreManager.Instance.AddScore(20); // Добавить очки
        }
    }

    
}
