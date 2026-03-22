using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject musicUI, creditsUI;
    [SerializeField] private Transform platypusHandTransform;
    [SerializeField] private TextMeshProUGUI platypusTalkingText;
    [SerializeField] private TMP_FontAsset defaultFont, horrorFont;
    [SerializeField] private Material platypusEyes;

    [SerializeField]
    private Light _light;
    
    private Coroutine _currentCoroutine;

    private void Start()
    {
        _light.color = Color.white;
        _light.intensity = 1.0f;
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

    public void Credits()
    {
        if (creditsUI.activeSelf)
        {
            creditsUI.SetActive(false);
        }
        else
        {
            creditsUI.SetActive(true);
        }
    }

    public void PointedAtPlay(Image img)
    {
        PointAtButton(img.transform.position);
        platypusTalkingText.text = "Lets see your skill";
        SetDefaultFont();
    }
    
    public void PointedAtAbout(Image img)
    {
        PointAtButton(img.transform.position);
        platypusTalkingText.text = "Want to see who the devs are?";
        SetDefaultFont();
    }
    
    public void PointedAtOptions(Image img)
    {
        PointAtButton(img.transform.position);
        platypusTalkingText.text = "Change the settings perhaps?";
        SetDefaultFont();
    }

    public void PointedAtQuit(Image img)
    {
        PointAtButton(img.transform.position);
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
        _light.color = Color.red;
        _light.intensity = 3.0f;
        platypusEyes.SetFloat("_Evil",1);
        platypusTalkingText.font = horrorFont;
        platypusTalkingText.color = Color.red;
        //platypusEyes
    }

    private void SetDefaultFont()
    {
        _light.color = Color.white;
        _light.intensity = 1.0f;
        platypusEyes.SetFloat("_Evil", 0);
        platypusTalkingText.font = defaultFont;
        platypusTalkingText.color = Color.white;
    }

    private void PointAtButton(Vector3 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.CompareTag("point"))
            {
                // platypusHandTransform.LookAt(hit.point);
                if (_currentCoroutine != null)
                    StopCoroutine(_currentCoroutine);
                
                _currentCoroutine =  StartCoroutine(LerpLookAt(platypusHandTransform, hit.point, 0.04f));
            }
            
        }
    }
    
    IEnumerator LerpLookAt(Transform platypusHandTransform, Vector3 targetPoint, float duration)
    {
        Quaternion startRotation = platypusHandTransform.rotation;
        Vector3 direction = targetPoint - platypusHandTransform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;
            platypusHandTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            time += Time.deltaTime;
            yield return null;
        }

        platypusHandTransform.rotation = targetRotation;
    }

    private void Update()
    {
        
    }
}
