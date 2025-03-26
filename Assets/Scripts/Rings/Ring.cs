using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Ring: NetworkBehaviour 
{
    // public GameObject bulletPrefab;
    // public Color bulletColor;
    // private GameObject firePoint;

    // public float fireForce = 1f;
    // public float cooldown = 1f;
    // private float timer;
    // private bool canFire = true;
    


    private void Update()
    {
        //Debug.Log(firePoint.transform.position);
        //Debug.Log(canFire);
        //Debug.Log(timer);

        // if (equiped)
        // {
        //     if (!canFire)
        //     {
        //         timer += (Time.deltaTime);
        //         if (timer > cooldown)
        //         {
        //             canFire = true;
        //             timer = 0;
        //         }


        //     }
        //     if (Input.GetMouseButton(0) && canFire)
        //     {
        //         //Shoot();

        //     }
        //}
    }

    public abstract void SetEquiped(Boolean boole);

    public abstract void Shoot();

    public abstract void Active();

    public abstract void Passive();

    public abstract void SetAttackSpeed(float speed);
  
    public abstract void SetFirePoint(GameObject point);

    public abstract void SetPlayer(GameObject player);

    public abstract Boolean GetCanFire();

}

