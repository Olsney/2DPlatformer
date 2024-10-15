using System;
using UnityEngine;
using UnityEngine.UI;
using World.Characters.Players.Systems;

public class DrainBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private DrainHealthAbility _drainHealthAbility;
    
    private void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }
    
    public void Init()
    {
        _drainHealthAbility.SkillActivated += Activate;
        _drainHealthAbility.ProgressChanged += DisplayProgress;
        _drainHealthAbility.CooldownEnded += Disable;
    }
    
    public void OnDispose()
    {
        _drainHealthAbility.SkillActivated -= Activate;
        _drainHealthAbility.ProgressChanged -= DisplayProgress;
        _drainHealthAbility.CooldownEnded -= Disable;
    }
    
    public void Disable()
    {
        gameObject.SetActive(false);
    }

    private void Activate(float maxValue)
    {
        gameObject.SetActive(true);
        _slider.maxValue = maxValue;
    }

    private void DisplayProgress(float currentProgress) => 
        _slider.value = currentProgress;
}