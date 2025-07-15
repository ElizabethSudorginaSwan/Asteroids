using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [field: SerializeField] public GameObject[] Asteroids { get; private set; }
    [field: SerializeField] public Transform[] SpawnPoint { get; private set; }
    [field: SerializeField] public float MinSize { get; private set; }
    [field: SerializeField] public float MaxSize { get; private set; }
    [field: SerializeField] public PlayerMovement PlayerMovement { get; private set; }
    [field: SerializeField] public ScoreManager ScoreManager { get; private set; }


    private int asteroidIndex; 
    private int spawnIndex; 
    private float randomSize; 
    private float randomRotation; 

    private GameObject createdAsteroid;
    private List<GameObject> asteroidList = new List<GameObject>(); 
   
    private void Start()
    {
        StartCoroutine(DelayedAction());
    }

    private void Update()
    {
        if (PlayerMovement != null && !PlayerMovement.live) 
        {
            ClearAllAsteroids(); 
        }
    }

    private IEnumerator DelayedAction()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);

            asteroidIndex = Random.Range(0, Asteroids.Length);
            spawnIndex = Random.Range(0, SpawnPoint.Length);
            createdAsteroid = Instantiate(Asteroids[asteroidIndex], SpawnPoint[spawnIndex].transform.position, Quaternion.identity);
            asteroidList.Add(createdAsteroid);

            AsteroidsEnemy asteroidsEnemy = createdAsteroid.GetComponent<AsteroidsEnemy>();
            if (asteroidsEnemy != null)
            {
                if (PlayerMovement != null)
                {
                    asteroidsEnemy.SetPlayer(PlayerMovement);
                }
                if (ScoreManager != null)
                {
                    asteroidsEnemy.SetScoreManager(ScoreManager);
                }
                if (SpawnPoint != null)
                {
                    asteroidsEnemy.SetWaypoints(SpawnPoint);
                }
            }

            randomSize = Random.Range(MinSize, MaxSize);
            createdAsteroid.transform.localScale = new Vector2(randomSize, randomSize);

            randomRotation = Random.Range(0f, 360f);
            createdAsteroid.transform.rotation = Quaternion.Euler(0f, 0f, randomRotation);
        }
    }

    private void ClearAllAsteroids()
    {
        foreach (var asteroid in asteroidList)
        {
            if (asteroid != null) 
            {
                Destroy(asteroid); 
            }
        }
        asteroidList.Clear(); 
    }
}
