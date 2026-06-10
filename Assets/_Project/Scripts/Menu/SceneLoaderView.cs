using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter.Menu
{
    public class SceneLoaderView : MonoBehaviour
    {
        [SerializeField] private Button _playButton;

        private SceneLoaderPresenter _presenter;

        public void Init(SceneLoaderPresenter presenter)
        {
            _presenter = presenter;
            _playButton.onClick.AddListener(OnPlayButtonClicked);
        }

        private void OnPlayButtonClicked()
        {
            _presenter.OnPlayClicked();
        }

        private void OnDestroy()
        {
            if (_playButton != null)
            {
                _playButton.onClick.RemoveListener(OnPlayButtonClicked);
            }
        }
    }
}


