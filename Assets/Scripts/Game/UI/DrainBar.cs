using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrainBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    private int _maxValue;

    private void LateUpdate()
    {
        transform.localRotation = Quaternion.identity;
    }

    public void Activate(int maxValue)
    {
        gameObject.SetActive(true);

        _maxValue = maxValue;
        _slider.maxValue = maxValue;
    } 
        
    public void Disable() => 
        gameObject.SetActive(false);

    public void DisplayProgress(int currentProgress)
    {
        int changePerSecond = 1;
        
        _slider.value = currentProgress;
        
        _slider.value = Mathf.MoveTowards(currentProgress, _maxValue, changePerSecond);

        // if (currentProgress == _maxValue)
        //     Disable();
    }
}
