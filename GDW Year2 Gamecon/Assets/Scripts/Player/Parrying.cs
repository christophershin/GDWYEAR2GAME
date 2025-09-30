using Alteruna;
using UnityEngine;

public class Parrying : MonoBehaviour
{

    
    public GameObject parryhitbox;
    private Alteruna.Avatar _avatar;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _avatar = GetComponent<Alteruna.Avatar>();

        if (!_avatar.IsMe)
            return;

        parryhitbox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_avatar.IsMe)
            return;

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
