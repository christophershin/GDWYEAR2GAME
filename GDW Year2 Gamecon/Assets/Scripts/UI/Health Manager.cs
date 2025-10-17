using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    public float healthAmount = 100f;
    
    private RectTransform _healthBarRect;
    private float _maxWidth;
    private float _maxHeight;

    void Start()
    {
        _healthBarRect = healthBar.GetComponent<RectTransform>();
        _maxWidth = _healthBarRect.sizeDelta.x;
        _maxHeight = _healthBarRect.sizeDelta.y;
    }
    void Update()
    {
        _healthBarRect.sizeDelta = Vector2.Lerp(
            _healthBarRect.sizeDelta,
            new Vector2(_maxWidth * (healthAmount/100f), _maxHeight),
            Time.deltaTime * 10f);
    }

    public void TakeDamage(float amount)
    {
        healthAmount -= amount;
        if (healthAmount <= 0)
        {
            healthAmount = 0;
        }
    }

    public void Heal(float amount)
    {
        healthAmount += amount;
        if (healthAmount >= 100)
        {
            healthAmount = 100;
        }
    }
}
