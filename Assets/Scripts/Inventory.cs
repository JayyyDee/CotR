using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<Ring> inventory = new List<Ring>();
    private Ring equipedRing;

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
            foreach (Ring ring in inventory)
            {
                fullInv += (ring.name + " ");
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
