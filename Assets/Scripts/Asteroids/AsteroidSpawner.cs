using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] asteroids; // Массив астероидов
    [SerializeField] Transform[] spawnPoint; // Массив точек спавна астероидов
    [SerializeField] private float timerStart; // Начальное значение таймера
    [SerializeField] private float minSize; // Мин. размер астероида
    [SerializeField] private float maxSize;   // Макс. размер астероида

    private int asteroidIndex; // Индекс астероида 
    private int spawnIndex; // Индекс точки спавна
    private float timerSpawn; // Таймер до следующего спавна
    private float randomSize; // Рандомный размер созданного астероида
    private float randomRotation; // Рандомный поворот
    private GameObject createdAsteroid; // Созданный астероид на сцене

    private void Start()
    {
        timerSpawn = timerStart; // Указание таймеру начальное значение
    }

    private void Update()
    {
        if (timerSpawn <=0) // Если таймер на 0
        {
            // Выбор случайного астероида из массива
            asteroidIndex = Random.Range(0, asteroids.Length);

            // Выбор случайной точки для его спавна
            spawnIndex = Random.Range(0, spawnPoint.Length);

            // Создание выбранного астероида в выбранной точке
            createdAsteroid = Instantiate(asteroids[asteroidIndex], spawnPoint[spawnIndex].transform.position, Quaternion.identity);

            // Выбор случайного размера для астероида
            randomSize = Random.Range(minSize, maxSize);
            createdAsteroid.transform.localScale = new Vector2(randomSize, randomSize);

            // Выбор случайного поворота для астероида
            randomRotation = Random.Range(0f, 360f);
            createdAsteroid.transform.rotation = Quaternion.Euler(0f, 0f, randomRotation);

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
