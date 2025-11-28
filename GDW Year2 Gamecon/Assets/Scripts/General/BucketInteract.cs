using System.Collections.Generic;
using UnityEngine;

public class BucketInteract : MonoBehaviour
{
    public GameObject pressE;
    public List<GameObject> Torches;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pressE.SetActive(false);
    }


    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            pressE.SetActive(true);
            if (Input.GetKey(KeyCode.E))
            {
                for (int k = 0; k < Torches.Count; k++)
                {
                    Torches[k].SetActive(false);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            for (int k = 0; k < Torches.Count; k++)
            {
                pressE.SetActive(false);
            }
        }

    }
}
