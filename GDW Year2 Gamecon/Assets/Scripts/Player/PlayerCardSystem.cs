using System;
using System.Collections;
using System.Drawing;
using Alteruna;
using Unity.Netcode;
using UnityEngine;

public class PlayerCardSystem : NetworkBehaviour
{
    public GameObject puck, grenade, pikeball, knife;
    
    public GameObject projectile;
    [SerializeField] private float proj_speed;
    [SerializeField] private Camera cam;
    public float colliderDisableTime = 0.05f;
    
    private CardsManager _cardsManager;
    
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

    private GameObject GetClosestPlayerToCamera(Camera camer)
    {
        // VERY expensive method change later if this causes too much lag
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        
        // variables to keep track of
        float closestAngle = 30;
        int closestPlayer = -1;
        
        // get the closest angle player from this player
        for (int i = 0; i < players.Length; i++)
        {
            // getting the angle between this player and all the players in the map
            Vector3 directionToTarget = players[i].transform.position - this.transform.position;
            float angle = Vector3.Angle(this.transform.forward, directionToTarget);

            if (angle < closestAngle)
            {
                // make a ray from the player's middle of the screen
                Ray ray = camer.ScreenPointToRay(
                    new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f)
                );
                
                // make sure the cards can't be hit
                int layerMask = ~LayerMask.GetMask("card");
                
                // raycasting to see if the player can actually be seen from the camera
                if (Physics.Raycast(ray, out RaycastHit hit, 10000, layerMask))
                {
                    // comparing if tag is player
                    if (hit.transform.CompareTag("Player"))
                    {
                        closestAngle = angle;
                        closestPlayer = i;
                    }
                }
            }
        }

        if (closestPlayer != -1) return players[closestPlayer];
        
