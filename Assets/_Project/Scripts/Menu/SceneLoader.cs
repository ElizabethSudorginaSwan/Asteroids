using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SpaceShooter.Menu
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private Button PlayButton;
        [SerializeField] private int GameSceneIndex;

        private void Start()
        {
            PlayButton.onClick.AddListener(LoadSceneGame);
        }

        private void OnDestroy()
        {
            PlayButton.onClick.RemoveListener(LoadSceneGame);
        }

        private void LoadSceneGame()
        {
            LoadScene(GameSceneIndex);
        }

        private void LoadScene(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }
}

