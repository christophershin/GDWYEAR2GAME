using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SideBar : NetworkBehaviour
{
    [SerializeField] private GameObject _sideBar;
    [SerializeField] private TextMeshProUGUI _text;

    private void Start()
    {
        if (!IsOwner) return;
        
        if (!PlayerPrefs.HasKey("Tutorial"))
        {
            PlayerPrefs.SetInt("Tutorial",1);
        }
        
        if (PlayerPrefs.GetInt("Tutorial") == 1)
        {
            _text.text = "Turn tutorial off";
        }
        else
        {
            _text.text = "Turn tutorial on";
        }
    }

    private void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.Tab))
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
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene("Scenes/NEW MAIN MENU TEST");
    }

    public void Restart()
    {
        if (!IsOwner) return;
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene("Scenes/SampleScene");
    }
    
    public void SetTutorial()
    {
        if (PlayerPrefs.GetInt("Tutorial") == 1)
        {
            PlayerPrefs.SetInt("Tutorial",2);
            _text.text = "Turn tutorial on";
        }
        else
        {
            PlayerPrefs.SetInt("Tutorial",1);
            _text.text = "Turn tutorial off";
        }
        
    }
}
