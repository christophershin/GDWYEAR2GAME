using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject musicUI;
    [SerializeField] private string scene;
    public void Online()
    {
        SceneManager.LoadScene(scene);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Music()
    {
        if (musicUI.activeSelf)
        {
            musicUI.SetActive(false);
        }
        else
        {
            musicUI.SetActive(true);
        }
    }
}
