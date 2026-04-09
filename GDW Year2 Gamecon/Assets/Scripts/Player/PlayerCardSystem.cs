using System;
using System.Collections;
using System.Drawing;
using Alteruna;
using Unity.Netcode;
using UnityEngine;

public class PlayerCardSystem : NetworkBehaviour
{
    public GameObject puck, pikeball, tomato, cone, speaker;
    public GameObject puckVisual, pikeVisual, tomatoVisual, coneVisual, speakerVisual;
    
    public GameObject projectile;
    [SerializeField] private float proj_speed;
    [SerializeField] private Camera cam;
    public float colliderDisableTime = 0.05f;
    
    [SerializeField] private CardsManager _cardsManager;
    
    public float startspeed, midspeed, endspeed;
    public float curve;

    private float _MAXANGLE = 15;
    
    [SerializeField] AnimationController animationController;

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
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        
        float closestAngle = _MAXANGLE;
        int closestPlayer = -1;
        
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == this.gameObject)
            {
                continue;
            }
            
            Vector3 directionToTarget = players[i].transform.position - this.transform.position;
            float angle = Vector3.Angle(camer.transform.forward, directionToTarget);
            
            Debug.Log("closest angle is: " + angle);
            Debug.Log("angle is: " + angle);
            
            if (angle <= closestAngle)
            {
                Debug.Log("ANGLE SMALLER");
                
                Ray ray = camer.ScreenPointToRay(
                    new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f)
                );
                
                
                int layerMask = ~LayerMask.GetMask("card");
                
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
            
            // animation
            animationController.SetAnimation("shooting", true);
            GameObject plr = GetClosestPlayerToCamera(cam);
            
            // visual
            
            Vector3 endpos = RaycastFromCamera(cam, 10000);
            
            if (!IsServer)
            {
                SpawnVisual(card, endpos);
            }
            
            if (plr != this.gameObject && plr.TryGetComponent(out NetworkObject netObj))
            {
                ShootPlayerServerRpc(card, direction, id, proj_speed, netObj.NetworkObjectId, NetworkManager.Singleton.LocalClientId);
                return;
            }
            
            
            ShootStraightServerRpc(card, direction, id, proj_speed, endpos, NetworkManager.Singleton.LocalClientId);
        }
    }

    private void SpawnVisual(string nam, Vector3 endPos)
    {

        GameObject localObj = null;
        
        switch (nam)
        {
            case "Puck":
                localObj = Instantiate(puckVisual, this.transform.position, this.transform.rotation);
                break;
            case  "Pikeball": 
                localObj = Instantiate(pikeVisual, this.transform.position, this.transform.rotation);
                break;
            case  "Tomato": 
                localObj = Instantiate(tomatoVisual, this.transform.position, this.transform.rotation);
                break;
            case  "Cone": 
                localObj = Instantiate(coneVisual, this.transform.position, this.transform.rotation);
                break;
            case  "Speaker": 
                localObj = Instantiate(speakerVisual, this.transform.position, this.transform.rotation);
                break;
        }

        if (localObj != null)
        {
            var smoother = localObj.GetComponentInChildren<VisualProjs>();
            if (smoother != null)
            {
                smoother.StartPrediction(name, endPos);
            }
        }
        
        
    }
    
    
    [ServerRpc]
    void ShootStraightServerRpc(string card, Vector3 shootdirection, string _id, float proj_speed, Vector3 endpos, ulong projOwnId)
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
        
        bullet.GetComponent<projectile>().projOwnId.Value = projOwnId;
        bullet.GetComponent<EntitiesClass>().teamID = _id;
        bullet.GetComponent<NetworkObject>().Spawn(true);

        if (bullet.GetComponent<EntitiesClass>().teamID == _id)
            StartCoroutine(colliderToggled(bullet.GetComponent<Collider>()));
    }
    
    [ServerRpc]
    void ShootPlayerServerRpc(string card, Vector3 shootdirection, string _id, float proj_speed, ulong plr, ulong projOwnId)
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
        
        bullet.GetComponent<projectile>().projOwnId.Value = projOwnId;
        bullet.GetComponent<EntitiesClass>().teamID = _id;
        bullet.GetComponent<NetworkObject>().Spawn(true);

        if (bullet.GetComponent<EntitiesClass>().teamID == _id)
            StartCoroutine(colliderToggled(bullet.GetComponent<Collider>()));
    }
    
    GameObject ActivateBulletToPlayer(Vector3 shootdirection, GameObject proj, ulong plrNetObj)
    {
        Vector3 startpos = cam.transform.position;
        GameObject bullet = Instantiate(proj, transform.position + shootdirection * 1.5f, Quaternion.identity);
        
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
        bullet.GetComponent<projectile>().ShootWithoutTracking(startpos, endpos);
        
        return bullet;
    }

    private IEnumerator colliderToggled(Collider collider)
    {
        collider.enabled = false;
        yield return new WaitForSeconds(colliderDisableTime);
        collider.enabled = true;
    }
}
