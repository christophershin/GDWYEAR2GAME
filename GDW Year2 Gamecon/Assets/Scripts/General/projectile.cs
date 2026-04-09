using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class projectile : NetworkBehaviour
{
    public NetworkVariable<ulong> projOwnId = new NetworkVariable<ulong>();
    
    [SerializeField] private float startSpeed, midSpeed, endSpeed, curve;

    public float projectileTimerMax = 10;
    [HideInInspector]
    public float projectileTimer;

    public float damage;

    private Rigidbody _rb;
    Coroutine _currentCoroutine;
    
    [SerializeField] private Material _material;

    public string projNamTagg;
    
    [SerializeField] private GameObject chil;
    
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    
    public override void OnNetworkSpawn()
    {
        print("proj own: " + projOwnId);
        print("local: " + NetworkManager.Singleton.LocalClientId.ToString());
        
        if (NetworkManager.Singleton.LocalClientId == projOwnId.Value && !IsServer)
        {
            chil.SetActive(false);
            //var smoothers = FindObjectsByType<ProjectileVisualSmoother>(FindObjectsSortMode.None);
            GameObject[] pro = GameObject.FindGameObjectsWithTag(projNamTagg);

            for (int i = 0; i < pro.Length; i++)
            {
                if (pro[i].GetComponent<VisualProjs>().isPredicting)
                {
                    pro[i].GetComponent<VisualProjs>().BindToNetwork(this.transform);
                    break;
                }
            }
            
            

            // foreach (var s in smoothers)
            // {
            //     if (s.isPredicting && s.projectileID == projNamTagg)
            //     {
            //         s.BindToNetwork(this.transform);
            //         break;
            //     }
            // }
        }
        // else
        // {
        //     // if (transform.childCount > 0)
        //     // {
        //     //     var smoother = chil.GetComponent<ProjectileVisualSmoother>();
        //     //     if (smoother != null)
        //     //     {
        //     //         smoother.BindToNetwork(this.transform);
        //     //     }
        //     // }
        // }
    }
    
    // public override void OnNetworkSpawn()
    // {
    //     if (IsOwner && !IsServer)
    //     {
    //         var smoothers = FindObjectsByType<ProjectileVisualSmoother>(FindObjectsSortMode.None);
    //
    //         foreach (var s in smoothers)
    //         {
    //             // We check the ID string and the prediction state
    //             if (s.isPredicting && s.projectileID == projNamTagg)
    //             {
    //                 s.BindToNetwork(this.transform);
    //             
    //                 // Hide the "factory" visual on the networked object
    //                 if (transform.childCount > 0)
    //                     transform.GetChild(0).gameObject.SetActive(false); 
    //             
    //                 break;
    //             }
    //         }
    //     }
    //     else
    //     {
    //         // if (transform.childCount > 0)
    //         // {
    //         //     transform.GetChild(0).GetComponent<ProjectileVisualSmoother>().BindToNetwork(this.transform);
    //         // }
    //     }
    //     
    //     // if (!IsServer)
    //     // {
    //     //     enabled = false;
    //     //     return;
    //     // }
    //     
    //     //projectileTimer = projectileTimerMax;
    // }
    
    public void StraightParry(Vector3 newPos)
    {
        Debug.Log("Parried");
        damage += 5f;
        startSpeed *= 1.2f;
        midSpeed *= 1.2f;
        endSpeed *= 1.2f;
        
        if (_currentCoroutine != null)
            StopCoroutine(_currentCoroutine);

        Vector3 start = transform.position;

        _currentCoroutine = StartCoroutine(
            MoveProjectileToEnd(start, newPos)
        );
    }
    
    public void TrackedParry(GameObject player)
    {
        // NetworkObject playerObject = NetworkManager.Singleton.ConnectedClients[targetID].PlayerObject;
        //
        // GameObject player = playerObject.gameObject;
            
        //Debug.Log("Parried");
        damage *= 1.1f;
        damage = Mathf.Ceil(damage);
        startSpeed *= 1.1f;
        midSpeed *= 1.1f;
        endSpeed *= 1.1f;
        
        if (_currentCoroutine != null)
            StopCoroutine(_currentCoroutine);

        Vector3 start = transform.position;

        _currentCoroutine = StartCoroutine(
            MoveProjectileToPlayer(start, player)
        );
    }
    
    

    public void ShootWithTracking(GameObject player, Vector3 startpos)
    {
        _currentCoroutine = StartCoroutine(MoveProjectileToPlayer(startpos, player));
    }
    
    public IEnumerator MoveProjectileToPlayer(Vector3 startPos, GameObject player)
    {
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
        
        yield return new WaitForSeconds(.4f);
        
        
        GetComponent<NetworkObject>().Despawn(true);
        //Destroy(this.gameObject);
    }
    
    public void ShootWithoutTracking(Vector3 startpos, Vector3 endpos)
    {
        _currentCoroutine = StartCoroutine(MoveProjectileToEnd(startpos, endpos));
    }
    
    public IEnumerator MoveProjectileToEnd(Vector3 startPos, Vector3 endPos)
    {
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
        damage = 0;
        
        GetComponent<NetworkObject>().Despawn(true);
        //Destroy(this.gameObject);
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
