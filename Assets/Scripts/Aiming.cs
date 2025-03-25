using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Aiming : NetworkBehaviour
{
    private Vector3 mousePos;
    public Camera cam;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsOwner) {
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 aimDirection = mousePos - transform.position;
            float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, aimAngle);
        }
    }
}
