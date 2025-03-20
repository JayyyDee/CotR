using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ring : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Color bulletColor;
    private static GameObject firePoint;

    public float fireForce = 1f;
    public float cooldown = 1f;
    private float timer;
    private bool canFire = true;
    public bool equiped = false;


    private void Update()
    {
        //Debug.Log(firePoint.transform.position);
        Debug.Log(canFire);
        //Debug.Log(timer);

        if (equiped)
        {
            if (!canFire)
            {
                timer += (Time.deltaTime);
                if (timer > cooldown)
                {
                    canFire = true;
                    timer = 0;
                }


            }
            if (Input.GetMouseButton(0) && canFire)
            {
                //Shoot();

            }
        }
    }

    public void Shoot()
    {
        if (canFire)
        {
            this.canFire = false;
            Vector3 pos = firePoint.transform.position;
            Quaternion rot = firePoint.transform.rotation;

            bulletPrefab.GetComponent<SpriteRenderer>().color = bulletColor;
            GameObject bullet = Instantiate(bulletPrefab, pos, rot);
            bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.transform.right * fireForce, ForceMode2D.Impulse);
        }
    }


    public void SetFirePoint(GameObject point)
    {
        firePoint = point;
    }
    
    public void SetCanFire(Boolean boole)
    {
        canFire = boole;
    }
    public Boolean GetCanFire()
    {
        return canFire;
    }

    //Shoot a bullet. 
    //firePoint: From where the bullet will be shot
    //public void Shoot(GameObject firePoint)
    //{

    //if (canFire)
    //{

    //    Vector3 pos = firePoint.transform.position;
    //    Quaternion rot = firePoint.transform.rotation;

    //    bulletPrefab.GetComponent<SpriteRenderer>().color = bulletColor;
    //    GameObject bullet = Instantiate(bulletPrefab, pos, rot);
    //    bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.transform.right * fireForce, ForceMode2D.Impulse);

    //    canFire = false;
    //}

    //}
    //public void ResetCoolDown()

    //{
    //}
}

