using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject musicUI;
    [SerializeField] private Transform platypusHandTransform;
    [SerializeField] private TextMeshProUGUI platypusTalkingText;
    [SerializeField] private TMP_FontAsset defaultFont, horrorFont;
    [SerializeField] private Material platypusEyes;

    private void Start()
    {
        platypusEyes.SetFloat("_Evil", 0);
    }

    public void Online()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void PointedAtPlay()
    {
        PointAtButton();
        platypusTalkingText.text = "Lets see your skill";
        SetDefaultFont();
    }
    
    public void PointedAtAbout()
    {
        PointAtButton();
        platypusTalkingText.text = "Want to know more about the game?";
        SetDefaultFont();
    }
    
    public void PointedAtOptions()
    {
        PointAtButton();
        platypusTalkingText.text = "Change the settings perhaps?";
        SetDefaultFont();
    }

    public void PointedAtQuit()
    {
        PointAtButton();
        platypusTalkingText.text = "Do you really want to leave?";
        SetHorrorFont();
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

    private void SetHorrorFont()
    {
        platypusEyes.SetFloat("_Evil",1);
        platypusTalkingText.font = horrorFont;
        platypusTalkingText.color = Color.red;
        //platypusEyes
    }

    private void SetDefaultFont()
    {
        platypusEyes.SetFloat("_Evil", 0);
        platypusTalkingText.font = defaultFont;
        platypusTalkingText.color = Color.white;
    }

    private void PointAtButton()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.CompareTag("point"))
            {
                platypusHandTransform.LookAt(hit.point);
            }
            
        }
    }
}
