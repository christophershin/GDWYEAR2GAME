using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{


    [SerializeField]
    RectTransform HealthUI;

    [SerializeField]
    TextMeshProUGUI Healthtext;


    void OnEnable()
    {
        GetComponent<EntitiesClass>().Health.OnValueChanged += HealthChanged;
        Healthtext.text = GetComponent<EntitiesClass>().Health.Value.ToString();

    }

    void OnDisable()
    {
        GetComponent<EntitiesClass>().Health.OnValueChanged -= HealthChanged;
    }

    private void HealthChanged(float previousValue, float newValue)
    {
        HealthUI.localScale = new Vector3(newValue/100f, 0.5f, 1);
        Healthtext.text = newValue.ToString();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
