using System.Threading;
using UnityEngine;
using Unity.Netcode;
using System.Collections;

[System.Serializable]
public class EnemyFiring : NetworkBehaviour
{

    public float timerMax = 1;
    private float timer = 0;
    public float projectile_speed = 5;
    public GameObject proj;
    public float colliderDisableTime = 0.05f;

    public Transform playerbody;

    public override void OnNetworkSpawn()
    {
        if(!IsServer)
        {
            enabled = false;
            return;
        }
        
    }


    void Update()
    {
        if (!IsServer)
        {
            return;
        }


        timer -= Time.deltaTime;

            if (timer <= 0)
            {

                string id = GetComponent<EntitiesClass>().teamID;

                GameObject bullet = Instantiate(proj, transform);
                bullet.GetComponent<Rigidbody>().linearVelocity = new Vector3(0, 0, -projectile_speed);
                bullet.GetComponent<EntitiesClass>().teamID = id;
                bullet.GetComponent<NetworkObject>().Spawn(true);

                if (bullet.GetComponent<EntitiesClass>().teamID == id)
                    StartCoroutine(colliderToggled(bullet.GetComponent<Collider>()));
                    
                timer = timerMax;
            }
        

    }

    private IEnumerator colliderToggled(Collider collider)
    {
        collider.enabled = false;

        yield return new WaitForSeconds(colliderDisableTime);

        collider.enabled = true;
    }
}
