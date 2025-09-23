using UnityEngine;

public class Deflect: MonoBehaviour
{
    [SerializeField] private AudioClip[] sounds;
    private AudioSource ManagerAudio;




    private void Start()
    {
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


