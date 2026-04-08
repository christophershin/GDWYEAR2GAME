using UnityEngine;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Slider _sd;
    void Start()
    {
        //_sd = GetComponent<Slider>();
        
        float playerVolume = 0;

        if (PlayerPrefs.HasKey("Volume"))
        {
            playerVolume = PlayerPrefs.GetFloat("Volume");
        }
        else
        {
            PlayerPrefs.SetFloat("Volume", 0.5f);
        }
        
        _sd.value = playerVolume;
        audioSource.volume = playerVolume;
        _sd.onValueChanged.AddListener(UpdateVolume);
    }

    void UpdateVolume(float value)
    {
        audioSource.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
    }
}
