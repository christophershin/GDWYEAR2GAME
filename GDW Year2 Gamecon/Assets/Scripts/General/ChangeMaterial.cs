using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{


    public Material anotherMaterial;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Renderer>().material = anotherMaterial;
    }
}
