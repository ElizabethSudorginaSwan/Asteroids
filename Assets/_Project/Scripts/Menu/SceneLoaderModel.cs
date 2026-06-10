using UnityEngine.SceneManagement;

namespace SpaceShooter.Menu
{
    public class SceneLoaderModel
    {
        private readonly int _sceneIndex;

        public SceneLoaderModel(int sceneIndex)
        {
            _sceneIndex = sceneIndex;
        }

        public void LoadGameScene()
        {
            SceneManager.LoadScene(_sceneIndex);
        }
    }
}

