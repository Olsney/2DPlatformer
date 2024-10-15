using UnityEngine;
using UnityEngine.UI;

public class DrainBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    
    private void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }

    public void Activate(float maxValue)
    {
        gameObject.SetActive(true);
        _slider.maxValue = maxValue;
    } 
        
    public void Disable() => 
        gameObject.SetActive(false);

    public void DisplayProgress(float currentProgress) => 
        _slider.value = currentProgress;
}