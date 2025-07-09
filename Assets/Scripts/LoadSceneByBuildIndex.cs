using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneByBuildIndex : MonoBehaviour
{
    public void LoadScene(int buildIndex) // Загрузка сцены по номеру билда
    {
        SceneManager.LoadScene(buildIndex);
    }
}
