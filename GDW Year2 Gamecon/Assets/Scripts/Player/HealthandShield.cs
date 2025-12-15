using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealthandShield : NetworkBehaviour
{

    [HideInInspector]
    public NetworkVariable<float> Health = new NetworkVariable<float>();

    [SerializeField]
    RectTransform HealthUI;

    [SerializeField]
    TextMeshProUGUI Healthtext;

    [SerializeField]
    RectTransform healthBar;

    [SerializeField]
    Image healthImage;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            Health.Value = 100f;



        }

        if (IsOwner)
        {
            healthImage.enabled = false;
        }


    }

    void OnEnable()
    {
        GetComponent<HealthandShield>().Health.OnValueChanged += HealthChanged;
        Healthtext.text = GetComponent<HealthandShield>().Health.Value.ToString();

    }

    void OnDisable()
    {
        GetComponent<HealthandShield>().Health.OnValueChanged -= HealthChanged;
    }

    private void HealthChanged(float previousValue, float newValue)
    {
        HealthUI.localScale = new Vector3(newValue / 100f, 0.5f, 1);
        healthBar.localScale = new Vector3(newValue / 100f, 0.5f, 1);
        Healthtext.text = newValue.ToString();
    }



    private void OnCollisionEnter(Collision collider)
    {

        if (!IsServer)
        {
            return;
        }

        string teamID = GetComponent<EntitiesClass>().teamID;


        if (collider.gameObject.CompareTag("Parriable") && collider.gameObject.GetComponent<EntitiesClass>().teamID != teamID)
        {
            Health.Value -= 20f;
            Debug.Log(Health.Value);
        }
    }
}
