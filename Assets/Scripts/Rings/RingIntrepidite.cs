using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
//using static UnityEditor.PlayerSettings;

public class RingIntrepidite : Ring
{
    public GameObject bulletPrefab;
    public Color bulletColor;
    private GameObject firePoint;
    private GameObject playerCharacter;

    public float fireForce = 1f;
    public float cooldown = 1.4f;
    private float timer;
    private bool canFire = true;
    private bool equiped = false;

    public float activeCooldown = 5f;
    private float activeTimer;
    private bool canActive = true;

    public float passiveSpeed = 300f;
    public float dashDistance = 1500f;

    /*
    1 is normal speed
    <1 is slower
    >1 is faster
    */
    private float attackSpeed =1f;



    // Update is called once per frame
    void Update()
    {
        //Debug.Log(timer + " Intrépidité");

        if (!canFire)
        {
            timer += (Time.deltaTime);
            if (timer > (cooldown/attackSpeed))
            {
                canFire = true;
                timer = 0;
            }
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
        if (equiped && Input.GetMouseButton(0) && canFire)
        {
            ShootServerRpc();
            canFire = false;
        }
        if (equiped && Input.GetKeyDown(KeyCode.LeftShift) && canActive)
        {
            Active();
        }
    }


    public override void Shoot()
    {

        Vector3 pos = firePoint.transform.position;
        Quaternion rot = firePoint.transform.rotation;
        bulletPrefab.GetComponent<SpriteRenderer>().color = bulletColor;
        StartCoroutine(TripleShot(pos, rot));
    }

    IEnumerator TripleShot(Vector3 pos, Quaternion rot)
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, pos, rot);
            bullet.GetComponent<NetworkObject>().Spawn();
            yield return new WaitForSeconds(0.05f);
        }
    }

    public override void Active()
    {
        canActive = false;
        Vector2 forceDirection = (playerCharacter.GetComponent<Rigidbody2D>().velocity).normalized;
        playerCharacter.GetComponent<Rigidbody2D>().AddForce(forceDirection*dashDistance);
    }

    public override void Passive()
    {
        playerCharacter.GetComponent<PlayerMovement>().speedBuff = passiveSpeed;
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
        if (IsOwner)
        {
            Shoot();
        }
        
        
    }
}
