using System.Drawing;
using Unity.Netcode;
using UnityEngine;

public class PlayerCardSystem : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject projectile;
    [SerializeField] private float proj_speed;
    [SerializeField] private Transform cam;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false;
            return;
        }

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {

            ShootServerRPC();

        }

    }


    [ServerRpc]
    void ShootServerRPC()
    {

        string id = GetComponent<EntitiesClass>().TeamID();

        GameObject bullet = Instantiate(projectile, transform);
        bullet.GetComponent<Rigidbody>().linearVelocity = cam.forward * proj_speed;
        bullet.GetComponent<EntitiesClass>().SetTeamID(id);
        bullet.GetComponent<NetworkObject>().Spawn(true);
        
    }

}
