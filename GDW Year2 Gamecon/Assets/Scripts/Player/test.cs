using Unity.Netcode;
using UnityEngine;

public class test : NetworkBehaviour
{

    public Transform cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public override void OnNetworkSpawn()
    {
        if(!IsOwner)
        {
            enabled = false;
            return;
        }
    }




    // Update is called once per frame
    void Update()
    {
       transform.position = cam.forward;
    }
}
