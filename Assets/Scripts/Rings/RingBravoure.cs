using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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

    public float activeCooldown = 5f;
    private float activeTimer;
    private bool canActive = true;

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
            ShootServerRpc();
        }

        if (!canActive)
        {
            activeTimer += (Time.deltaTime);
            if (activeTimer > activeCooldown)
            {
                canActive = true;
                activeTimer = 0;
            }
        }
        
        if (equiped && Input.GetKeyDown(KeyCode.LeftShift) && canActive)
        {
            Active();
        }
    }


    public override void Shoot()
    {
        Debug.Log("shoot");
        if (!IsOwner) {
            return;
        }
        canFire = false;

        playerCharacter.GetComponent<ProjectileBravoure>().Shoot();
        
    }

    public override void Active()
    {
        StartCoroutine(ActiveAbility());
        canActive = false;
        
    }

    IEnumerator ActiveAbility()
    {
        Vector3 pos = firePoint.transform.position + (firePoint.transform.right*3.5f);
        Quaternion rot = firePoint.transform.rotation;
        yield return new WaitForSeconds(0.75f);
        bulletPrefab.GetComponent<SpriteRenderer>().color = bulletColor;
        GameObject bullet = Instantiate(bulletPrefab, pos, rot);


        

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

    [ServerRpc(RequireOwnership = false)]
    public void ShootServerRpc()
    {
        ShootClientRpc();
    }

    [ClientRpc]
    private void ShootClientRpc()
    {
            Shoot();

        
    }
    

}
