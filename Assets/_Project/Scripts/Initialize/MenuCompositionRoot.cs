using UnityEngine;

namespace SpaceShooter.Menu
{
    public class MenuCompositionRoot : MonoBehaviour
    {
        [SerializeField] private SceneLoaderView _sceneLoaderView;
        [SerializeField] private int _gameSceneIndex;

        private void Awake()
        {
            SceneLoaderModel model = new(_gameSceneIndex);
            SceneLoaderPresenter presenter = new(model);
            _sceneLoaderView.Init(presenter);
        }
    }
}

