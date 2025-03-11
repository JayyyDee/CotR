using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GemManager : MonoBehaviour
{
    [SerializeField] public Image gemIcon;
    private int gemCounter = 0;

    public void GemCollect (Collider2D collision)
    {
        if (collision.CompareTag("Gemme"))
        {
            collision.gameObject.SetActive(false);
            gemIcon.gameObject.SetActive(true);
            gemCounter = 1;
        }
    }
    
}
