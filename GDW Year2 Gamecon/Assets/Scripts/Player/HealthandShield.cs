using System.Security.Cryptography;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthandShield : NetworkBehaviour
{

    [SerializeField]
    private float maxHealth;

    [HideInInspector]
    public NetworkVariable<float> Health = new NetworkVariable<float>();

    [SerializeField]
    RectTransform HealthUI;

    [SerializeField]
    TextMeshProUGUI Healthtext;

    [SerializeField]
    RectTransform healthBarAnchor;

    [SerializeField]
    GameObject healthImage;


    private GameObject gameManager;


    public override void OnNetworkSpawn()
    {

        gameManager = GameObject.FindGameObjectWithTag("GameManager");

        gameManager.GetComponent<GameManager>().allPlayers.Add(this.gameObject);

        if (IsOwner)
        {
            healthImage.SetActive(false);
        }

         

    }


    void Start()
    {
        if(!IsServer)
        {
            return;
        }

        Health.Value = maxHealth;

    }



    void Update()
    {

        if (!IsOwner)
        {


            for (int i = 0; i < gameManager.GetComponent<GameManager>().allPlayers.Count; i++)
            {

                GameObject obj = gameManager.GetComponent<GameManager>().allPlayers[i];

                if (obj.GetComponent<NetworkObject>().IsOwner)
                {

                    rotateObjectTo(healthBarAnchor, obj.transform.position);

                    break;
                }
                
            }

            return;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            HealServerRPC(30);
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

        HealthUI.localScale = new Vector3(newValue / 100f, 1, 1);
        healthImage.transform.localScale = new Vector3(newValue / 100f, 1, 1);
        Healthtext.text = newValue.ToString();
    }


    [ServerRpc]
    public void HealServerRPC(float heal)
    {
        if(Health.Value>0 && Health.Value < maxHealth)
        {
            Health.Value += heal;

            if (Health.Value > maxHealth)
                Health.Value = maxHealth;
                

        }



       
    }

    public void Damage(float dmg)
    {
        if (Health.Value > 0)
        {
            Health.Value -= dmg;
        }
        else
        {
            Health.Value = 0;
        }

        Debug.Log(Health.Value);
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

            Damage(collider.gameObject.GetComponent<projectile>().damage);

        }
    }


    protected void rotateObjectTo(Transform _object, Vector3 to)
    {

        Quaternion _lookRotation =
            Quaternion.LookRotation((to - _object.position).normalized);

        //over time
        _object.rotation =
            Quaternion.Slerp(_object.rotation, _lookRotation, Time.deltaTime * 10);

        //instant
        _object.rotation = _lookRotation;
    }



}
