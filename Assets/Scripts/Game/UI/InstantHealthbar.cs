using Game.UI;
using UnityEngine;
using UnityEngine.UI;

public class InstantHealthbar : HealthViewBase
{
    [SerializeField] private Image _image;
    [SerializeField] private Transform _transform;
    
    private int _maxHealth;

    public void LateUpdate()
    {
        _transform.rotation = Quaternion.identity;
    }
    
    public override void SetHealthInfo(int health, int maxHealth)
    {
        _maxHealth = maxHealth;
        _image.fillAmount = health / maxHealth;
    }

    public override void SetCurrentHealth(int health)
    {
        _image.fillAmount = (float)health / _maxHealth;
    }
}