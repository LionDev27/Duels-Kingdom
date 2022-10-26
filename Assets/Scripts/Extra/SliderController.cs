using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SliderController : MonoBehaviour, IMoveHandler
{
    [SerializeField] float stepOnValueChanged = 0.05f;
    Slider slider;
    float previousSliderValue = 0f;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Start()
    {
        previousSliderValue = slider.value;
    }

    public void OnMove(AxisEventData eventData)
    {
        if (eventData.moveDir == MoveDirection.Left)
        {
            slider.value = previousSliderValue - stepOnValueChanged;
        }
        else if (eventData.moveDir == MoveDirection.Right)
        {
            slider.value = previousSliderValue + stepOnValueChanged;
        }

        previousSliderValue = slider.value;
    }
}
