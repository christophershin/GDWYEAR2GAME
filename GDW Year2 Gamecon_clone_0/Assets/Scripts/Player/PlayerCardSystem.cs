using System.Drawing;
using Unity.Netcode;
using UnityEngine;

public class PlayerCardSystem : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject projectile;
    [SerializeField] private float proj_speed;
    [SerializeField] private Camera cam;

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
        

        if(Input.GetMouseButtonDown(0))
        {

            

            GameObject bullet = Instantiate(projectile, transform);
            bullet.GetComponent<Rigidbody>().linearVelocity = Camera.main.transform.forward * proj_speed;
            bullet.GetComponent<SphereCollider>().enabled = false;
            bullet.GetComponent<NetworkObject>().Spawn(true);
        }





    }
}
