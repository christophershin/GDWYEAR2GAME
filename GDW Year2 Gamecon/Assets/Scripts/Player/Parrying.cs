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

    private void Start()
    {
        NotParryServerRPC();
    }



    void Update()
    {
        if (IsOwner) // only the owning player should send these RPCs
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
    }

    [ServerRpc]
    void ParryServerRPC(ServerRpcParams rpcParams = default)
    {
        // Enable on the server
        parryhitbox.SetActive(true);

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


}
