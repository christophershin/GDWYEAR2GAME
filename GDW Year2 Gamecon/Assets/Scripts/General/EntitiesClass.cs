using System.Drawing;
using Unity.Netcode;
using UnityEngine;

public class EntitiesClass: NetworkBehaviour
{

    public string teamID;


    private void Awake()
    {
        teamID = gameObject.GetInstanceID().ToString();
    }


    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false;
            return;

        }
    }

    public void SetTeamID(string id)
    {
        teamID = id;
    }

}
