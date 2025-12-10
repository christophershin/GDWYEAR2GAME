using Unity.Netcode;
using UnityEngine;

public class DeleteCard : NetworkBehaviour
{
    [ServerRpc(RequireOwnership = false)]
    public void DespawnServerRPC()
    {
        if(IsServer)
            NetworkObject.Despawn();
    }
}
