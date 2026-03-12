using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;


public class Parrying : NetworkBehaviour
{

    [SerializeField]
    private EntitiesClass entitiesClass;
    
    public GameObject parryhitbox;

    //[SerializeField]
    private float maxParryEnergy = 100;

    //[SerializeField]
    private float EnergyConsumptionAmount = 2f;

    //[SerializeField]
    private float EnergyRestoreAmount = 2;

    //[SerializeField]
    private float maxParryCooldown = 1;


    [SerializeField]
    private TextMeshProUGUI parryEnergyText;

    [HideInInspector]
    public NetworkVariable<float> ParryEnergy = new NetworkVariable<float>();

    private float privateParryEnergy;

    private float maxResourceTimer = 2f;

    private float resourceTimer;

    private float cooldown;

    public Slider slider;

    private bool _isParrying = false;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false;
            return;
        }
    }

    private void Start()
    {
        parryhitbox.SetActive(false);
        privateParryEnergy = maxParryEnergy;
        
        //NotParryServerRPC();

        if (!IsServer)
        {
            return;
        }

        //ParryEnergy.Value = maxParryEnergy;
        

    }



    void Update()
    {
        if (IsOwner) // only the owning player should send these RPCs
        {
            // if you are alive
            if(entitiesClass.isAlive.Value)
            {
                
                if (Input.GetMouseButtonDown(1))
                {
                    // if (ParryEnergy.Value > 0)
                    // {
                    //     _isParrying = true;
                    //     //ParryServerRPC();
                    //     StartParrying();
                    // }
                    if (privateParryEnergy > 0)
                    {
                        _isParrying = true;
                        StartParrying();
                    }
                }
                
                
                // turned off for testing purposes
                // if(ParryEnergy.Value <=0)
                // {
                //     _isParrying = false;
                //     StartCoroutine(StopParrying());
                //     //NotParryServerRPC();
                // }
                
                if(privateParryEnergy <=0)
                {
                    _isParrying = false;
                    StartCoroutine(StopParrying());
                }

                if (Input.GetMouseButtonUp(1))
                {
                    _isParrying = false;
                    StartCoroutine(StopParrying());
                    //NotParryServerRPC();
                }
            }

        }


        //ParryEnergySystemServerRPC(maxParryEnergy,EnergyConsumptionAmount, EnergyRestoreAmount, resourceTimer, maxResourceTimer);
        UpdateEnergySystem();
        parryEnergyText.text = ((int)privateParryEnergy).ToString();
        
        slider.value = privateParryEnergy;


    }

    private void StartParrying()
    {
        parryhitbox.SetActive(true);
    }
    
    IEnumerator StopParrying()
    {
        yield return new WaitForSeconds(.4f);

        if (!_isParrying) parryhitbox.SetActive(false); //NotParryServerRPC();
    }
    

    private void UpdateEnergySystem()
    {
        if(cooldown>=0)
        {
            cooldown -= Time.deltaTime;
        }
        

        if (resourceTimer >= 0)
        {
            resourceTimer -= Time.deltaTime;
        }
        
        // if you are pressing the parry button
        if(parryhitbox.activeSelf)
        {

            if(privateParryEnergy>0)
            {
                if (resourceTimer < 0)
                {
                    privateParryEnergy -= EnergyConsumptionAmount;

                    resourceTimer = maxResourceTimer;
                }
            }

            if (privateParryEnergy <= 0)
            {
                privateParryEnergy = 0;
            }

        }
        else
        {
            if (privateParryEnergy < maxParryEnergy)
            {

                if (resourceTimer < 0 && cooldown< 0)
                {
                    privateParryEnergy += EnergyRestoreAmount;

                    resourceTimer = maxResourceTimer;
                }

            }
            
            if(privateParryEnergy >= maxParryEnergy)
            {
                privateParryEnergy = maxParryEnergy;
            }

        }

        parryEnergyText.text = ((int)privateParryEnergy).ToString();
    }

    [ServerRpc]
    void ParryServerRPC(ServerRpcParams rpcParams = default)
    {
        // Enable on the server
        parryhitbox.SetActive(true);
        cooldown = maxParryCooldown;

        // Tell all clients to enable theirs too
        ParryClientRPC();
    }

    [ServerRpc]
    void NotParryServerRPC(ServerRpcParams rpcParams = default)
    {
        // Disable on the server
        parryhitbox.SetActive(false);

        // Tell all clients to disable theirs too
        NotParryClientRPC();
    }

    [ClientRpc]
    void ParryClientRPC(ClientRpcParams rpcParams = default)
    {
        parryhitbox.SetActive(true);
    }

    [ClientRpc]
    void NotParryClientRPC(ClientRpcParams rpcParams = default)
    {
        parryhitbox.SetActive(false);
    }


    [ServerRpc]
    void ParryEnergySystemServerRPC(float maxEnergy, float EnergyCon, float EnergyRes, float timer, float maxTimer)
    {
        if(cooldown>=0)
        {
            cooldown -= Time.deltaTime;
        }
        

        if (resourceTimer >= 0)
        {
            resourceTimer -= Time.deltaTime;
        }
        
        // if you are pressing the parry button
        if(parryhitbox.activeSelf)
        {

            if(ParryEnergy.Value>0)
            {
                if (resourceTimer < 0)
                {
                    ParryEnergy.Value -= EnergyCon;

                    resourceTimer = maxTimer;
                }
            }

            if (ParryEnergy.Value <= 0)
            {
                ParryEnergy.Value = 0;
            }

        }
        else
        {
            if (ParryEnergy.Value < maxEnergy)
            {

                if (resourceTimer < 0 && cooldown< 0)
                {
                    ParryEnergy.Value += EnergyRes;

                    resourceTimer = maxTimer;
                }

            }
            
            if(ParryEnergy.Value >= maxEnergy)
            {
                ParryEnergy.Value = maxEnergy;
            }

        }

        parryEnergyText.text = ((int)ParryEnergy.Value).ToString();
    }


}
