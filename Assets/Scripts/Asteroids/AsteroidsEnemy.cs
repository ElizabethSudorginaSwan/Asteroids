using UnityEngine;

public class AsteroidsEnemy : MonoBehaviour
{
    [SerializeField] private float speed; // Скорость астероида
    [SerializeField] private Transform[] directionPoints; // Массив точек для преследования
    [SerializeField] private GameObject[] smallAsteroids; // Массив для маленьких астероидов

    private int pointIndex; // Индекс точки, к которой движется астероид

    private void Start()
    {
        // Поиск всех объектов с тегом "Point" на сцене
        GameObject[] waypoints = GameObject.FindGameObjectsWithTag("Point");

        // Подготовка массива из точек
        directionPoints = new Transform[waypoints.Length];

        // Заполнение массива точками для преследования
        for (int i = 0; i < waypoints.Length; i++)
        {
            directionPoints[i] = waypoints[i].transform;
        }

        // Выбор первой случайной точки
        pointIndex = Random.Range(0, directionPoints.Length);
    }

    private void Update()
    {
        // Перемещение к выбранной точке
        transform.position = Vector2.MoveTowards(transform.position, directionPoints[pointIndex].position, speed * Time.deltaTime);

        // Проверка, достигнута ли точка
        if (Vector2.Distance(transform.position, directionPoints[pointIndex].position) < 0.2f)
        {
            pointIndex = Random.Range(0, directionPoints.Length); // Выбор новой случайной точки
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Lazer")) // Если попал лазер
        {
            Destroy(gameObject); // Удалить Астероид
            Destroy(collision.gameObject); // Удалить лазер

            ScoreManager.Instance.AddScore(10); // Добавить очки
        }

        if (collision.CompareTag("Bullet")) // Если попала пуля
        {
            Destroy(collision.gameObject); // Удалить пулю
            SpawnSmallAsteroids(); // Создать маленькие астероиды
            Destroy(gameObject); // Удалить Астероид

            ScoreManager.Instance.AddScore(5); // Добавить очки

        }
    }

    private void SpawnSmallAsteroids() // Метод создания маленьких астероидов
    {
        // Выбор 2 случайных индексов из префабов
        int asteroidIndex1 = Random.Range(0, smallAsteroids.Length);
        int asteroidIndex2 = Random.Range(0, smallAsteroids.Length);

        // Создание 2 маленьких астероидов
        GameObject createdAsteroid1 = Instantiate(smallAsteroids[asteroidIndex1], transform.position, Quaternion.identity);
        GameObject createdAsteroid2 = Instantiate(smallAsteroids[asteroidIndex2], transform.position, Quaternion.identity);

        // Выбор случайного размера для 2 маленьких астероидов
        float randomSize = Random.Range(0.25f, 0.4f);
        createdAsteroid1.transform.localScale = new Vector2(randomSize, randomSize);
        createdAsteroid2.transform.localScale = new Vector2(randomSize, randomSize);

        // Выбор случайного поворота для 2 маленьких астероидов
        float randomRotation = Random.Range(0f, 360f);
        createdAsteroid1.transform.rotation = Quaternion.Euler(0f, 0f, randomRotation);
        createdAsteroid1.transform.rotation = Quaternion.Euler(0f, 0f, randomRotation);
    }
}
