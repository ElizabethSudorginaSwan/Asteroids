using UnityEngine;
using TMPro;

public class Shooter : MonoBehaviour
{
    [SerializeField] private GameObject bullet; // Префаб пули
    [SerializeField] private GameObject lazer; // Префаб лазера
    [SerializeField] private Transform firePoint; // Точка вылета пули

    [SerializeField] private float speedFire; // Скорость стрельбы
    [SerializeField] private float blLifetime; // Время жизни пули и лазера
    [SerializeField] private float rechargeTime; // Время для перезарадки
    [SerializeField] private int mazLazerShots; // Мак. количество выстрелов лазером

    [SerializeField] private TMP_Text lazerShotsT; // UI кол-во зарядов лазера
    [SerializeField] private TMP_Text rechargeT; // UI время перезарядки

    private int currentLazerShots; // Текущее количество пуль для лазера
    private float rechargeTimer; // Текущий таймер перезарядки
    private bool isRacharging; // Флаг процесса перезарядки

    private void Start()
    {
        currentLazerShots = mazLazerShots; // Максимальный заряд лазера

        UpdateLazerShots();
        UpdateRecharge();
    }

    private void Update()
    {
        if (isRacharging)
        {
            rechargeTimer += Time.deltaTime; // Увеличение таймера перезарядки
            UpdateRecharge();

            if (rechargeTimer >= rechargeTime) // Если перезарядка завершилась
            {
                isRacharging = false;
                currentLazerShots = mazLazerShots; // Максимальный заряд лазера
                rechargeTimer = 0; // Сброс таймера

                UpdateLazerShots();
                UpdateRecharge();
            }
        }
    }

    public void ShootBullet() // Стрельба пулями
    {
        // Создание пули в точке выстрелов
        GameObject currentBullet = Instantiate(bullet, firePoint.position, Quaternion.identity);
        Rigidbody2D rbB = currentBullet.GetComponent<Rigidbody2D>();

        if (rbB != null)
        {
            rbB.velocity = firePoint.up * speedFire; // Пуля летит вперёд относительно игрока
        }

        Destroy(currentBullet, blLifetime); // Удаление пули
    }

    public void ShootLazer() // Стрельба лазером (если есть заряды и нет перезарядки)
    {
        if (isRacharging) // Если идёт перезарядка, стрелять нельзя
        {
            return;
        }

        if (currentLazerShots <= 0) // Если заряды кончились, старт перезарядки
        {
            StartRecharge();
            return;
        }

        // Создаем лазер в точке выстрелов
        GameObject currentLazer = Instantiate(lazer, firePoint.position, firePoint.rotation);
        Rigidbody2D rbL = currentLazer.GetComponent<Rigidbody2D>();

        if (rbL != null)
        {
            rbL.velocity = firePoint.up * speedFire; // Лазер летит вперёд относительно игрока
        }

        Destroy (currentLazer, blLifetime); // Удаление лазера

        currentLazerShots--; // Уменьшение количества зарядов

        if (currentLazerShots <= 0) // Если заряды кончились, старт перезарядки
        {
            StartRecharge();
        }

        UpdateLazerShots();
        UpdateRecharge();
    }

    private void StartRecharge() // Запуск перезарядки лазера
    {
        isRacharging = true; // Перезарядка началась
        rechargeTimer = 0; // Сброс таймера

        UpdateLazerShots();
        UpdateRecharge();
    }

    private void UpdateLazerShots() // Обновление UI (кол-во зарядов лазера)
    {
        lazerShotsT.text = $"{currentLazerShots}";
    }

    private void UpdateRecharge() // Обновление UI (время перезарядки)
    {
        if (isRacharging)
        {
            float remainingTime = rechargeTime - rechargeTimer;
            rechargeT.text = $"{Mathf.CeilToInt(remainingTime)}"; // Округляем
        }
        else
        {
            rechargeT.text = ""; // Если перезарядки нет, то поле пустое
        }
    }
}
