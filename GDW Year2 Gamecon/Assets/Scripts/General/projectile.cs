using UnityEngine;

public class projectile : MonoBehaviour
{


    [SerializeField] private float projectileTimerMax = 10;
    private float projectileTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        projectileTimer = projectileTimerMax;
    }

    // Update is called once per frame
    void Update()
    {
        projectileTimer -= Time.deltaTime;

        if (projectileTimer <= 0)
            Destroy(this.gameObject);



    }
}
