using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject musicUI;
    
    public void Online()
    {
        SceneManager.LoadScene("Online");
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
