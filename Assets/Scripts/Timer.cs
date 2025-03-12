using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    float currentTime;
    public float startingTime = 22f;

    [SerializeField] TMP_Text countdownText;
    [SerializeField] GameObject player;
    void Start()
    {
        countdownText.enabled = false;
        currentTime = startingTime;
    }
    public void ReactToTrigger(GameObject triggerObject)
    {
        countdownText.enabled = true;
        currentTime = -1 * Time.deltaTime;
        countdownText.text = currentTime.ToString("0");

        if (currentTime <= 0)
        {
            currentTime = 0;
            // Your Code Here
        }
    }

}
