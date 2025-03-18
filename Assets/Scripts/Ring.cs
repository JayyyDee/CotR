using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ring : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Color bulletColor;
    
    public float fireForce = 5f;

    //Shoot a bullet. 
    //firePoint: From where the bullet will be shot
    public void Shoot(GameObject firePoint)
    {
        Vector3 pos = firePoint.transform.position;
        Quaternion rot = firePoint.transform.rotation;

        bulletPrefab.GetComponent<SpriteRenderer>().color = bulletColor;
        GameObject bullet = Instantiate(bulletPrefab, pos, rot);
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.transform.right * fireForce, ForceMode2D.Impulse);
    }
}

