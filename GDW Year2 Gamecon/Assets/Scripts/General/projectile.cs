using Unity.Netcode;
using UnityEngine;

public class projectile : NetworkBehaviour
{


    [SerializeField] private float projectileTimerMax = 10;
    private float projectileTimer;

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

        projectileTimer -= Time.deltaTime;

        if (projectileTimer <= 0)
            Destroy(this.gameObject);



    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<EntitiesClass>().TeamID() == GetComponent<EntitiesClass>().TeamID())
        {
            GetComponent<SphereCollider>().enabled = false;
        } 
    }

}
