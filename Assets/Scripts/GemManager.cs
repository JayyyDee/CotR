using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GemManager : MonoBehaviour
{
    public GameManager GameManager;
    [SerializeField] public Image gemIcon;
    private int gemCounter = 0;
    private GameObject gem;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Move in GemManager, when colliding with the gem, set inactive and play sound.
        if (collision.CompareTag("Gemme"))
        {
            gem = collision.GameObject();
            CollectGemServerRpc();
        }
    }

    [ServerRpc]
    private void CollectGemServerRpc()
    {
        CollectGemClientRpc();
    }

    [ClientRpc]
    void CollectGemClientRpc()
    {
        gem.gameObject.SetActive(false);
        gemIcon.gameObject.SetActive(true);
        AkUnitySoundEngine.PostEvent("Event_Jadeide_Slow__Pickup", this.gameObject); // The Event to play sounds of collecting the Jadeide
    }

    public void Death()
    {
        if (gem)
        {
            AkUnitySoundEngine.PostEvent("Play_SFX_SelectRing_Type4__itemnumber", this.gameObject);
            DropGemServerRpc();
        }
        
    }

    [ServerRpc]
    private void DropGemServerRpc()
    {
        DropGemClientRpc();

    }

    [ServerRpc]
    private void DropGemClientRpc()
    {
        gem.transform.position = transform.position;
        gem.gameObject.SetActive(true);

    }
}
