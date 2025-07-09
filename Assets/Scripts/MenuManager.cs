using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button playAgain; // Кнопка "Играть заново"
    [SerializeField] private TMP_Text scoreText; // Текстовое поле для отображения общего счёта

    private void Start()
    {
        playAgain.onClick.AddListener(OnPlayAgainClicked); // Обработчик нажатия кнопки "Играть заново"

        if (ScoreManager.Instance != null && scoreText != null) // Проверка существования ScoreManager и текстового поля
        {
            scoreText.text = $"{ScoreManager.Instance.TotalScore}"; // Вывод текста со счётом
        }    
    }

    private void OnPlayAgainClicked() // Обработчик нажатия кнопки "Играть заново"
    {
        ScoreManager.Instance.ResetScore(); // Сброс счёта на 0
    }
}
