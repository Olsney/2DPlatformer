using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrainBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    private float _maxValue;

    private void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }

    public void Activate(float maxValue)
    {
        gameObject.SetActive(true);

        _maxValue = maxValue;
        _slider.maxValue = maxValue;
    } 
        
    public void Disable() => 
        gameObject.SetActive(false);

    public void DisplayProgress(float currentProgress)
    {
        float changePerSecond = 1f;
        
        _slider.value = currentProgress;
        
        _slider.value = Mathf.MoveTowards(currentProgress, _maxValue, changePerSecond);

        // if (currentProgress == _maxValue)
        //     Disable();
    }
}