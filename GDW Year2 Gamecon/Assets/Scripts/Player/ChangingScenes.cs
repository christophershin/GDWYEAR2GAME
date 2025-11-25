using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangingScenes : MonoBehaviour
{

    public string Scene;
    private bool pauseIsOn = false;
    [SerializeField] private Canvas PauseMenu;

    private void Start()
    {

    }



    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseIsOn = !pauseIsOn;

        }


        if (PauseMenu)
        {
            if (pauseIsOn)
            {
                PauseMenu.enabled = true;
                Time.timeScale = 0;
            }
            else
            {
                PauseMenu.enabled = false;
                Time.timeScale = 1;
            }
        }


    }


    public void ChangeSceneTo()
    {

        SceneManager.LoadScene(Scene);

    }


}
