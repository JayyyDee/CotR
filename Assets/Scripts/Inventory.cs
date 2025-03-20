using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Net.NetworkInformation;
using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;

public class Inventory : MonoBehaviour
{
    private List<Ring> inventory = new List<Ring>();
    private Ring equipedRing;
    public List<GameObject> UISlots = new List<GameObject>();
    public GameObject firePoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Ring"))
        {
            if (inventory.Count <= 0)
            {
                equipedRing = collision.gameObject.GetComponent<Ring>();
            }
            collision.gameObject.GetComponent<Ring>().equiped = true;
            collision.gameObject.GetComponent<Ring>().SetFirePoint(firePoint);
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeEquiped(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeEquiped(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeEquiped(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChangeEquiped(3);
        }

        if (Input.GetMouseButton(0) && equipedRing.GetCanFire())
        {
            ShootEquiped();
        }
    }

    public Ring GetEquipped()
    {
        return inventory[0];
    }

    public void ChangeEquiped(int i)
    {
        equipedRing = inventory[i];
    }

    public void ShootEquiped()
    {
        equipedRing.Shoot();
    }
}
