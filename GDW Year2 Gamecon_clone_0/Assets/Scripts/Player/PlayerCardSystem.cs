using System.Collections;
using System.Drawing;
using Alteruna;
using Unity.Netcode;
using UnityEngine;

public class PlayerCardSystem : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject projectile;
    [SerializeField] private float proj_speed;
    [SerializeField] private Transform cam;
    public float colliderDisableTime = 0.05f;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false;
            return;
        }

        Debug.Log(GetComponent<EntitiesClass>().teamID);
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

        string id = GetComponent<EntitiesClass>().teamID;

        GameObject bullet = Instantiate(projectile, transform);
        bullet.GetComponent<Rigidbody>().linearVelocity = cam.forward * proj_speed;
        bullet.GetComponent<EntitiesClass>().teamID = id;
        bullet.GetComponent<NetworkObject>().Spawn(true);
        Debug.Log(bullet.GetComponent<EntitiesClass>().teamID);

        if (bullet.GetComponent<EntitiesClass>().teamID == id)
            StartCoroutine(colliderToggled(bullet.GetComponent<Collider>()));
            

    }


    private IEnumerator colliderToggled(Collider collider)
    {
        collider.enabled = false;

        yield return new WaitForSeconds(colliderDisableTime);

        collider.enabled = true;
    }

}
