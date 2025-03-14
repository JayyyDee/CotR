using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingShooting : MonoBehaviour
{
    private Inventory inventory;
    private Rigidbody2D rb;
    [SerializeField] private GameObject playerCamera;
    private Vector2 mousePosition;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        inventory = this.gameObject.GetComponent<Inventory>();
    }

    private void Update()
    {
        //Test for aiming with mouse 
        //mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetKeyDown(KeyCode.Mouse0)){
            inventory.ShootEquiped();
        }

    }



    private void FixedUpdate()
    {
        ////Testing the aiming (again)
        //Vector2 aimDirection = mousePosition - rb.position;
        //float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        //rb.rotation = aimAngle;
    }



    private void OnMouseDown()
    {
        inventory.ShootEquiped();
    }
}

