using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class UISizeHelper : MonoBehaviour
{
    public virtual void SetWidthHeight(Vector2 dims)
    {

    }
}

public class SliderWidthHelper : UISizeHelper
{
    public Slider slider;

    [Button]
    public void SetWidthTest()
    {
        SetSliderWidth(3.0f);
    }

    public override void SetWidthHeight(Vector2 dims)
    {
        base.SetWidthHeight(dims);
        SetSliderWidth(dims.x);
    }

    public void SetSliderWidth(float width)
    {
        if (slider == null)
            slider = GetComponent<Slider>();

        Debug.Log("SetSliderWidth called with " + width);
        width = width - 0.3f;

        RectTransform fillArea = slider.transform.Find("Fill Area").GetComponent<RectTransform>();
        RectTransform fill = fillArea.transform.Find("Fill").GetComponent<RectTransform>();
        RectTransform handleSlideArea = slider.transform.Find("Handle Slide Area").GetComponent<RectTransform>();
        RectTransform handle = handleSlideArea.transform.Find("Handle").GetComponent<RectTransform>();

        //handle.sizeDelta = new Vector3(1, 0);
        //slider.transform.localScale = new Vector3(width / 10, 0.5f, 1);
        //handle.transform.localScale = 0.5f * new Vector3(1 / slider.transform.localScale.x, 1 / slider.transform.localScale.y, 1 / slider.transform.localScale.z);
        //handle.transform.localScale

        //Vector3 diff = slider.transform.position - fill.transform.position;
        Canvas canvas = slider.GetComponentInParent<Canvas>();
        // canvas.transform.localPosition += diff;

        /*
         * Changes to fit Slider into area
         * 
         * 
         */

    }

}
