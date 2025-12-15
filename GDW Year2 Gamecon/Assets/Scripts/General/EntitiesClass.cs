using System.Drawing;
using Unity.Netcode;
using UnityEngine;

public class EntitiesClass: NetworkBehaviour
{

    public string teamID = "";




    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false;
            return;
        }

        if (teamID == null || teamID == "")
        {
            teamID = gameObject.GetInstanceID().ToString();
        }



    }

    public void SetTeamID(string id)
    {
        teamID = id;
    }

}
