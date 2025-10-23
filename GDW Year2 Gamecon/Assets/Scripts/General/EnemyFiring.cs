using System.Threading;
using UnityEngine;

[System.Serializable]
public class EnemyFiring : MonoBehaviour
{

    public float timerMax = 1;
    private float timer = 0;
    public float projectile_speed = 5;
    public GameObject proj;

    public Transform playerbody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;


        if (timer <= 0)
        {
            GameObject bullet = Instantiate(proj, transform);
            bullet.transform.position = transform.position + new Vector3(0, 1, 0);
            bullet.GetComponent<Rigidbody>().linearVelocity = new Vector3(-projectile_speed, 0, 0);
            timer = timerMax;
        }

    }
}
