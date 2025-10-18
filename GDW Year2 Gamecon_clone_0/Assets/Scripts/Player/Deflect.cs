using Unity.Netcode;
using UnityEngine;


public class Deflect: NetworkBehaviour
{
    public AudioClip[] sounds;
    private AudioSource ManagerAudio;



    private void Start()
    {

        
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false;
            return;

        }

        ManagerAudio = GetComponent<AudioSource>();
        ManagerAudio.clip = sounds[0];
    }



    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Parriable"))
        {
            GameObject obj = collision.gameObject;
            obj.GetComponent<Rigidbody>().linearVelocity = Camera.main.transform.forward * 8;

        }
            
    }

}


