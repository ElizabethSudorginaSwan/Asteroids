using System.Collections.Generic;
using UnityEngine;

public class AsteroidsEnemy : MonoBehaviour
{
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public Transform[] DirectionPoints { get; private set; }
    [field: SerializeField] public GameObject[] SmallAsteroids { get; private set; }

    private int pointIndex; 

    private SmallAsteroidEnemy smallAsteroidManager;
    private ScoreManager scoreManager;
    private PlayerMovement playerMovement;

    public void SetWaypoints(Transform[] points)
    {
        DirectionPoints = points;
        pointIndex = Random.Range(0, DirectionPoints.Length);
    }

    public void SetScoreManager(ScoreManager manager)
    {
        scoreManager = manager;
    }

    public void SetPlayer(PlayerMovement player)
    {
        playerMovement = player;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, DirectionPoints[pointIndex].position, Speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, DirectionPoints[pointIndex].position) < 0.2f)
        {
            pointIndex = Random.Range(0, DirectionPoints.Length);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Lazer _))
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
            scoreManager.AddScore(10);
        }
        else if (collision.TryGetComponent(out Bullet _))
        {
            Destroy(collision.gameObject);
            SpawnSmallAsteroids();
            Destroy(gameObject);

            scoreManager.AddScore(5);
        }
    }

    private void SpawnSmallAsteroids()
    {
        for (int i = 0; i < 2; i++)
        {
            int randomIndex = Random.Range(0, SmallAsteroids.Length);

            GameObject smallAsteroid = Instantiate(SmallAsteroids[randomIndex], transform.position, Quaternion.identity);

            if (!smallAsteroid.TryGetComponent(out SmallAsteroidEnemy smallEnemy)) continue;
            smallEnemy.SetPlayer(playerMovement);
            smallEnemy.SetScoreManager(scoreManager);
            smallEnemy.SetWaypoints(DirectionPoints);
            smallEnemy.AddToAsteroidList(smallAsteroid);

            float randomSize = Random.Range(1f, 1.3f);
            smallAsteroid.transform.localScale = new Vector2(randomSize, randomSize);
            smallAsteroid.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        }
    }
}