        return this.gameObject;
    }
    
    void Update()
    {
    
        if (Input.GetMouseButtonDown(0))
        {
            string id = GetComponent<EntitiesClass>().teamID;
            Vector3 direction = cam.transform.forward;
            
            string card = _cardsManager.UseCard();
            if (card == "")
            {
                return;
            }
            
            Vector3 startPos = cam.transform.position;
            Vector3 endPos = RaycastFromCamera(cam, 10000);
            
            ShootServerRPC(card, direction, id, proj_speed);
            
            // switch (card)
            // {
            //     case "Puck":
            //         ShootPuckServerRPC(direction, id, proj_speed);
            //         break;
            //     case "Grenade":
            //         ShootBombServerRPC(direction, id, proj_speed);
            //         break;
            //     case "Pikeball":
            //         ShootPikeServerRPC(direction, id, proj_speed);
            //         break;
            //     case "Knife":
            //         ShootKnifeServerRPC(direction, id, proj_speed);
            //         break;
            // }
        }
    }

    [ServerRpc]
    void ShootServerRPC(string card, Vector3 shootdirection, string _id, float proj_speed)
    {
        GameObject bullet;
        switch (card)
        {
            case "Puck":
                bullet = ActivateBullet(shootdirection, puck);
                break;
            case  "Grenade":
                bullet = ActivateBullet(shootdirection, grenade);
                break;
            case  "Pikeball":
                bullet = ActivateBullet(shootdirection, pikeball);
                break;
            case  "Knife":
                bullet = ActivateBullet(shootdirection, knife);
                break;
            default:
                return;
        }
        
        bullet.GetComponent<EntitiesClass>().teamID = _id;
        bullet.GetComponent<NetworkObject>().Spawn(true);
        Debug.Log(bullet.GetComponent<EntitiesClass>().teamID);

        if (bullet.GetComponent<EntitiesClass>().teamID == _id)
            StartCoroutine(colliderToggled(bullet.GetComponent<Collider>()));
    }
    
    GameObject ActivateBullet(Vector3 shootdirection, GameObject proj)
    {
        Vector3 startpos = cam.transform.position;
        
        // Vector3 midpos = (startpos + endpos) * 0.5f + Vector3.up * curve;
        
        GameObject bullet = Instantiate(proj, transform.position + shootdirection * 1.5f, Quaternion.identity);
        
        GameObject plr = GetClosestPlayerToCamera(cam);
        if (plr == this.gameObject)
        {
            Vector3 endpos = RaycastFromCamera(cam, 10000);
            bullet.GetComponent<projectile>().ShootWithoutTracking(startpos, endpos);
        }
        else
        {
            bullet.GetComponent<projectile>().ShootWithTracking(plr, startpos);
        }
        
        return bullet;
    }

    private IEnumerator colliderToggled(Collider collider)
    {
        collider.enabled = false;

        yield return new WaitForSeconds(colliderDisableTime);

        collider.enabled = true;
    }

    // [ServerRpc]
    // void OldShootServerRPC(Vector3 shootdirection, string _id, float proj_speed)
    // {
    //     GameObject bullet = SetBullet(shootdirection, projectile);
    //     
    //     bullet.GetComponent<EntitiesClass>().teamID = _id;
    //     bullet.GetComponent<NetworkObject>().Spawn(true);
    //     Debug.Log(bullet.GetComponent<EntitiesClass>().teamID);
    //
    //     if (bullet.GetComponent<EntitiesClass>().teamID == _id)
    //         StartCoroutine(colliderToggled(bullet.GetComponent<Collider>()));
    // }
    //
    // [ServerRpc]
    // void ShootPuckServerRPC(Vector3 shootdirection, string _id, float proj_speed)
    // {
    //     GameObject bullet = SetBullet(shootdirection, puck);
    //     
    //     bullet.GetComponent<EntitiesClass>().teamID = _id;
    //     bullet.GetComponent<NetworkObject>().Spawn(true);
    //     Debug.Log(bullet.GetComponent<EntitiesClass>().teamID);
    //
    //     if (bullet.GetComponent<EntitiesClass>().teamID == _id)
    //         StartCoroutine(colliderToggled(bullet.GetComponent<Collider>()));
    // }
    //
    // [ServerRpc]
    // void ShootPikeServerRPC(Vector3 shootdirection, string _id, float proj_speed)
    // {
    //     GameObject bullet = SetBullet(shootdirection, pikeball);
    //
    //     bullet.GetComponent<EntitiesClass>().teamID = _id;
    //     bullet.GetComponent<NetworkObject>().Spawn(true);
    //     Debug.Log(bullet.GetComponent<EntitiesClass>().teamID);
    //
    //     if (bullet.GetComponent<EntitiesClass>().teamID == _id)
    //         StartCoroutine(colliderToggled(bullet.GetComponent<Collider>()));
    // }
    //
    // [ServerRpc]
    // void ShootBombServerRPC(Vector3 shootdirection, string _id, float proj_speed)
    // {
    //     GameObject bullet = SetBullet(shootdirection, grenade);
    //
    //     bullet.GetComponent<EntitiesClass>().teamID = _id;
    //     bullet.GetComponent<NetworkObject>().Spawn(true);
    //     Debug.Log(bullet.GetComponent<EntitiesClass>().teamID);
    //
    //     if (bullet.GetComponent<EntitiesClass>().teamID == _id)
    //         StartCoroutine(colliderToggled(bullet.GetComponent<Collider>()));
    // }
    //
    // [ServerRpc]
    // void ShootKnifeServerRPC(Vector3 shootdirection, string _id, float proj_speed)
    // {
    //
    //     GameObject bullet = SetBullet(shootdirection, knife);
    //     
    //     bullet.GetComponent<EntitiesClass>().teamID = _id;
    //     bullet.GetComponent<NetworkObject>().Spawn(true);
    //     Debug.Log(bullet.GetComponent<EntitiesClass>().teamID);
    //
    //     if (bullet.GetComponent<EntitiesClass>().teamID == _id)
    //         StartCoroutine(colliderToggled(bullet.GetComponent<Collider>()));
    // }

    

}
