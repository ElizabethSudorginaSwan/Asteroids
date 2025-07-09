using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Portal portal; // В какой портал телепортировать
    [SerializeField] private string[] ignorTags; // Теги, которые не будут телепортироваться

    public static bool portalActive; // Флаг предотвращения бесконечной телепортации

    void Start()
    {
        portalActive = true; // Активация возможности телепортации
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        foreach (string tag in ignorTags) // Проверка всех тегов, который не будут телепортироваться
        {
            if (other.CompareTag(tag)) // Если этот тег не телепортируется
            {
                return; // Выход из метода
            }
        }

        Rigidbody2D rb = other.GetComponent<Rigidbody2D>(); // Проверка rb у входящего в телепорт объекта

        if (portalActive) // Проверка флага активности портала
        {
            portalActive = false; // Выкл флаг активности портала
            float magnitude = rb.velocity.magnitude; // Сохранить скорость объекта
            rb.velocity = Vector3.zero; // Обнулить скорость перед телепортацией

            // Расчёт направления вылета из второго портала:
            // Направление "вправо" от портала2 - направление "влево" от портала1 = напрвление вылета из портала
            Vector3 direction = portal.transform.TransformDirection(Vector3.right) - transform.TransformDirection(Vector3.left);

            other.transform.position = portal.transform.position; // Телепорт объекта к второму порталу

            // Придать объекту импульс в рассчитанном направлении с сохраненной скоростью
            rb.AddForce(direction *  magnitude, ForceMode2D.Impulse); 
        }
        else
        {
            portalActive= true; // Если флаг активности телепорта выкл, то вкл его
        }
    }
}
