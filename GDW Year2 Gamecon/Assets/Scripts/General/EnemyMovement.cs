using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Patrol Points (local or world)")]
    public Vector3 pointA = new Vector3(-3, 0, 0);
    public Vector3 pointB = new Vector3(3, 0, 0);

    [Header("Movement")]
    public float speed = 2f;
    public bool useLocalPositions = true;

    private Vector3 _a, _b;
    private Vector3 _target;

    void Start()
    {
        _a = useLocalPositions ? transform.TransformPoint(pointA) : pointA;
        _b = useLocalPositions ? transform.TransformPoint(pointB) : pointB;
        _target = _b;
    }

    void Update()
    {
        // Move toward current target
        transform.position = Vector3.MoveTowards(transform.position, _target, speed * Time.deltaTime);

        // If reached target, flip to other endpoint
        if (Vector3.Distance(transform.position, _target) < 0.01f)
        {
            _target = (_target == _b) ? _a : _b;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the patrol path in the editor
        Gizmos.color = Color.yellow;
        Vector3 a = useLocalPositions ? transform.TransformPoint(pointA) : pointA;
        Vector3 b = useLocalPositions ? transform.TransformPoint(pointB) : pointB;
        Gizmos.DrawSphere(a, 0.15f);
        Gizmos.DrawSphere(b, 0.15f);
        Gizmos.DrawLine(a, b);
    }
}
