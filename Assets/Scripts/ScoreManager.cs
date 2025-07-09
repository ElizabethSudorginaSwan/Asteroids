using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // Переменная для хранения единственного экземпляра ScoreManager
    public int TotalScore { get; private set; } // Cвойство для доступа к общему счёту с защитой от записи извне

    private void Awake()
    {
        if (Instance == null) // Существует ли уже экземпляр ScoreManager
        {
            Instance = this; // Если нет, то этот объект экземпляр ScoreManager
            DontDestroyOnLoad(gameObject); // Нельзя удалять этот экземпляр между сценами
        }
        else
        {
            Destroy(gameObject); // Если экземпляр ScoreManager уже существует, удалить этот объект
        }   
    }
    
    public void AddScore(int points) // Метод для добавления очков к общему счёту
    {
        TotalScore += points; // Увеличить общий счёт на указанное кол-во очков
        Debug.Log("Total Score: " + TotalScore);
    }

    public void ResetScore() // Метод для сброса счёта
    {
        TotalScore = 0;
    }
}
