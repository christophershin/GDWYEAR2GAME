using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    private void Start()
    {
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
