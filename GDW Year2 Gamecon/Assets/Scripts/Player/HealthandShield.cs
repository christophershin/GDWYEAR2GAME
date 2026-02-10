using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;

public class HealthandShield : NetworkBehaviour
{

    [SerializeField]
    private float maxHealth;

    [SerializeField]
    private float maxShield;

    [SerializeField]
    private float maxRegenShieldTimer;

    private float regenShieldTimer;


    [HideInInspector]
    public NetworkVariable<float> Health = new NetworkVariable<float>();

    [HideInInspector]
    public NetworkVariable<float> Shield = new NetworkVariable<float>();

    [SerializeField]
    RectTransform HealthUI;

    [SerializeField]
    RectTransform ShieldUI;


    public TextMeshProUGUI CenterText; 


    [SerializeField]
    TextMeshProUGUI Healthtext;

    [SerializeField]
    TextMeshProUGUI Shieldtext;

    [SerializeField]
    RectTransform healthBarAnchor;

    [SerializeField]
    RectTransform shieldBarAnchor;

    [SerializeField]
    GameObject healthImage;


    [SerializeField]
    GameObject shieldImage;

    [SerializeField]
    private EntitiesClass entitiesClass;


    private GameManager gameManager;


    public override void OnNetworkSpawn()
    {

        gameManager = FindFirstObjectByType<GameManager>();

        gameManager.allPlayers.Add(this.gameObject);

        if (IsOwner)
        {
            healthImage.SetActive(false);
            shieldImage.SetActive(false);
        }

    }


    void Start()
    {
        if(!IsServer)
        {
            return;
        }

        Health.Value = maxHealth;
        Shield.Value = maxShield;

    }



    void Update()
    {

        if (!IsOwner)
        {


            for (int i = 0; i < gameManager.allPlayers.Count; i++)
            {

                GameObject obj = gameManager.allPlayers[i];

                if (obj.GetComponent<NetworkObject>().IsOwner)
                {

                    rotateObjectTo(healthBarAnchor, obj.transform.position);
                    rotateObjectTo(shieldBarAnchor, obj.transform.position);

                    break;
                }
                
            }

            //return;
        }









        if (Input.GetKeyDown(KeyCode.Q))
        {
            HealServerRPC(30);
        }


        RegenShieldServerRPC();
        


    }










    void OnEnable()
    {
        GetComponent<HealthandShield>().Health.OnValueChanged += HealthChanged;
        GetComponent<HealthandShield>().Shield.OnValueChanged += ShieldChanged;
        Healthtext.text = GetComponent<HealthandShield>().Health.Value.ToString();
        Shieldtext.text = GetComponent<HealthandShield>().Shield.Value.ToString();

    }

    void OnDisable()
    {
        GetComponent<HealthandShield>().Health.OnValueChanged -= HealthChanged;
        GetComponent<HealthandShield>().Health.OnValueChanged -= ShieldChanged;
    }

    private void HealthChanged(float previousValue, float newValue)
    {

        HealthUI.localScale = new Vector3(newValue / maxHealth, 1, 1);
        healthImage.transform.localScale = new Vector3(newValue / maxHealth, 1, 1);
        Healthtext.text = newValue.ToString();
    }

    private void ShieldChanged(float previousValue, float newValue)
    {
        ShieldUI.localScale = new Vector3(newValue / maxShield, 1, 1);
        shieldImage.transform.localScale = new Vector3(newValue / maxShield, 1, 1);
        Shieldtext.text = newValue.ToString();
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

    [ServerRpc]
    public void RegenShieldServerRPC()
    {

        if(Shield.Value<maxShield && Health.Value>0)
        {
            regenShieldTimer -= Time.deltaTime;

            if(regenShieldTimer<=0)
            {

                Shield.Value += maxShield / 10;

                regenShieldTimer = 0.3f;
            }

        }

    }





    public void Damage(float dmg)
    {

        if (Shield.Value > 0)
        {
            Shield.Value -= dmg;
            regenShieldTimer = maxRegenShieldTimer;

        }else if (Health.Value > 0 && Shield.Value <= 0)
        {

            Health.Value -= dmg;
            regenShieldTimer = maxRegenShieldTimer;
        }

        if (Health.Value <= 0)
        {

            entitiesClass.isAlive.Value = false;

            Health.Value = 0;
        }

        if(Shield.Value<=0)
        {
            Shield.Value = 0;
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



     private IEnumerator TextLifeTime(string text1, string text2, float time)
    {

        text1 = text2;

        yield return new WaitForSeconds(time);

        text1 = "";
    }




}
