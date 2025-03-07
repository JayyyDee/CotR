using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    public GameObject bulletPrefab;
    
    public float fireForce = 15f;

    //Shoot a bullet. 
    //firePoint: From where the bullet will be shot
    public void Shoot(Transform firePoint)
    {
        GameObject bullet = Instantiate(bulletPrefab, new Vector2(firePoint.position.x, firePoint.position.y+1),firePoint.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
    }
}

