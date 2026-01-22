using Unity.Netcode;
using UnityEngine;

public class DeleteCard : NetworkBehaviour
{
    [ServerRpc(RequireOwnership = false)]
    public void DespawnServerRPC()
    {
        if(IsServer)
            //NetworkObject.Despawn();
            this.gameObject.GetComponent<SpriteRenderer>().sprite = null;
        this.gameObject.name = "null";
    }
}
