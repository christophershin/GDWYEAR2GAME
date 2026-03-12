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
            GameObject[] plr = GameObject.FindGameObjectsWithTag("Player");
            string id = GetComponent<EntitiesClass>().teamID;

            GameObject bullet = Instantiate(proj, transform);
            bullet.GetComponent<EntitiesClass>().teamID = id;
            bullet.transform.position = transform.position;
            bullet.GetComponent<NetworkObject>().Spawn(true);
            bullet.GetComponent<projectile>().damage = 0;

            float smallestDis = 100000f;
            int ind;

            for (int i=0; i<plr.Length; i++)
            {
                    
                float dis = Vector2.Distance(transform.position, plr[i].transform.position);

                if (dis < smallestDis)
                {
                    smallestDis = dis;
                }
                    
                    
                if (plr[i] != this.gameObject)
                {
                    bullet.GetComponent<projectile>().ShootWithTracking(plr[i], transform.position);
                }

            }
                

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
