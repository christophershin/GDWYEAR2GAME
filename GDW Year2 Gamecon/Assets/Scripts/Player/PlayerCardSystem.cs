using System;
using System.Collections;
using System.Drawing;
using Alteruna;
using Unity.Netcode;
using UnityEngine;

public class PlayerCardSystem : NetworkBehaviour
{
    public GameObject puck, pikeball, tomato, cone, speaker;
    
    public GameObject projectile;
    [SerializeField] private float proj_speed;
    [SerializeField] private Camera cam;
    public float colliderDisableTime = 0.05f;
    
    [SerializeField] private CardsManager _cardsManager;
    
    public float startspeed, midspeed, endspeed;
    public float curve;

    private float _MAXANGLE = 5;
    
    [SerializeField] AnimationController animationController;

    private void Start()
    {
        //_cardsManager = GameObject.Find("Cards").GetComponent<CardsManager>();
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
        float closestAngle = _MAXANGLE;
        int closestPlayer = -1;
        
        // get the closest angle player from this player
        for (int i = 0; i < players.Length; i++)
        {
            //print(players.Length);
            if (players[i] == this.gameObject)
            {
                continue;
            }
            // getting the angle between this player and all the players in the map
            Vector3 directionToTarget = players[i].transform.position - this.transform.position;
            float angle = Vector3.Angle(camer.transform.forward, directionToTarget);
            
            Debug.Log("closest angle is: " + angle);
            Debug.Log("angle is: " + angle);
            
            if (angle <= closestAngle)
            {
                Debug.Log("ANGLE SMALLER");
                // make a ray from the player's middle of the screen
                Ray ray = camer.ScreenPointToRay(
                    new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f)
                );
                
                // make sure the cards can't be hit
                int layerMask = ~LayerMask.GetMask("card");
                
                // raycasting to see if the player can actually be seen from the camera
                
                Vector3 direction = players[i].transform.position - this.transform.position;
                float distance = direction.magnitude;
                direction = direction.normalized;
        
                RaycastHit hit;
        
                if (Physics.Raycast(this.transform.position, direction, out hit, distance, layerMask))
                {
                    if (hit.transform.gameObject.CompareTag("Parriable"))
                    {
                        continue;
                    }
                    
                    string tag = hit.transform.gameObject.tag;
                    
                    if (tag == "Player")
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
        if (!IsOwner) return;
        if (Input.GetMouseButtonDown(0))
        {
            string id = GetComponent<EntitiesClass>().teamID;
            Vector3 direction = cam.transform.forward;
            
            string card = _cardsManager.UseCard();
            if (card == "")
            {
                return;
            }
            
            // Vector3 startPos = cam.transform.position;
            // Vector3 endPos = RaycastFromCamera(cam, 10000);
            
            animationController.SetAnimation("shooting", true);
            
            
            GameObject plr = GetClosestPlayerToCamera(cam);

            if (plr != this.gameObject)
            {
                // if (plr.TryGetComponent(out NetworkObject targetNetObj))
                // {
                //     ShootPlayerServerRpc(card, direction, id, proj_speed, targetNetObj);
                //     return;
                // }
                
                if (plr.TryGetComponent(out NetworkObject netObj))
                {
                    ShootPlayerServerRpc(card, direction, id, proj_speed, netObj.NetworkObjectId);
                    return;
                }
                
            }
            
            Vector3 endpos = RaycastFromCamera(cam, 10000);
            ShootStraightServerRpc(card, direction, id, proj_speed, endpos);
        }
    }
    
    [ServerRpc]
    void ShootStraightServerRpc(string card, Vector3 shootdirection, string _id, float proj_speed, Vector3 endpos)
    {
        GameObject bullet;
        
        switch (card)
        {
            case "Puck": bullet = ActivateBulletForward(shootdirection, puck, endpos); break;
            case  "Pikeball": bullet = ActivateBulletForward(shootdirection, pikeball, endpos); break;
            case  "Tomato": bullet = ActivateBulletForward(shootdirection, tomato, endpos); break;
            case  "Cone": bullet = ActivateBulletForward(shootdirection, cone, endpos); break;
            case  "Speaker": bullet = ActivateBulletForward(shootdirection, speaker, endpos); break;
            default: return;
        }
        
        bullet.GetComponent<EntitiesClass>().teamID = _id;
        bullet.GetComponent<NetworkObject>().Spawn(true);

        if (bullet.GetComponent<EntitiesClass>().teamID == _id)
            StartCoroutine(colliderToggled(bullet.GetComponent<Collider>()));
    }
    
    [ServerRpc]
    void ShootPlayerServerRpc(string card, Vector3 shootdirection, string _id, float proj_speed, ulong plr)
    {
        GameObject bullet;
        
        switch (card)
        {
            case "Puck": bullet = ActivateBulletToPlayer(shootdirection, puck, plr); break;
            case  "Pikeball": bullet = ActivateBulletToPlayer(shootdirection, pikeball, plr); break;
            case  "Tomato": bullet = ActivateBulletToPlayer(shootdirection, tomato, plr); break;
            case  "Cone": bullet = ActivateBulletToPlayer(shootdirection, cone, plr); break;
            case  "Speaker": bullet = ActivateBulletToPlayer(shootdirection, speaker, plr); break;
            default: return;
        }
        
        bullet.GetComponent<EntitiesClass>().teamID = _id;
        bullet.GetComponent<NetworkObject>().Spawn(true);

        if (bullet.GetComponent<EntitiesClass>().teamID == _id)
            StartCoroutine(colliderToggled(bullet.GetComponent<Collider>()));
    }
    
    GameObject ActivateBulletToPlayer(Vector3 shootdirection, GameObject proj, ulong plrNetObj)
    {
        Vector3 startpos = cam.transform.position;
        
        GameObject bullet = Instantiate(proj, transform.position + shootdirection * 1.5f, Quaternion.identity);
        
        //GameObject plr = GetClosestPlayerToCamera(cam);
        
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(plrNetObj, out NetworkObject targetNetObj))
        {
            GameObject plr = targetNetObj.gameObject;
            bullet.GetComponent<projectile>().ShootWithTracking(plr, startpos);
        }
        
        return bullet;
    }
    
    GameObject ActivateBulletForward(Vector3 shootdirection, GameObject proj, Vector3 endpos)
    {
        Vector3 startpos = cam.transform.position;
        
        GameObject bullet = Instantiate(proj, transform.position + shootdirection * 1.5f, Quaternion.identity);
        
        //GameObject plr = GetClosestPlayerToCamera(cam);
        
        //Vector3 endpos = RaycastFromCamera(cam, 10000);
        bullet.GetComponent<projectile>().ShootWithoutTracking(startpos, endpos);
        
        return bullet;
    }
    

    // [ServerRpc]
    // void ShootServerRPC(string card, Vector3 shootdirection, string _id, float proj_speed)
    // {
    //     GameObject bullet;
    //     
    //     
    //     
    //     switch (card)
    //     {
    //         case "Puck": bullet = ActivateBullet(shootdirection, puck); break;
    //         case  "Pikeball": bullet = ActivateBullet(shootdirection, pikeball); break;
    //         case  "Tomato": bullet = ActivateBullet(shootdirection, tomato); break;
    //         case  "Cone": bullet = ActivateBullet(shootdirection, cone); break;
    //         case  "Speaker": bullet = ActivateBullet(shootdirection, speaker); break;
    //         default: return;
    //     }
    //     
    //     bullet.GetComponent<EntitiesClass>().teamID = _id;
    //     bullet.GetComponent<NetworkObject>().Spawn(true);
    //
    //     if (bullet.GetComponent<EntitiesClass>().teamID == _id)
    //         StartCoroutine(colliderToggled(bullet.GetComponent<Collider>()));
    // }
    
    
    
    
    
    // GameObject ActivateBullet(Vector3 shootdirection, GameObject proj)
    // {
    //     Vector3 startpos = cam.transform.position;
    //     
    //     GameObject bullet = Instantiate(proj, transform.position + shootdirection * 1.5f, Quaternion.identity);
    //     
    //     GameObject plr = GetClosestPlayerToCamera(cam);
    //     if (plr == this.gameObject)
    //     {
    //         Vector3 endpos = RaycastFromCamera(cam, 10000);
    //         bullet.GetComponent<projectile>().ShootWithoutTracking(startpos, endpos);
    //     }
    //     else
    //     {
    //         Debug.Log("Player is other player");
    //         bullet.GetComponent<projectile>().ShootWithTracking(plr, startpos);
    //     }
    //     
    //     return bullet;
    // }

    private IEnumerator colliderToggled(Collider collider)
    {
        collider.enabled = false;

        yield return new WaitForSeconds(colliderDisableTime);

        collider.enabled = true;
    }
}
