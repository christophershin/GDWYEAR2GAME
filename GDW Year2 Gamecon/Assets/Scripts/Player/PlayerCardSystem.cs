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
    
    // Testing
    public float startspeed, midspeed, endspeed;
    public float curve;

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
    
    public static Vector3 RaycastFromCamera(Camera cam, float maxDistance)
    {
        Ray ray = cam.ScreenPointToRay(
            new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f)
        );

        int layerMask = ~LayerMask.GetMask("card");

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, layerMask))
            return hit.point;

        return ray.origin + ray.direction * maxDistance;
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
            
            Vector3 startPos = cam.transform.position;
            Vector3 endPos = RaycastFromCamera(cam, 10000);
            
            
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
        GameObject bullet = SetBullet(shootdirection, projectile);
        
        
        
        //bullet.GetComponent<Rigidbody>().linearVelocity = shootdirection.normalized * proj_speed;
        bullet.GetComponent<EntitiesClass>().teamID = _id;
        bullet.GetComponent<NetworkObject>().Spawn(true);
        Debug.Log(bullet.GetComponent<EntitiesClass>().teamID);

        if (bullet.GetComponent<EntitiesClass>().teamID == _id)
            StartCoroutine(colliderToggled(bullet.GetComponent<Collider>()));
    }
    
    [ServerRpc]
    void ShootPuckServerRPC(Vector3 shootdirection, string _id, float proj_speed)
    {
        GameObject bullet = SetBullet(shootdirection, puck);
        

        //GameObject bullet = Instantiate(puck, transform.position + shootdirection * 1.5f, Quaternion.identity);
        //bullet.GetComponent<Rigidbody>().linearVelocity = shootdirection.normalized * proj_speed;
        bullet.GetComponent<EntitiesClass>().teamID = _id;
        bullet.GetComponent<NetworkObject>().Spawn(true);
        Debug.Log(bullet.GetComponent<EntitiesClass>().teamID);

        if (bullet.GetComponent<EntitiesClass>().teamID == _id)
            StartCoroutine(colliderToggled(bullet.GetComponent<Collider>()));
    }
    
    [ServerRpc]
    void ShootPikeServerRPC(Vector3 shootdirection, string _id, float proj_speed)
    {
        GameObject bullet = SetBullet(shootdirection, pikeball);

        //GameObject bullet = Instantiate(pikeball, transform.position + shootdirection * 1.5f, Quaternion.identity);
        //bullet.GetComponent<Rigidbody>().linearVelocity = shootdirection.normalized * proj_speed;
        bullet.GetComponent<EntitiesClass>().teamID = _id;
        bullet.GetComponent<NetworkObject>().Spawn(true);
        Debug.Log(bullet.GetComponent<EntitiesClass>().teamID);

        if (bullet.GetComponent<EntitiesClass>().teamID == _id)
            StartCoroutine(colliderToggled(bullet.GetComponent<Collider>()));
    }
    
    [ServerRpc]
    void ShootBombServerRPC(Vector3 shootdirection, string _id, float proj_speed)
    {
        GameObject bullet = SetBullet(shootdirection, grenade);

        //GameObject bullet = Instantiate(grenade, transform.position + shootdirection * 1.5f, Quaternion.identity);
        //bullet.GetComponent<Rigidbody>().linearVelocity = shootdirection.normalized * proj_speed;
        bullet.GetComponent<EntitiesClass>().teamID = _id;
        bullet.GetComponent<NetworkObject>().Spawn(true);
        Debug.Log(bullet.GetComponent<EntitiesClass>().teamID);

        if (bullet.GetComponent<EntitiesClass>().teamID == _id)
            StartCoroutine(colliderToggled(bullet.GetComponent<Collider>()));
    }
    
    [ServerRpc]
    void ShootKnifeServerRPC(Vector3 shootdirection, string _id, float proj_speed)
    {

        GameObject bullet = SetBullet(shootdirection, knife);

        //GameObject bullet = Instantiate(knife, transform.position + shootdirection * 1.5f, Quaternion.identity);
        //bullet.GetComponent<Rigidbody>().linearVelocity = shootdirection.normalized * proj_speed;
        bullet.GetComponent<EntitiesClass>().teamID = _id;
        bullet.GetComponent<NetworkObject>().Spawn(true);
        Debug.Log(bullet.GetComponent<EntitiesClass>().teamID);

        if (bullet.GetComponent<EntitiesClass>().teamID == _id)
            StartCoroutine(colliderToggled(bullet.GetComponent<Collider>()));
    }

    GameObject SetBullet(Vector3 shootdirection, GameObject proj)
    {
        Vector3 startpos = cam.transform.position;
        Vector3 endpos = RaycastFromCamera(cam, 10000);
        Vector3 midpos = (startpos + endpos) * 0.5f + Vector3.up * curve;
        
        
        GameObject bullet = Instantiate(proj, transform.position + shootdirection * 1.5f, Quaternion.identity);

        // bullet.GetComponent<projectile>().Shoot(bullet,
        //     startpos,
        //     midpos,
        //     endpos,
        //     startspeed,
        //     midspeed,
        //     endspeed
        // );

        // StartCoroutine(bullet.GetComponent<projectile>()
        //     .MoveProjectile(bullet.gameObject.transform, 
        //         startpos, 
        //         midpos, 
        //         endpos, 
        //         startspeed, 
        //         midspeed, 
        //         endspeed
        //     ));
        
        return bullet;
    }


    private IEnumerator colliderToggled(Collider collider)
    {
        collider.enabled = false;

        yield return new WaitForSeconds(colliderDisableTime);

        collider.enabled = true;
    }

}
