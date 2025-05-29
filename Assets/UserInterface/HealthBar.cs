using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void updateHealthbar(float current, float max)
    {
        slider.value = current / max;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
