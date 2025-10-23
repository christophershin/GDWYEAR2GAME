using Alteruna;
using UnityEngine;

public class Parrying : MonoBehaviour
{

    
    public GameObject parryhitbox;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        parryhitbox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        Parry();
    }


    
    void Parry()
    {
        if (Input.GetMouseButtonDown(1))
        {
            parryhitbox.SetActive(true);

            Debug.Log("true");
        }

        if (Input.GetMouseButtonUp(1))
        {
            parryhitbox.SetActive(false);
            Debug.Log("false");
        }
    }
}
