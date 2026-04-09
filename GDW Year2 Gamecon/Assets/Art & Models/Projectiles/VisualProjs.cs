using System.Collections;
using UnityEngine;

public class VisualProjs : MonoBehaviour
{
    [SerializeField] private float smoothSpeed = 20f;
    [SerializeField] private float predictionSpeed = 25f;
    
    private Transform _trueParent;

    public string projectileID; 
    public bool isPredicting = false;
    
    private Coroutine _predictionCoroutine;

    void Start()
    {
        _trueParent = transform.parent;
    }

    public void StartPrediction(string id, Vector3 endPos)
    {
        projectileID = id;
        isPredicting = true;
        
        _predictionCoroutine = StartCoroutine(MoveProjectileToEnd(transform.position, endPos));
        
        transform.SetParent(null);
    }

    public void BindToNetwork(Transform target)
    {
        StopCoroutine(_predictionCoroutine);
        _trueParent = target;
        isPredicting = false;
        transform.SetParent(target); 
    }

    void Update()
    {
        if (!isPredicting && _trueParent != null)
        {
            transform.position = Vector3.Lerp(transform.position, _trueParent.position, Time.deltaTime * smoothSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, _trueParent.rotation, Time.deltaTime * smoothSpeed);
        }
    }
    
    public IEnumerator MoveProjectileToEnd(Vector3 startPos, Vector3 endPos)
    {
        Vector3 midpos = (startPos + endPos) * 0.5f + Vector3.up * 2;
        
        float t = 0f;
        while (t < 1f)
        {
            this.transform.position = GetCurvedPosition(
                startPos,
                midpos,
                endPos,
                .4f,
                .8f,
                0.2f,
                ref t,
                Time.deltaTime
            );

            yield return null;
        }
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
}
