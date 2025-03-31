using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upanddown : MonoBehaviour
{
    public float amplitude = 0.5f;
    public float velocity = 1.5f;

    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float newY = startPos.y + amplitude * Mathf.Sin(Time.time * velocity);

        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
