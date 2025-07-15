using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UFOSpawner : MonoBehaviour
{
    [field: SerializeField] public GameObject Ufo { get; private set; }
    [field: SerializeField] public Transform[] SpawnPoint { get; private set; }
    [field: SerializeField] public float MinSize { get; private set; }
    [field: SerializeField] public float MaxSize { get; private set; }
    [field: SerializeField] public PlayerMovement PlayerMovement { get; private set; }
    [field: SerializeField] public ScoreManager ScoreManager { get; private set; }

    private int spawnIndex; 
    private float randomSize; 

    private GameObject createdUFO; 
    private List<GameObject> ufoList = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(DelayedAction());
    }

    private void Update()
    {
        if (PlayerMovement != null && !PlayerMovement.live)
        {
            ClearAllUfo();
        }
    }

    private IEnumerator DelayedAction()
    {
        while (true)
        {
            yield return new WaitForSeconds(6f);

            spawnIndex = Random.Range(0, SpawnPoint.Length);
            createdUFO = Instantiate(Ufo, SpawnPoint[spawnIndex].transform.position, Quaternion.identity);
            ufoList.Add(createdUFO);

            UFOEnemy ufoEnemy = createdUFO.GetComponent<UFOEnemy>();
            if (ufoEnemy != null)
            {
                if (PlayerMovement != null)
                {
                    ufoEnemy.SetPlayer(PlayerMovement.transform);
                }
                if (ScoreManager != null)
                {
                    ufoEnemy.SetScoreManager(ScoreManager);
                }
            }

            randomSize = Random.Range(MinSize, MaxSize);
            createdUFO.transform.localScale = new Vector2(randomSize, randomSize);
        }
    }

    private void ClearAllUfo()
    {
        foreach (var ufo in ufoList)
        {
            if (ufo != null) 
            {
                Destroy(ufo); 
            }
        }
        ufoList.Clear(); 
    }
}

