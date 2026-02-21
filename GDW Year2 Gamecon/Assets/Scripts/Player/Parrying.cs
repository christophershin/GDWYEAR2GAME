using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;


public class Parrying : NetworkBehaviour
{

    [SerializeField]
    private EntitiesClass entitiesClass;
    
    public GameObject parryhitbox;

    [SerializeField]
    private float maxParryEnergy;

    [SerializeField]
    private float EnergyConsumptionAmount;

    [SerializeField]
    private float EnergyRestoreAmount;

    [SerializeField]
    private float maxParryCooldown;


    [SerializeField]
    private TextMeshProUGUI parryEnergyText;

    [SerializeField] private Slider slider;
    //private float _slideSpeed = 10f;

    [HideInInspector]
    public NetworkVariable<float> ParryEnergy = new NetworkVariable<float>();

    private float maxResourceTimer = 0.05f;

    private float resourceTimer;

    private float cooldown;



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

        NotParryServerRPC();

        if (!IsServer)
        {
            return;
        }

        ParryEnergy.Value = maxParryEnergy;

    }



    void Update()
    {
        if (IsOwner) // only the owning player should send these RPCs
        {
            // if you are alive
            if(entitiesClass.isAlive.Value)
            {

                if (ParryEnergy.Value > 0)
                {

                    if (Input.GetMouseButtonDown(1))
                    {
                        ParryServerRPC();

                    }

                }

                if(ParryEnergy.Value <=0)
                {
                    NotParryServerRPC();
                }

                if (Input.GetMouseButtonUp(1))
                {
                    NotParryServerRPC();
                }
            }

        }


        ParryEnergySystemServerRPC(maxParryEnergy,EnergyConsumptionAmount, EnergyRestoreAmount, resourceTimer, maxResourceTimer);
        parryEnergyText.text = ((int)ParryEnergy.Value).ToString();
        
        //slider.value = ParryEnergy.Value;

        slider.value = Mathf.Lerp(
            slider.value,
            ParryEnergy.Value,
            10f * Time.deltaTime
        );

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
