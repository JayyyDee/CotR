using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : NetworkBehaviour
{
    [SerializeField] public Slider slider;
    [SerializeField] public Gradient gradient;
    [SerializeField] public Image fill;

    public void SetMaxHealth(int health)
    {
            slider.maxValue = health;
            slider.value = health;

            //The gradient will start at the beginning, so the health will be green.
            fill.color = gradient.Evaluate(1f);
    }
   public void SetHealth(int health)
   {
            slider.value = health;

            //Update the slider to represent the color for the amount of health
            fill.color = gradient.Evaluate(slider.normalizedValue);
    }

}
