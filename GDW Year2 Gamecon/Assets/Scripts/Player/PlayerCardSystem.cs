using System;
using System.Collections;
using System.Drawing;
using Alteruna;
using Unity.Netcode;
using UnityEngine;

public class PlayerCardSystem : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject puck, grenade, pikeball, knife;
    
    public GameObject projectile;
    [SerializeField] private float proj_speed;
    [SerializeField] private Camera cam;
    public float colliderDisableTime = 0.05f;
    
    private CardsManager _cardsManager;

    private void Start()
    {
        _cardsManager = GameObject.Find("Cards").GetComponent<CardsManager>();
    }

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
            string id = GetComponent<EntitiesClass>().teamID;
            Vector3 direction = cam.transform.forward;
            
            string card = _cardsManager.UseCard();
            if (card == "")
            {
                //ShootServerRPC(direction, id, proj_speed);
                return;
            }
                
            
            
            //ShootServerRPC(direction, id, proj_speed);
            
            switch (card)
            {
                case "Puck":
                    ShootPuckServerRPC(direction, id, proj_speed);
                    // projectile = puck;
                    break;
                case "Grenade":
                    ShootBombServerRPC(direction, id, proj_speed);
                    //projectile = grenade;
                    break;
                case "Pikeball":
                    ShootPikeServerRPC(direction, id, proj_speed);
                    //projectile = pikeball;
                    break;
                case "Knife":
                    ShootKnifeServerRPC(direction, id, proj_speed);
                    //projectile = knife;
                    break;
            }

            

        }

    }


    [ServerRpc]
    void ShootServerRPC(Vector3 shootdirection, string _id, float proj_speed)
    {

        GameObject bullet = Instantiate(projectile, transform.position + shootdirection * 1.5f, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().linearVelocity = shootdirection.normalized * proj_speed;
        bullet.GetComponent<EntitiesClass>().teamID = _id;
        bullet.GetComponent<NetworkObject>().Spawn(true);
        Debug.Log(bullet.GetComponent<EntitiesClass>().teamID);

        if (bullet.GetComponent<EntitiesClass>().teamID == _id)
            StartCoroutine(colliderToggled(bullet.GetComponent<Collider>()));
    }
    
    [ServerRpc]
    void ShootPuckServerRPC(Vector3 shootdirection, string _id, float proj_speed)
    {

        GameObject bullet = Instantiate(puck, transform.position + shootdirection * 1.5f, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().linearVelocity = shootdirection.normalized * proj_speed;
        bullet.GetComponent<EntitiesClass>().teamID = _id;
        bullet.GetComponent<NetworkObject>().Spawn(true);
        Debug.Log(bullet.GetComponent<EntitiesClass>().teamID);

        if (bullet.GetComponent<EntitiesClass>().teamID == _id)
            StartCoroutine(colliderToggled(bullet.GetComponent<Collider>()));
    }
    
    [ServerRpc]
    void ShootPikeServerRPC(Vector3 shootdirection, string _id, float proj_speed)
    {

        GameObject bullet = Instantiate(pikeball, transform.position + shootdirection * 1.5f, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().linearVelocity = shootdirection.normalized * proj_speed;
        bullet.GetComponent<EntitiesClass>().teamID = _id;
        bullet.GetComponent<NetworkObject>().Spawn(true);
        Debug.Log(bullet.GetComponent<EntitiesClass>().teamID);

        if (bullet.GetComponent<EntitiesClass>().teamID == _id)
            StartCoroutine(colliderToggled(bullet.GetComponent<Collider>()));
    }
    
    [ServerRpc]
    void ShootBombServerRPC(Vector3 shootdirection, string _id, float proj_speed)
    {

        GameObject bullet = Instantiate(grenade, transform.position + shootdirection * 1.5f, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().linearVelocity = shootdirection.normalized * proj_speed;
        bullet.GetComponent<EntitiesClass>().teamID = _id;
        bullet.GetComponent<NetworkObject>().Spawn(true);
        Debug.Log(bullet.GetComponent<EntitiesClass>().teamID);

        if (bullet.GetComponent<EntitiesClass>().teamID == _id)
            StartCoroutine(colliderToggled(bullet.GetComponent<Collider>()));
    }
    
    [ServerRpc]
    void ShootKnifeServerRPC(Vector3 shootdirection, string _id, float proj_speed)
    {

        GameObject bullet = Instantiate(knife, transform.position + shootdirection * 1.5f, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().linearVelocity = shootdirection.normalized * proj_speed;
        bullet.GetComponent<EntitiesClass>().teamID = _id;
        bullet.GetComponent<NetworkObject>().Spawn(true);
        Debug.Log(bullet.GetComponent<EntitiesClass>().teamID);

        if (bullet.GetComponent<EntitiesClass>().teamID == _id)
            StartCoroutine(colliderToggled(bullet.GetComponent<Collider>()));
    }


    private IEnumerator colliderToggled(Collider collider)
    {
        collider.enabled = false;

        yield return new WaitForSeconds(colliderDisableTime);

        collider.enabled = true;
    }

}
