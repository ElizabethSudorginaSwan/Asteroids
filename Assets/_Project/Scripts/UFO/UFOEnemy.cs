using UnityEngine;

public class UFOEnemy : MonoBehaviour
{
    [field: SerializeField] public float Speed { get; private set; } 

    private Transform player; 

    private ScoreManager scoreManager;

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, Speed * Time.deltaTime);
    }

    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }

    public void SetScoreManager(ScoreManager manager)
    {
        scoreManager = manager;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Lazer _) || collision.TryGetComponent(out Bullet _)) 
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
            scoreManager.AddScore(20);
        }
    }
}
