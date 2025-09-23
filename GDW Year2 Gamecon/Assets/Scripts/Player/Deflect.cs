using UnityEngine;

public class Deflect: MonoBehaviour
{












   void OnCollisionEnter(Collision collision)
   {
        if(collision.gameObject.CompareTag("Parriable"))
        {
            GameObject obj = collision.gameObject;
            obj.GetComponent<Rigidbody>().linearVelocity = Camera.main.transform.forward * 8;

        }
   }

}


