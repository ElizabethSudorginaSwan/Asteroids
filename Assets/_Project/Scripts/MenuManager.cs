using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [field: SerializeField] public Button PlayButton { get; private set; }

    private void Start()
    {
        if (PlayButton != null)
        {
            PlayButton.onClick.AddListener(LoadScene);
        }
    }
    public void LoadScene()
    {
        SceneManager.LoadScene(1);
    }
}
