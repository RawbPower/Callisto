using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterBar : MonoBehaviour
{
    private Slider slider;
    public Image border;
    public Sprite brokenBorder;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void SetWaterRatio(float ratio)
    {
        slider.value = 1.0f - ratio;
    }

    public void BreakWaterBar()
    {
        border.sprite = brokenBorder;
    }
}
