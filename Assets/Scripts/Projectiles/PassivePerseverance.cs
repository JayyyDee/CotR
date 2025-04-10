using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassivePerseverance : MonoBehaviour
{
    public GameObject player;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy") /*&& collider.gameObject != player*/)
        {
            Debug.Log(collider);
            collider.gameObject.transform.Find("Target").gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy") /*&& collider.gameObject != player*/)
        {
            collider.gameObject.transform.Find("Target").gameObject.SetActive(false);
        }
    }
}
