using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;


public class Deflect: NetworkBehaviour
{
    public AudioClip[] sounds;
    private AudioSource ManagerAudio;
    public GameObject cam;
    private GameObject obj;
    [SerializeField] GameObject player;

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


    private void Update()
    {
        transform.Rotate(cam.transform.forward);
    }



    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Parriable"))
        {

            obj = collision.gameObject;
            Vector3 direction = cam.transform.forward;
            deflectServerRPC(direction, 9);

        }
            
    }


    [ServerRpc(RequireOwnership = false)]
    void deflectServerRPC(Vector3 dir, float deflectSpeed)
    {
        if(obj !=null)
        {
            if(obj.GetComponent<EntitiesClass>().teamID != player.GetComponent<EntitiesClass>().teamID)
            {
                obj.transform.position = transform.position + dir;
                obj.transform.rotation = Quaternion.identity;
                obj.GetComponent<Rigidbody>().linearVelocity = dir.normalized * deflectSpeed;
            }

        }
    }

    //void RPCparams(ServerRpcParams paramters)
    //{

    //}


}


