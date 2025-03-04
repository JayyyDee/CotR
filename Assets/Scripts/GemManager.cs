using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemManager : MonoBehaviour
{
    private int gemCounter = 0;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void GemCollect (Collider2D collision)
    {
        if (collision.CompareTag("Gemme"))
        {
            collision.gameObject.SetActive(false);
            gemCounter = 1;
        }
    }
    
}
