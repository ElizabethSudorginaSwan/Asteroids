using UnityEngine;
using TMPro; 

namespace SpaceShooter.Score
{
    public class ScoreManagerUI : MonoBehaviour
    {
        [field: SerializeField] public TMP_Text FinalScoreText {  get; set; } 

        private ScoreManager _scoreManager;

        public void Initialize(ScoreManager scoreManager)
        {
            if (_scoreManager != null)
            {
                _scoreManager.OnScoreChanged -= UpdateFinalScoreText;
            }

            _scoreManager = scoreManager;

            _scoreManager.OnScoreChanged += UpdateFinalScoreText;

            UpdateFinalScoreText(_scoreManager.CurrentScore);
        }

        private void OnDestroy()
        {
            _scoreManager.OnScoreChanged -= UpdateFinalScoreText;
        }

        private void UpdateFinalScoreText(int currentScore)
        {
            FinalScoreText.text = $"{currentScore}";
        }
    }
}



















        //        // Ссылка на текстовый элемент для отображения счета
        //        [field: SerializeField] public TMP_Text ScoreText {  get; set; }
        //        // Ссылка на компонент управления игроком
        //        [field: SerializeField] public PlayerMovement PlayerMovement { get; set; }
        //        // Ссылка на объект событий очков
        //        [field: SerializeField] public ScoreEvent ScoreEvent { get; set; }
        //        // Ссылка на менеджер счета
        //        [field: SerializeField] public ScoreManager ScoreManager { get; set; }

//        // При активации объекта
//        private void OnEnable()
//        {
//            // Подписываемся на событие изменения счета
//            ScoreEvent.OnEnemyDestroyed += UpdateScoreText;
//        }

//        // При деактивации объекта
//        private void OnDisable()
//        {
//            // Отписываемся от события
//            ScoreEvent.OnEnemyDestroyed -= UpdateScoreText;
//        }

//        // Метод, вызываемый каждый кадр
//        private void Update()
//        {
//            // Если игрок мертв
//            if (!PlayerMovement.Live)
//            {
//                // Обновляем текст счета
//                ScoreText.text = $"{ScoreManager.TotalScore}";
//            }
//        }

//        // Метод для обновления текста счета
//        private void UpdateScoreText(int score)
//        {
//            // Устанавливаем новый текст с текущим счетом
//            ScoreText.text = $"{ScoreManager.TotalScore}";
//        }
//    }
//}

