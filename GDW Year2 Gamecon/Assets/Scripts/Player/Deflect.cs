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

    private void Start()
    {
        //_animator = GetComponent<AnimationController>();
    }

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
            
            GameObject plr = GetClosestPlayerToCamera();
            
            _animator.SetAnimation("parry", true);
        
            if (plr == this.gameObject)
            {
                Vector3 newPos = RaycastFromCamera(cam, 10000);
                
                DeflectServerRPC(objId, direction, 14, teamid, newPos);
                player.GetComponent<HealthandShield>().getShieldServerRPC(30);
            }
            else
            {
                NetworkObject netObj = plr.GetComponent<NetworkObject>();
                ulong targetId = netObj.OwnerClientId;
                
                DeflectTrackedServerRPC(objId, direction, 14, teamid, targetId);
                player.GetComponent<HealthandShield>().getShieldServerRPC(30);
            }
            
            // if(obj.GetComponent<EntitiesClass>().teamID != teamid)
            // {
            //     //play sound
            //     ManagerAudio.clip = sounds[0];
            //     ManagerAudio.Play();
            // }

        }
    }
    
    private GameObject GetClosestPlayerToCamera()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject bestTarget = this.gameObject;
        float closestAngle = _MAXANGLE;
        int layerMask = ~LayerMask.GetMask("card");

        foreach (GameObject player in players)
        {
            if (player == gameObject) continue;

            Vector3 direction = player.transform.position - transform.position;
            float angle = Vector3.Angle(transform.forward, direction);

            if (angle < closestAngle)
            {
                float distance = direction.magnitude;
                
                if (Physics.Raycast(cam.transform.position, direction.normalized, out RaycastHit hit, distance, layerMask))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        closestAngle = angle;
                        bestTarget = player;
                    }
                }
            }
        }

        return bestTarget;
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
     void DeflectServerRPC(ulong objId, Vector3 dir, float deflectSpeed, string id, Vector3 newPos)
     {
    
         if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(objId, out NetworkObject netObj))
         {
             GameObject obj = netObj.gameObject;
    
             if (obj.GetComponent<EntitiesClass>().teamID != id)
             {
                 // parry
                 obj.GetComponent<projectile>().StraightParry(newPos);
                 
                 obj.GetComponent<EntitiesClass>().teamID = id;
                 obj.GetComponent<projectile>().projectileTimer = obj.GetComponent<projectile>().projectileTimerMax;
                 
             }
         }
     }

    [ServerRpc]
    void DeflectTrackedServerRPC(ulong objId, Vector3 dir, float deflectSpeed, string id, ulong targetID)
    {
    
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(objId, out NetworkObject netObj))
        {
            GameObject obj = netObj.gameObject;
    
            if (obj.GetComponent<EntitiesClass>().teamID != id)
            {
                obj.GetComponent<projectile>().TrackedParry(targetID);
                 
                obj.GetComponent<EntitiesClass>().teamID = id;
                obj.GetComponent<projectile>().projectileTimer = obj.GetComponent<projectile>().projectileTimerMax;
                
            }
        }
    }
}


// private GameObject GetClosestPlayerToCamera(Camera camer)
    // {
    //     GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
    //     
    //     // variables to keep track of
    //     float closestAngle = _MAXANGLE;
    //     int closestPlayer = -1;
    //     
    //     for (int i = 0; i < players.Length; i++)
    //     {
    //         print(players.Length);
    //         if (players[i] == this.gameObject)
    //         {
    //             continue;
    //         }
    //         
    //         Vector3 directionToTarget = players[i].transform.position - this.transform.position;
    //         float angle = Vector3.Angle(this.transform.forward, directionToTarget);
    //         
    //         Debug.Log("closest angle is: " + angle);
    //         Debug.Log("angle is: " + angle);
    //         
    //         if (angle <= closestAngle)
    //         {
    //             Ray ray = camer.ScreenPointToRay(
    //                 new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f)
    //             );
    //             
    //             int layerMask = ~LayerMask.GetMask("card");
    //             
    //             Vector3 direction = players[i].transform.position - this.transform.position;
    //             float distance = direction.magnitude;
    //             direction = direction.normalized;
    //     
    //             RaycastHit hit;
    //     
    //             if (Physics.Raycast(this.transform.position, direction, out hit, distance, layerMask))
    //             {
    //                 if (hit.transform.gameObject.CompareTag("Parriable"))
    //                 {
    //                     continue;
    //                 }
    //                 
    //                 string tag = hit.transform.gameObject.tag;
    //                 Debug.Log(tag);
    //                 
    //                 if (tag == "Player")
    //                 {
    //                     closestAngle = angle;
    //                     closestPlayer = i;
    //                 }
    //             }
    //         }
    //     }
    //
    //     if (closestPlayer != -1) return players[closestPlayer];
    //     
    //     return this.gameObject;
    // }