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

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false;
            return;

        }

        ManagerAudio = GetComponent<AudioSource>();
        ManagerAudio.clip = sounds[0];

        //RPCparams(new ServerRpcParams { Receive = new ServerRpcReceiveParams { }, Send = new ServerRpcSendParams { } });
    }

    void OnTriggerEnter(Collider collision)
    {
        // Only the client with the camera should trigger this
        if (!IsOwner)
            return;

        if (collision.gameObject.CompareTag("Parriable"))
        {
            GameObject obj = collision.gameObject;
            Vector3 direction = cam.transform.forward; 
            string teamid = player.GetComponent<EntitiesClass>().teamID;
            ulong objId = obj.GetComponent<NetworkObject>().NetworkObjectId;
            deflectSpeed = obj.GetComponent<Rigidbody>().linearVelocity.magnitude;

            // Tell the server to handle the deflect
            DeflectServerRpc(objId, direction, 10, teamid);
        }
    }


    [ServerRpc(RequireOwnership = false)]
    void DeflectServerRpc(ulong objId, Vector3 dir, float deflectSpeed, string id)
    {
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(objId, out NetworkObject netObj))
        {
            GameObject obj = netObj.gameObject;

            if (obj.GetComponent<EntitiesClass>().teamID != id)
            {
                obj.transform.position = transform.position; // or the hit point, not +dir
                obj.transform.rotation = Quaternion.LookRotation(dir);
                obj.GetComponent<Rigidbody>().linearVelocity = dir.normalized * deflectSpeed;
                obj.GetComponent<EntitiesClass>().teamID = id;
                obj.GetComponent<projectile>().projectileTimer = obj.GetComponent<projectile>().projectileTimerMax;
            }
        }
    }

    //void RPCparams(ServerRpcParams paramters)
    //{

    //}


}


