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
    private float EnergyConsumptionAmount = 3f;

    //[SerializeField]
    private float EnergyRestoreAmount = 1f;

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
    //public Slider shieldSlider;

    private bool _isParryButtonDown = false;
    //private bool _canRecover = false;
    
    private NetworkVariable<bool> _isHitboxActive = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()
    {
        parryhitbox.SetActive(_isHitboxActive.Value);
        _isHitboxActive.OnValueChanged += OnObjectStateChanged;
    }
    
    private void OnObjectStateChanged(bool previousValue, bool newValue)
    {
        parryhitbox.SetActive(newValue);
    }

    private void Start()
    {
        if (!IsOwner)
        {
            return;
        }
        
        privateParryEnergy = maxParryEnergy;
    }



    void Update()
    {
        
        if (!IsOwner || !entitiesClass.isAlive.Value) return;
        
        if (Input.GetMouseButtonDown(1))
        {
            if (privateParryEnergy > 0)
            {
                _isParryButtonDown = true;
                _isHitboxActive.Value = true;
            }
        }
                
        if(privateParryEnergy <=0)
        {
            privateParryEnergy = 0;
            _isParryButtonDown = false;
            _isHitboxActive.Value = false;
        }

        if (Input.GetMouseButtonUp(1))
        {
            _isParryButtonDown = false;
            StartCoroutine(StopParryingDelayed());
        }
        
        UpdateEnergySystem();
        parryEnergyText.text = ((int)privateParryEnergy).ToString();
        
        slider.value = privateParryEnergy;
    }
    
    IEnumerator StopParryingDelayed()
    {
        yield return new WaitForSeconds(.4f);
        
        if (_isParryButtonDown == false)  _isHitboxActive.Value = false;
    }
    

    private void UpdateEnergySystem()
    {
        if (parryhitbox.activeSelf)
        {
            privateParryEnergy -= EnergyConsumptionAmount * Time.deltaTime * 30f;
        }
        else
        {
            privateParryEnergy += EnergyRestoreAmount * Time.deltaTime * 30f;
        }
        
        if (privateParryEnergy >= maxParryEnergy)
        {
            privateParryEnergy = maxParryEnergy;
        }
    }

    // [ServerRpc]
    // void ParryServerRPC(ServerRpcParams rpcParams = default)
    // {
    //     // Enable on the server
    //     parryhitbox.SetActive(true);
    //     cooldown = maxParryCooldown;
    //
    //     // Tell all clients to enable theirs too
    //     ParryClientRPC();
    // }

    // [ServerRpc]
    // void NotParryServerRPC(ServerRpcParams rpcParams = default)
    // {
    //     // Disable on the server
    //     parryhitbox.SetActive(false);
    //
    //     // Tell all clients to disable theirs too
    //     NotParryClientRPC();
    // }

    // [ClientRpc]
    // void ParryClientRPC(ClientRpcParams rpcParams = default)
    // {
    //     parryhitbox.SetActive(true);
    // }

    // [ClientRpc]
    // void NotParryClientRPC(ClientRpcParams rpcParams = default)
    // {
    //     parryhitbox.SetActive(false);
    // }


    // [ServerRpc]
    // void ParryEnergySystemServerRPC(float maxEnergy, float EnergyCon, float EnergyRes, float timer, float maxTimer)
    // {
    //     if(cooldown>=0)
    //     {
    //         cooldown -= Time.deltaTime;
    //     }
    //     
    //
    //     if (resourceTimer >= 0)
    //     {
    //         resourceTimer -= Time.deltaTime;
    //     }
    //     
    //     // if you are pressing the parry button
    //     if(parryhitbox.activeSelf)
    //     {
    //
    //         if(ParryEnergy.Value>0)
    //         {
    //             if (resourceTimer < 0)
    //             {
    //                 ParryEnergy.Value -= EnergyCon;
    //
    //                 resourceTimer = maxTimer;
    //             }
    //         }
    //
    //         if (ParryEnergy.Value <= 0)
    //         {
    //             ParryEnergy.Value = 0;
    //         }
    //
    //     }
    //     else
    //     {
    //         if (ParryEnergy.Value < maxEnergy)
    //         {
    //
    //             if (resourceTimer < 0 && cooldown< 0)
    //             {
    //                 ParryEnergy.Value += EnergyRes;
    //
    //                 resourceTimer = maxTimer;
    //             }
    //
    //         }
    //         
    //         if(ParryEnergy.Value >= maxEnergy)
    //         {
    //             ParryEnergy.Value = maxEnergy;
    //         }
    //
    //     }
    //
    //     parryEnergyText.text = ((int)ParryEnergy.Value).ToString();
    // }


}
