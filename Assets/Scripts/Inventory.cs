using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Net.NetworkInformation;
using UnityEngine.UI;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<Ring> inventory = new List<Ring>();
    private Ring equipedRing;
    public List<GameObject> UISlots = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Ring"))
        {
            if (inventory.Count <= 0)
            {
                equipedRing = collision.gameObject.GetComponent<Ring>();
            }
            inventory.Add(collision.gameObject.GetComponent<Ring>());

            
            
            collision.gameObject.SetActive(false);
            string fullInv = "";
            int i = 0;
            foreach (Ring ring in inventory)
            {
                GameObject.Find("Slot" + i).GetComponent<Image>().sprite = inventory[i].GetComponent<SpriteRenderer>().sprite;
                fullInv += (ring.name + " ");
                i++;
            }
            Debug.Log(fullInv);

        }
    }

    public Ring GetEquipped()
    {
        return inventory[0];
    }

    public void ShootEquiped()
    {
        equipedRing.Shoot(transform);
    }
}
