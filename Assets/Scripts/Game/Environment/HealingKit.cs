using UnityEngine;
using World.Characters.Interfaces;

namespace World.Environment
{
    public class HealingKit : MonoBehaviour, IHealer
    {
        [SerializeField] private HealingKitConfig _config;
        
        public void Heal(IHealeable unit) => 
            unit.TakeHeal(_config.HealthToHeal);
    }
}