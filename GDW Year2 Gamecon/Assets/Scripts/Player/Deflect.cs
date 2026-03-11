using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;


public class Deflect : NetworkBehaviour
{
    public AudioClip[] sounds;
    private AudioSource ManagerAudio;
    public GameObject cam;
    private GameObject obj;
    private float deflectSpeed;
    [SerializeField] private GameObject player;

    private float _MAXANGLE = 30;
    
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
            
            GameObject plr = GetClosestPlayerToCamera(cam.GetComponent<Camera>());
        
            if (plr == this.gameObject)
            {
                Vector3 newPos = RaycastFromCamera(cam.GetComponent<Camera>(), 10000);
                
                DeflectServerRPC(objId, direction, 14, teamid, newPos);
                
                //StraightParryServerRPC(newPos);
            }
            else
            {
                NetworkObject netObj = plr.GetComponent<NetworkObject>();
                ulong targetId = netObj.OwnerClientId;
                //TrackedParryServerRPC(targetId);
                
                DeflectTrackedServerRPC(objId, direction, 14, teamid, targetId);
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
        // VERY expensive method change later if this causes too much lag
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        
        // variables to keep track of
        float closestAngle = _MAXANGLE;
        int closestPlayer = -1;
        
        // get the closest angle player from this player
        for (int i = 0; i < players.Length; i++)
        {
            print(players.Length);
            if (players[i] == this.gameObject)
            {
                continue;
            }
            // getting the angle between this player and all the players in the map
            Vector3 directionToTarget = players[i].transform.position - this.transform.position;
            float angle = Vector3.Angle(this.transform.forward, directionToTarget);
            
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
                    
                    Debug.Log("RAY WORKS");
                    
                    // Debug.Log("TAG CONFIRMED, changed closest player to this player");
                    // closestAngle = angle;
                    // closestPlayer = i;
                    // //comparing if tag is player
                    //
                    string tag = hit.transform.gameObject.tag;
                    Debug.Log(tag);
                    
                    if (tag == "Player")
                    {
                        Debug.Log("TAG CONFIRMED, changed closest player to this player");
                        closestAngle = angle;
                        closestPlayer = i;
                    }
                }
            }
        }

        if (closestPlayer != -1) return players[closestPlayer];
        
        return this.gameObject;
    }

    // void Parry()
    // {
    //     GameObject player = GetClosestPlayerToCamera(cam.GetComponent<Camera>());
    //     
    //     if (player == this.gameObject)
    //     {
    //         Vector3 newPos = RaycastFromCamera(cam.GetComponent<Camera>(), 10000);
    //         StraightParryServerRPC(newPos);
    //     }
    //     else
    //     {
    //         NetworkObject netObj = player.GetComponent<NetworkObject>();
    //         ulong targetId = netObj.OwnerClientId;
    //         TrackedParryServerRPC(targetId);
    //     }
    // }
    
    // [ServerRpc]
    // void StraightParryServerRPC(Vector3 newPos)
    // {
    //     obj.GetComponent<projectile>().StraightParry(newPos);
    // }
    //
    // [ServerRpc]
    // void TrackedParryServerRPC(ulong targetID)
    // {
    //     obj.GetComponent<projectile>().TrackedParry(targetID);
    // }
    
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
                 player.GetComponent<HealthandShield>().getShieldServerRPC(30);
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
                // parry
                obj.GetComponent<projectile>().TrackedParry(targetID);
                 
                obj.GetComponent<EntitiesClass>().teamID = id;
                obj.GetComponent<projectile>().projectileTimer = obj.GetComponent<projectile>().projectileTimerMax;
                player.GetComponent<HealthandShield>().getShieldServerRPC(30);
            }
        }
    }
}


