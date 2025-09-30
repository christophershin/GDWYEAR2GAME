using UnityEngine;
using Alteruna;
using Avatar = Alteruna.Avatar;

public class Deflect: AttributesSync
{
    [SerializeField] private AudioClip[] sounds;
    private AudioSource ManagerAudio;

    [SerializeField] private Alteruna.Avatar _avatar;



    private void Start()
    {

        if (!_avatar.IsMe)
            return;

        ManagerAudio = GetComponent<AudioSource>();
    }





    void OnCollisionEnter(Collision collision)
   {
        if(collision.gameObject.CompareTag("Parriable"))
        {
            GameObject obj = collision.gameObject;
            obj.GetComponent<Rigidbody>().linearVelocity = Camera.main.transform.forward * 8;
            ManagerAudio.Play();
        }
   }

}


