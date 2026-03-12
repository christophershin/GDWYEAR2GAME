using System;
using System.Collections;
using System.Drawing;
using System.Security.Cryptography;
using Alteruna;
using Unity.Netcode;
using UnityEngine;

public class Deflect2 : NetworkBehaviour
{
    public AudioClip[] sounds;
    private AudioSource ManagerAudio;
    public Camera cam;
    private GameObject obj;
    private float deflectSpeed;
    [SerializeField] private GameObject player;

    public float colliderDisableTime = 0.05f;
    public float startspeed, midspeed, endspeed;
    public float curve;

    private float _MAXANGLE = 15;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false;
            return;
        }

    }


    void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.CompareTag("Parriable"))
        {
            GameObject obj = collision.gameObject;
            Vector3 direction = cam.transform.forward;
            string teamid = player.GetComponent<EntitiesClass>().teamID;

            ulong objId = obj.GetComponent<NetworkObject>().NetworkObjectId;
            deflectSpeed = obj.GetComponent<Rigidbody>().linearVelocity.magnitude;

            if(obj!=null)
                ShootServerRPC(objId, direction, teamid, 1f);

            obj = null;
            obj.GetComponent<NetworkObject>().Despawn(true);


        }
    }



    public static Vector3 RaycastFromCamera(Camera cam, float maxDistance)
    {
        Ray ray = cam.ScreenPointToRay(
            new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f)
        );

        int layerMask = ~LayerMask.GetMask("card");

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, layerMask))
            return hit.point;

        return ray.origin + ray.direction * maxDistance;
    }

    private GameObject GetClosestPlayerToCamera(Camera camer)
    {
        // VERY expensive method change later if this causes too much lag
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // variables to keep track of
        float closestAngle = _MAXANGLE;
        int closestPlayer = -1;

        // get the closest angle player from this player
        for (int i = 0; i < players.Length; i++)
        {
            print(players.Length);
            if (players[i] == this.gameObject)
            {
                continue;
            }
            // getting the angle between this player and all the players in the map
            Vector3 directionToTarget = players[i].transform.position - this.transform.position;
            float angle = Vector3.Angle(this.transform.forward, directionToTarget);

            Debug.Log("closest angle is: " + angle);
            Debug.Log("angle is: " + angle);

            if (angle <= closestAngle)
            {
                Debug.Log("ANGLE SMALLER");
                // make a ray from the player's middle of the screen
                Ray ray = camer.ScreenPointToRay(
                    new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f)
                );

                // make sure the cards can't be hit
                int layerMask = ~LayerMask.GetMask("card");

                // raycasting to see if the player can actually be seen from the camera

                Vector3 direction = players[i].transform.position - this.transform.position;
                float distance = direction.magnitude;
                direction = direction.normalized;

                RaycastHit hit;

                if (Physics.Raycast(this.transform.position, direction, out hit, distance, layerMask))
                {
                    if (hit.transform.gameObject.CompareTag("Parriable"))
                    {
                        continue;
                    }

                    Debug.Log("RAY WORKS");

                    // Debug.Log("TAG CONFIRMED, changed closest player to this player");
                    // closestAngle = angle;
                    // closestPlayer = i;
                    // //comparing if tag is player
                    //
                    string tag = hit.transform.gameObject.tag;
                    Debug.Log(tag);

                    if (tag == "Player")
                    {
                        Debug.Log("TAG CONFIRMED, changed closest player to this player");
                        closestAngle = angle;
                        closestPlayer = i;
                    }
                }
            }
        }

        if (closestPlayer != -1) return players[closestPlayer];

        return this.gameObject;
    }



    [ServerRpc]
    void ShootServerRPC(ulong objId, Vector3 shootdirection, string _id, float proj_speed)
    {

        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(objId, out NetworkObject netObj))
        {
            GameObject obj = netObj.gameObject;
            GameObject bullet;

            bullet = ActivateBullet(shootdirection, obj);

            bullet.GetComponent<EntitiesClass>().teamID = _id;
            bullet.GetComponent<NetworkObject>().Spawn(true);

            if (bullet.GetComponent<EntitiesClass>().teamID == _id)
                StartCoroutine(colliderToggled(bullet.GetComponent<Collider>()));

        }
    }

    GameObject ActivateBullet(Vector3 shootdirection, GameObject proj)
    {
        Vector3 startpos = cam.transform.position;

        GameObject bullet = Instantiate(proj, transform.position + shootdirection * 1.5f, Quaternion.identity);

        GameObject plr = GetClosestPlayerToCamera(cam);
        if (plr == this.gameObject)
        {
            Debug.Log("Player is this.gameobject");
            Vector3 endpos = RaycastFromCamera(cam, 10000);
            bullet.GetComponent<projectile>().ShootWithoutTracking(startpos, endpos);
        }
        else
        {
            Debug.Log("Player is other player");
            bullet.GetComponent<projectile>().ShootWithTracking(plr, startpos);
        }

        return bullet;
    }

    private IEnumerator colliderToggled(Collider collider)
    {
        collider.enabled = false;

        yield return new WaitForSeconds(colliderDisableTime);

        collider.enabled = true;
    }
}
