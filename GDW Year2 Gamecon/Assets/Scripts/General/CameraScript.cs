using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class CameraScript : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {

            gameObject.SetActive(false);
            return;

        }

    }
}
