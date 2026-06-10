using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter.Player
{
    public class PlayerGameOverUIView : MonoBehaviour
    {
        [SerializeField] private Button _restartButton;

        private PlayerUIPresenter _presenter;

        public void Init(PlayerUIPresenter presenter)
        {
            _presenter = presenter;

            _restartButton.onClick.RemoveAllListeners();
            _restartButton.onClick.AddListener(OnRestartButtonClicked);
        }

        private void OnRestartButtonClicked()
        {
            _presenter?.RestartGame();
        }

        private void OnDestroy()
        {
            if (_restartButton != null)
            {
                _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
            }
        }
    }
}
