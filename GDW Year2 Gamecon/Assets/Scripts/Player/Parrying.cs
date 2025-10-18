using Unity.Netcode;
using UnityEngine;


public class Parrying : NetworkBehaviour
{

    
    public GameObject parryhitbox;



    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false;
            return;

        }

        parryhitbox.SetActive(false);

    }

    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {

            ParryServerRPC();

        }

        if (Input.GetMouseButtonUp(1))
        {

            NotParryServerRPC();

        }

    }


    [ServerRpc]
    void ParryServerRPC()
    {

        parryhitbox.SetActive(true);

    }

    [ServerRpc]
    void NotParryServerRPC()
    {
        parryhitbox.SetActive(false);
    }

}
