using System.Drawing;
using Unity.Netcode;
using UnityEngine;

public class EntitiesClass: NetworkBehaviour
{

    public string teamID = "";

    [HideInInspector]
    public NetworkVariable<float> Health = new NetworkVariable<float>(); 


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

        if (IsServer)
        {
            Health.Value = 100f;
        }

    }

    public void SetTeamID(string id)
    {
        teamID = id;
    }



    private void OnCollisionEnter(Collision collider)
    {

        if (!IsServer)
        {
            return;
        }

        if (collider.gameObject.CompareTag("Parriable") && collider.gameObject.GetComponent<EntitiesClass>().teamID != teamID)
        {
            Health.Value -= 20f;
            Debug.Log(Health.Value);
        }
    }


}
