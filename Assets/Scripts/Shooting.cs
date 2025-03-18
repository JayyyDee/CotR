using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private Inventory inventory;
    public GameObject aimSpot;
    [SerializeField] private Camera playerCamera;
    private Vector2 mousePosition;
    void Start()
    {
        inventory = this.gameObject.GetComponent<Inventory>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            inventory.ShootEquiped(aimSpot);
        }

    }

    private void OnMouseDown()
    {
        inventory.ShootEquiped(aimSpot);
    }
}

