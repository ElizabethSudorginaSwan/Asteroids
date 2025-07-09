using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveForce = 10f; // Сила, применяемая к игроку для движения
    [SerializeField] private float maxSpeed = 5f; // Максимально допустипая скорость игрока
    [SerializeField] private float drag = 2f; // Сила сопротивления для игрока
    [SerializeField] private float rotatonSpeed = 120f; // Скорость вращения игрока

    [SerializeField] private TMP_Text speedText; // Текст скорости
    [SerializeField] private TMP_Text positionText; // Текст позиции
    [SerializeField] private TMP_Text rotationText; // Текст поворота

    private Rigidbody2D rb;
    private bool shouldMoveForward; // Флаг, указывающий нужно ли двигаться вперед в текущем кадре
    private float rotatonDirection; // Напрвление вращения игрока

    private MenuManager menuManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Получение компонента Rigidbody2D игрока
        rb.drag = drag; // Установка сопротивления движению игрока

        menuManager = GetComponent<MenuManager>();
    }

    public void Move() // Метод активации движения вперед
    {
        shouldMoveForward = true;
    }

    public void Rotate(int direction) // Метод установки направления вращения (-1 - вправо, 1 - влево)
    {
        rotatonDirection = direction;
    }

    private void FixedUpdate()
    {
        if (shouldMoveForward) // Движение
        {
            Vector2 forwardDirection = transform.up; // Текущее "вперёд" для игрока

            // Проверка превышения максимальной скорости
            if (Vector2.Dot(rb.velocity, forwardDirection) < maxSpeed)
            {
                // Приложение силы в направлении "вперёд"
                rb.AddForce(forwardDirection *  moveForce, ForceMode2D.Force);
            }
        }

        if (rotatonDirection != 0) // Вращение
        {
            // Изменение угла поворота с учетом направления игрока
            rb.MoveRotation(rb.rotation + rotatonDirection * rotatonSpeed * Time.fixedDeltaTime);
        }

        shouldMoveForward = false;

        UpdateSpeed();
        UpdatePosition();
        UpdateRotation();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Asteroid") || collision.CompareTag("UFO")) // Обработка столкновений
        {
            SceneManager.LoadScene(2); // Загрузка сцены с проигрышем
        }
    }

    private void UpdateSpeed() // Обновление текста скорости
    {
        float currentSpeed = rb.velocity.magnitude; // Текущая скорость
        speedText.text = $"{currentSpeed.ToString("F2")}"; // Вывод на экран (2 знака после запятой)
    }

    private void UpdatePosition() // Обновление текста позиции
    {
        Vector2 playerPos = transform.position; // Текущее положение
        positionText.text = $"{playerPos.x:F1} | {playerPos.y:F1}"; // Вывод на экран (1 знак после запятой)
    }

    private void UpdateRotation() // Обновление текста поворота
    {
        float angle = transform.eulerAngles.z; // Текущий поворот
        rotationText.text = $"{angle:F0}°"; // Вывод на экран (0 знаков после запятой)
    }
}
