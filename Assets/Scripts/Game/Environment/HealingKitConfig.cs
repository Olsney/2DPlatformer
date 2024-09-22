using UnityEngine;

[CreateAssetMenu(fileName = "HealingKitConfig", menuName = "Configs/HealingKitConfig")]
public class HealingKitConfig : ScriptableObject
{
    [SerializeField] private int _healthToHeal = 10;

    public int HealthToHeal => _healthToHeal;
}