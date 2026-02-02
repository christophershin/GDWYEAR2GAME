using Unity.Netcode;
using UnityEngine;

public class projectile : NetworkBehaviour
{


    public float projectileTimerMax = 10;
    [HideInInspector]
    public float projectileTimer;

    public float damage;

    // Performs behavior on the server which then sends data to all clients
    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            enabled = false;
            return;
        }

        projectileTimer = projectileTimerMax;
    }

    // Update is called once per frame
    void Update()
    {

        if (IsServer)
        {
            projectileTimer -= Time.deltaTime;

            if (projectileTimer <= 0)
                GetComponent<NetworkObject>().Despawn(true);
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (IsServer)
        {
            if (other.gameObject.CompareTag("Obstacle"))
            {
                GetComponent<NetworkObject>().Despawn(true);
            }
        }

    }

}
