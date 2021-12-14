using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public Transform cam;
    
    void Start()
    {
        slider.normalizedValue = 1.0f;
        if(cam == null)
        {
            cam = Camera.main.transform;
        }
    }

    void LateUpdate()
    {
        transform.LookAt(cam.position);
    }

    public void UpdateHealthBar(float normalizedHealth)
    {
        slider.normalizedValue = normalizedHealth;
    }

}
