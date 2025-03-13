using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GemManager : MonoBehaviour
{
    [SerializeField] public Image gemIcon;
    private int gemCounter = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Move in GemManager, when colliding with the gem, set inactive and play sound.
        if (collision.CompareTag("Gemme"))
        {
            CollectGemServerRpc(collision);
        }
    }

    [ServerRpc]
    private void CollectGemServerRpc(Collider2D gem)
    {
        CollectGemClientRpc(gem);
    }

    [ClientRpc]
    void CollectGemClientRpc(Collider2D gem)
    {
        gem.gameObject.SetActive(false);
        gemIcon.gameObject.SetActive(true);
        gemCounter = 1;
        AkUnitySoundEngine.PostEvent("Event_Jadeide_Slow__Pickup", this.gameObject); // The Event to play sounds of collecting the Jadeide
    }

}
