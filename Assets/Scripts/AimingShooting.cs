using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingShooting : MonoBehaviour
{
    private Inventory inventory;
    public GameObject aimSpot;
    [SerializeField] private Camera playerCamera;
    private Vector2 mousePosition;
    void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
        inventory = this.gameObject.GetComponent<Inventory>();
    }

    private void Update()
    {
        //Test for aiming with mouse 
        //playerCamera.ScreenToWorldPoint(Input.mousePosition);
        //mousePosition = playerCamera.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            inventory.ShootEquiped(transform);
        }

    }



    private void FixedUpdate()
    {
        //Debug.Log(mousePosition);
        //////Testing the aiming (again)
        //Vector2 aimDirection = mousePosition - aimSpot.GetComponent<Rigidbody2D>().position;
        //float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        //Debug.Log(aimAngle);
        //aimSpot.GetComponent<Rigidbody2D>().rotation = aimAngle;
    }



    private void OnMouseDown()
    {
        inventory.ShootEquiped(transform);
    }
}

