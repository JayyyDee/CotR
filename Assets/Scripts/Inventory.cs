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
    private GameObject firePoint;
    private float attackSpeed =1f;
   

    public override void OnNetworkSpawn()
    {
        GetComponentInChildren<Aiming>().SetPlayer(gameObject);
        firePoint = transform.GetChild(2).gameObject.transform.GetChild(0).gameObject;
        
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
            
        if (collision.CompareTag("Ring"))
        {
            collision.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            collision.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            collision.gameObject.GetComponent<CircleCollider2D>().enabled = false;

           
            if (inventory.Count <= 0)
            {
                equipedRing = collision.gameObject.GetComponent<Ring>();
                collision.gameObject.GetComponent<Ring>().SetEquiped(true);
                gameObject.GetComponent<Animator>().SetInteger("Form", equipedRing.GetComponent<Ring>().GetForm());
            }

            inventory.Add(collision.gameObject.GetComponent<Ring>());
            collision.gameObject.GetComponent<Ring>().SetFirePoint(firePoint);
            collision.gameObject.GetComponent<Ring>().SetPlayer(gameObject);
            if (gameObject.GetComponent<NetworkObject>().IsOwner)
            {
                collision.gameObject.GetComponent<Ring>().Passive();
            }
            collision.gameObject.GetComponent<Ring>().SetAttackSpeed(attackSpeed);
            
            if(collision.gameObject.name == "Patience")
            {
                Debug.Log("patience");
                collision.gameObject.GetComponent<RingPatience>().SetCamera(gameObject.transform.Find("PlayerCamera").gameObject.GetComponent<Camera>());
            }
            

            


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
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ChangeEquiped(4);
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
            
            GetComponent<Animator>().SetInteger("Form",equipedRing.GetComponent<Ring>().GetForm());
        }
        
    }

    //Set the attack speed of all rings in the inventory
    public void SetASpeed(float speed)
    {
        attackSpeed = speed;
    }

    public void Death()
    {
        foreach (Ring ring in inventory)
        {
            ring.Drop();
            ring.transform.GetChild(1).gameObject.SetActive(true);
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void SetPlayerServerRpc()
    {
        SetPlayerClientRpc();
    }
    [ClientRpc]
    public void SetPlayerClientRpc()
    {
        //a.GetComponent<Ring>().SetPlayer();
    }

}
