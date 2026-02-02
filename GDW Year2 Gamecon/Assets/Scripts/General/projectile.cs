using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class projectile : NetworkBehaviour
{


    public float projectileTimerMax = 10;
    [HideInInspector]
    public float projectileTimer;

    public float damage;

    private Rigidbody _rb;
    Coroutine _currentCoroutine;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Performs behavior on the server which then sends data to all clients
    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            enabled = false;
            return;
        }

        projectileTimer = projectileTimerMax;
    }

    // Update is called once per frame
    void Update()
    {

        if (IsServer)
        {
            projectileTimer -= Time.deltaTime;

            if (projectileTimer <= 0)
                GetComponent<NetworkObject>().Despawn(true);
        }
    }
    
    public void Parry(Vector3 newPos, float startSpeed, float midSpeed, float endSpeed)
    {
        if (_currentCoroutine != null)
            StopCoroutine(_currentCoroutine);
        
        //Vector3 dir = new Vector3(1f, 1f, 0f).normalized;


        Vector3 start = transform.position;
        Vector3 mid = (start + newPos) * 0.5f + Vector3.right * 3;


        _currentCoroutine = StartCoroutine(
            MoveProjectile(
                transform,
                start,
                mid,
                newPos,
                startSpeed,
                midSpeed,
                endSpeed
            )
        );
    }

    public void Shoot(GameObject bullet, Vector3 startpos, Vector3 midpos, Vector3 endpos, float startspeed, float midspeed, float endspeed)
    {
        _currentCoroutine = StartCoroutine(MoveProjectile(bullet.gameObject.transform, 
                startpos, 
                midpos, 
                endpos, 
                startspeed, 
                midspeed, 
                endspeed
            ));
    }
    
    public IEnumerator MoveProjectile(
        Transform projectile,
        Vector3 startPos,
        Vector3 midPos,
        Vector3 endPos,
        float startSpeed,
        float midSpeed,
        float endSpeed)
    {
        
        //midPos = (startPos + endPos) * 0.5f + Vector3.right * 3;
        
        float t = 0f;

        while (t < 1f)
        {
            projectile.position = GetCurvedPosition(
                startPos,
                midPos,
                endPos,
                startSpeed,
                midSpeed,
                endSpeed,
                ref t,
                Time.deltaTime
            );

            yield return null;
        }
        
        //Destroy(this.gameObject);
        _rb.useGravity = true;
    }
    
    public static Vector3 GetCurvedPosition(
        Vector3 startPos,
        Vector3 middlePos,
        Vector3 endPos,
        float startSpeed,
        float middleSpeed,
        float endSpeed,
        ref float t,
        float deltaTime)
    {
        // Blend speed across t (0 → start, 0.5 → middle, 1 → end)
        float speed =
            Mathf.Lerp(
                Mathf.Lerp(startSpeed, middleSpeed, t),
                Mathf.Lerp(middleSpeed, endSpeed, t),
                t
            );

        // Advance t using that speed
        t += speed * deltaTime;
        t = Mathf.Clamp01(t);

        // Quadratic Bézier interpolation
        Vector3 a = Vector3.Lerp(startPos, middlePos, t);
        Vector3 b = Vector3.Lerp(middlePos, endPos, t);
        return Vector3.Lerp(a, b, t);
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (IsServer)
        {
            if (other.gameObject.CompareTag("Obstacle"))
            {
                GetComponent<NetworkObject>().Despawn(true);
            }
        }

    }

}
