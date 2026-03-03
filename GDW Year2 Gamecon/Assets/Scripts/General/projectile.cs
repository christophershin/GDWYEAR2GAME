using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class projectile : NetworkBehaviour
{
    // Projectile Stats
    [SerializeField] private float startSpeed, midSpeed, endSpeed, curve;


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


        // _currentCoroutine = StartCoroutine(
        //     MoveProjectile(
        //         transform,
        //         start,
        //         mid,
        //         newPos,
        //         startSpeed,
        //         midSpeed,
        //         endSpeed
        //     )
        // );
    }

    public void ShootWithTracking(GameObject player, Vector3 startpos)
    {
        _currentCoroutine = StartCoroutine(MoveProjectileToPlayer(startpos, player));
    }
    
    public IEnumerator MoveProjectileToPlayer(Vector3 startPos, GameObject player)
    {
        print("Moved projectile to other player");
        float t = 0f;
        while (t < 1f)
        {
            Vector3 endPos = player.transform.position;
            Vector3 midpos = (startPos + endPos) * 0.5f + Vector3.up * curve;
            
            this.transform.position = GetCurvedPosition(
                startPos,
                midpos,
                endPos,
                startSpeed,
                midSpeed,
                endSpeed,
                ref t,
                Time.deltaTime
            );

            yield return null;
        }
        
        _rb.useGravity = true;
    }
    
    public void ShootWithoutTracking(Vector3 startpos, Vector3 endpos)
    {
        _currentCoroutine = StartCoroutine(MoveProjectileToEnd(startpos, endpos));
    }
    
    public IEnumerator MoveProjectileToEnd(Vector3 startPos, Vector3 endPos)
    {
        print("Moved projectile to end");
        Vector3 midpos = (startPos + endPos) * 0.5f + Vector3.up * curve;
        
        float t = 0f;
        while (t < 1f)
        {
            this.transform.position = GetCurvedPosition(
                startPos,
                midpos,
                endPos,
                startSpeed,
                midSpeed,
                endSpeed,
                ref t,
                Time.deltaTime
            );

            yield return null;
        }
        
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
        float speed =
            Mathf.Lerp(
                Mathf.Lerp(startSpeed, middleSpeed, t),
                Mathf.Lerp(middleSpeed, endSpeed, t),
                t
            );

        t += speed * deltaTime;
        t = Mathf.Clamp01(t);

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
