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

    }


    // Update is called once per frame
    void Update()
    {

        Collider collider = parryhitbox.GetComponent<BoxCollider>();

        if (Input.GetMouseButtonDown(1))
        {

            ParryServerRPC();


        }
        else if(Input.GetMouseButtonUp(1))
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
