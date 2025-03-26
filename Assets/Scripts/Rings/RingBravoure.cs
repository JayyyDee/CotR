using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingBravoure : Ring
{
    public GameObject bulletPrefab;
    public Color bulletColor;
    private GameObject firePoint;
    private GameObject playerCharacter;

    public float fireForce = 1f;
    public float cooldown = 1f;
    private float timer;
    private bool canFire = true;
    private bool equiped = false;

    /*
    1 is normal speed
    <1 is slower
    >1 is faster
    */
    private float attackSpeed = 1f;


    // Update is called once per frame
    void Update()
    {
        //Debug.Log(timer + " Bravoure");

        if (!canFire)
        {
            timer += (Time.deltaTime);
            if (timer > (cooldown / attackSpeed))
            {
                canFire = true;
                timer = 0;
            }
        }
        if (equiped && Input.GetMouseButton(0) && canFire)
        {
            Shoot();
        }
    }


    public override void Shoot()
    {
        Debug.Log("shoot");
        canFire = false;
        Vector3 pos = firePoint.transform.position;
        Quaternion rot = firePoint.transform.rotation;

        bulletPrefab.GetComponent<SpriteRenderer>().color = bulletColor;
        GameObject bullet = Instantiate(bulletPrefab, pos, rot);
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.transform.right * fireForce, ForceMode2D.Impulse);
    }

    public override void Active()
    {
        
    }

    public override void Passive()
    {
        
    }
    public override void SetEquiped(bool boole)
    {
        equiped = boole;
    }
    public override bool GetCanFire()
    {
        return canFire;
    }

    public override void SetFirePoint(GameObject point)
    {
        firePoint = point;
    }

    public override void SetPlayer(GameObject player)
    {
        playerCharacter = player;
    }

    public override void SetAttackSpeed(float speed)
    {
        attackSpeed = speed;
    }
}
