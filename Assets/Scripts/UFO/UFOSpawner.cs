using UnityEngine;

public class UFOSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ufo; // НЛО
    [SerializeField] Transform[] spawnPoint; // Массив точек спавна НЛО
    [SerializeField] private float timerStart; // Начальное значение таймера
    [SerializeField] private float minSize; // Мин. размер НЛО
    [SerializeField] private float maxSize;   // Макс. размер НЛO
    
    private int spawnIndex; // Индекс точки спавна
    private float timerSpawn; // Таймер до следующего спавна
    private float randomSize; // Рандомный размер созданного НЛО
    private GameObject createdUFO; // Созданный НЛО на сцене

    private void Start()
    {
        timerSpawn = timerStart; // Указание таймеру начальное значение
    }

    private void Update()
    {
        if (timerSpawn <= 0) // Если таймер на 0
        {
            // Выбор случайной точки для его спавна
            spawnIndex = Random.Range(0, spawnPoint.Length);

            // Создание НЛO в выбранной точке
            createdUFO = Instantiate(ufo, spawnPoint[spawnIndex].transform.position, Quaternion.identity);

            // Выбор случайного размера для НЛO
            randomSize = Random.Range(minSize, maxSize);
            createdUFO.transform.localScale = new Vector2(randomSize, randomSize);

            // Сброс таймера на начальное значение
            timerSpawn = timerStart;
        }
        else
        {
            // Если таймер не на 0, то уменьшить его значение
            timerSpawn -= Time.deltaTime;
        }
    }
}
