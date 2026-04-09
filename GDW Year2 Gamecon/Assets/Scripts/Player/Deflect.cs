using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;


public class Deflect : NetworkBehaviour
{
    public AudioClip[] sounds;
    private AudioSource ManagerAudio;
    public Camera cam;
    private GameObject obj;
    private float deflectSpeed;
    [SerializeField] private GameObject player;

    private float _MAXANGLE = 30;
    
    [SerializeField] private AnimationController _animator;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false;
            return;

        }

        ManagerAudio = GetComponent<AudioSource>();


        //RPCparams(new ServerRpcParams { Receive = new ServerRpcReceiveParams { }, Send = new ServerRpcSendParams { } });
    }

    void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.CompareTag("Parriable"))
        {
            GameObject obj = collision.gameObject;
            Vector3 direction = cam.transform.forward; 
            string teamid = player.GetComponent<EntitiesClass>().teamID;
            ulong objId = obj.GetComponent<NetworkObject>().NetworkObjectId;
            deflectSpeed = obj.GetComponent<Rigidbody>().linearVelocity.magnitude;

            // Tell the server to handle the deflect
            
            GameObject plr = GetClosestPlayerToCamera(cam);
            
            _animator.SetAnimation("parry", true);
        
            if (plr != this.gameObject && plr.TryGetComponent(out NetworkObject netObj))
            {
                DeflectTrackedServerRpc(objId, direction, 14, teamid, netObj.NetworkObjectId);
                player.GetComponent<HealthandShield>().GetShieldServerRpc(30);
            }
            else
            {
                Vector3 newPos = RaycastFromCamera(cam, 10000);
                
                DeflectServerRpc(objId, direction, 14, teamid, newPos);
                player.GetComponent<HealthandShield>().GetShieldServerRpc(30);
            }
            
            // if(obj.GetComponent<EntitiesClass>().teamID != teamid)
            // {
            //     //play sound
            //     ManagerAudio.clip = sounds[0];
            //     ManagerAudio.Play();
            // }

        }
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
    
    public static Vector3 RaycastFromCamera(Camera cam, float maxDistance)
    {
        Ray ray = cam.ScreenPointToRay(
            new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f)
        );

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
            return hit.point;

        return ray.origin + ray.direction * maxDistance;
    }
    
    [ServerRpc]
     void DeflectServerRpc(ulong objId, Vector3 dir, float deflectSpeed, string id, Vector3 newPos)
     {
    
         if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(objId, out NetworkObject netObj))
         {
             GameObject objj = netObj.gameObject;
    
             if (objj.GetComponent<EntitiesClass>().teamID != id)
             {
                 objj.GetComponent<projectile>().StraightParry(newPos);
                 objj.GetComponent<EntitiesClass>().teamID = id;
                 objj.GetComponent<projectile>().projectileTimer = objj.GetComponent<projectile>().projectileTimerMax;
             }
         }
     }

    [ServerRpc]
    void DeflectTrackedServerRpc(ulong objId, Vector3 dir, float deflectSpeed, string id, ulong plrNetObj)
    {
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(objId, out NetworkObject netObj))
        {
            GameObject objj = netObj.gameObject;
    
            if (objj.GetComponent<EntitiesClass>().teamID != id)
            {
                if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(plrNetObj, out NetworkObject targetNetObj))
                {
                    GameObject plr = targetNetObj.gameObject;
                    objj.GetComponent<projectile>().TrackedParry(plr);
                }
                
                
                objj.GetComponent<EntitiesClass>().teamID = id;
                objj.GetComponent<projectile>().projectileTimer = objj.GetComponent<projectile>().projectileTimerMax;
            }
            
            
            
            
            // if (obj.GetComponent<EntitiesClass>().teamID != id)
            // {
            //     // if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(plr,
            //     //         out NetworkObject targetNetObj))
            //     // {
            //     //     GameObject plr = targetNetObj.gameObject;
            //     //     obj.GetComponent<projectile>().TrackedParry(plrs);
            //     // }
            //     
            //     obj.GetComponent<EntitiesClass>().teamID = id;
            //     obj.GetComponent<projectile>().projectileTimer = obj.GetComponent<projectile>().projectileTimerMax;
            // }
        }
    }
}