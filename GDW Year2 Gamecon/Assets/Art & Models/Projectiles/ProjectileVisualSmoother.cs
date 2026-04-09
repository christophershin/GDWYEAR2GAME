using UnityEngine;

public class ProjectileVisualSmoother : MonoBehaviour
{
    [SerializeField] private float smoothSpeed = 20f;
    [SerializeField] private float predictionSpeed = 25f;
    
    private Transform _trueParent;

    void Start()
    {
        _trueParent = transform.parent;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _trueParent.position, Time.deltaTime * smoothSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, _trueParent.rotation, Time.deltaTime * smoothSpeed);
    }
}