using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SmallAsteroidEnemy : MonoBehaviour
{
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public Transform[] DirectionPoints { get; private set; } 

    private int pointIndex; 
    private List<GameObject> smallAsteroidList = new List<GameObject>(); 

    private PlayerMovement playerMovement; 
    private ScoreManager scoreManager;


    private void Update()
    {
        if (playerMovement != null && !playerMovement.live) 
        {
            ClearAllSmallAsteroids();
        }
       
        transform.position = Vector2.MoveTowards(transform.position, DirectionPoints[pointIndex].position, Speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, DirectionPoints[pointIndex].position) < 0.2f)
        {
            pointIndex = Random.Range(0, DirectionPoints.Length);
        }
    }

    public void SetPlayer(PlayerMovement player)
    {
        playerMovement = player;
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

            scoreManager.AddScore(30); 
        }
    }

    private void ClearAllSmallAsteroids()
    {
        foreach (var smallAsteroid in smallAsteroidList)
        {
            if (smallAsteroid != null) 
            {
                Destroy(smallAsteroid); 
            }
        }
        smallAsteroidList.Clear(); 
    }

    public void AddToAsteroidList(GameObject asteroid)
    {
        smallAsteroidList.Add(asteroid);
    }

    public void SetWaypoints(Transform[] points)
    {
        DirectionPoints = points;
        pointIndex = Random.Range(0, DirectionPoints.Length);
    }
}
