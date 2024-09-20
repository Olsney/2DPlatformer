using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class HealingKit : MonoBehaviour, IHealer
{
    private int _healthToHeal;

    private void Awake()
    {
        _healthToHeal = 10;
    }

    public void Heal(IHealeable player)
    {
        player.TakeHeal(_healthToHeal);
    }
}
