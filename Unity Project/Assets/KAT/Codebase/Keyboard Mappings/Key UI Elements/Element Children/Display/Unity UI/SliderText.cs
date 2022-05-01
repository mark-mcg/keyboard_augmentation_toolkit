using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.UI;

public class SliderText : MonoBehaviour
{

    TMPro.TextMeshPro textComponent;

    void Start()
    {
        textComponent = GetComponent<TMPro.TextMeshPro>();
    }

    public void SetSliderValue(float sliderValue)
    {
        if (textComponent != null)
            textComponent.text = Mathf.Round(sliderValue * 100).ToString();
    }
}