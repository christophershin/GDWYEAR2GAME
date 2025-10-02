using System.Threading;
using UnityEngine;
using Unity.Netcode;

[System.Serializable]
public class EnemyFiring : NetworkBehaviour
{

    public float timerMax = 1;
    private float timer = 0;
    public float projectile_speed = 5;
    public GameObject proj;

    public Transform playerbody;

    public override void OnNetworkSpawn()
    {
        if(!IsServer)
        {
            enabled = false;
            return;
        }

        Debug.Log("fire");
    }



    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;


        if (timer <= 0)
        {
            GameObject bullet = Instantiate(proj, transform);
            bullet.GetComponent<Rigidbody>().linearVelocity = new Vector3(0, 0, -projectile_speed);
            timer = timerMax;
        }

    }
}
