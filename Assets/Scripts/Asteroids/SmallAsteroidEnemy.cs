using UnityEngine;

public class SmallAsteroidEnemy : MonoBehaviour
{
    [SerializeField] private float speed; // Скорость маленького астероида
    [SerializeField] private Transform[] directionPoints; // Массив точек для преследования

    private int pointIndex; // Индекс точки, к которой движется маленький астероид

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
        if (collision.CompareTag("Lazer") || collision.CompareTag("Bullet")) // Если попали лазер или пуля
        {
            Destroy(gameObject); // Удалить маленький астероид
            Destroy(collision.gameObject); // Удалить лазер или пулю

            ScoreManager.Instance.AddScore(30); // Добавить очки
        }
    }
}
