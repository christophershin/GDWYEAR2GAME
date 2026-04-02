using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Authentication;

public class SideBar : NetworkBehaviour
{
    [SerializeField] private GameObject _sideBar;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private GameObject tutorialStuff;

    private void Start()
    {
        if (!IsOwner) return;
        
        if (!PlayerPrefs.HasKey("Tutorial"))
        {
            PlayerPrefs.SetInt("Tutorial",1);
        }
        
        if (PlayerPrefs.GetInt("Tutorial") == 1)
        {
            tutorialStuff.SetActive(true);
            _text.text = "Turn tutorial off";
        }
        else
        {
            tutorialStuff.SetActive(false);
            _text.text = "Turn tutorial on";
        }
    }

    private void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetSidebar();
        }
    }

    public void SetSidebar()
    {
        if (!IsOwner) return;
        if (_sideBar.activeSelf)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            _sideBar.SetActive(false);
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            _sideBar.SetActive(true);
        }
    }

    public void MainMenu()
    {
        if (!IsOwner) return;
        
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsListening)
        {
            AuthenticationService.Instance.SignOut(true); 
            NetworkManager.Singleton.Shutdown();
            SceneManager.LoadScene("Scenes/NEW MAIN MENU TEST", LoadSceneMode.Single);
        }
    }

    public void Restart()
    {
        if (!IsOwner) return;
        
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsListening)
        {
            AuthenticationService.Instance.SignOut(true); 
            NetworkManager.Singleton.Shutdown();
            SceneManager.LoadScene("Scenes/SampleScene", LoadSceneMode.Single);
        }
        
    }
    
    public void SetTutorial()
    {
        if (PlayerPrefs.GetInt("Tutorial") == 1)
        {
            PlayerPrefs.SetInt("Tutorial",2);
            _text.text = "Turn tutorial on";
            tutorialStuff.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetInt("Tutorial",1);
            _text.text = "Turn tutorial off";
            tutorialStuff.SetActive(true);
        }
        
    }
}
