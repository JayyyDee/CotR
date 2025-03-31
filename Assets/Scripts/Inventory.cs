using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Net.NetworkInformation;
using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;
using Unity.Netcode;

public class Inventory : NetworkBehaviour
{
    private List<Ring> inventory = new List<Ring>();
    private Ring equipedRing;
    public List<GameObject> UISlots = new List<GameObject>();
    public GameObject firePoint;
    private float attackSpeed =1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        
        if (collision.CompareTag("Ring"))
        {
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            collision.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            
            //if (!IsOwner) {
            //return;
            //}
            if (inventory.Count <= 0)
            {
                equipedRing = collision.gameObject.GetComponent<Ring>();
                collision.gameObject.GetComponent<Ring>().SetEquiped(true);
            }
            
            
            
           
            inventory.Add(collision.gameObject.GetComponent<Ring>());
            collision.gameObject.GetComponent<Ring>().SetFirePoint(firePoint);
            collision.gameObject.GetComponent<Ring>().SetPlayer(this.gameObject);
                
            collision.gameObject.GetComponent<Ring>().Passive();
            collision.gameObject.GetComponent<Ring>().SetAttackSpeed(attackSpeed);
            
            
            

            


            string fullInv = "";
            int i = 0;

            if (!IsOwner)
            {
                return;
            }
                foreach (Ring ring in inventory)
            {
                ring.SetAttackSpeed(attackSpeed);
                GameObject.Find("Slot" + i).GetComponent<Image>().sprite = inventory[i].GetComponent<SpriteRenderer>().sprite;
                fullInv += (ring.name + " ");
                i++;
            }
            Debug.Log(fullInv);

        }
    }

    private void Update()
    {
        if (!IsOwner) {
            return;
        }

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

    }

    public Ring GetEquipped()
    {
        return equipedRing;
    }

    public void ChangeEquiped(int i)
    {
        if (inventory[i] != null)
        {
            equipedRing.SetEquiped(false);
            equipedRing = inventory[i];
            inventory[i].SetEquiped(true);
        }
        
    }

    //Set the attack speed of all rings in the inventory
    public void SetASpeed(float speed)
    {
        attackSpeed = speed;
    }
}
