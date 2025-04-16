using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class RingBravoure : Ring
{
    public GameObject bulletPrefab;
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


    public float passiveCooldown = 7f;
    private float passiveTimer;
    private bool startPassive = false;

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
        if (equiped && Input.GetMouseButton(0) && canFire && IsOwner)
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
        
        if (equiped && Input.GetKeyDown(KeyCode.LeftShift) && canActive && IsOwner)
        {
            ActiveServerRpc();
        }

        if (startPassive)
        {
            passiveTimer += (Time.deltaTime);
            if (passiveTimer > passiveCooldown)
            {
                playerCharacter.GetComponent<HealthManager>().HealingServerRpc(150);
                passiveTimer = 0;
            }
            
        }

    }


    public override void Shoot()
    {
        canFire = false;

        playerCharacter.GetComponent<ProjectileBravoure>().Shoot();
        AkUnitySoundEngine.PostEvent("Play_Anneaux_Bravoure_Anneaux_Bravoure_Attack_Full__itemnumber", this.gameObject);

    }

    public override void Active()
    {
        Vector3 pos = firePoint.transform.position + (firePoint.transform.right * 4f);
        Quaternion rot = firePoint.transform.rotation;
        GameObject bullet = Instantiate(bulletPrefab, pos, rot);
        bullet.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
        canActive = false;
        
    }

 

    public override void Passive()
    {
        startPassive = true;
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
    public void ActiveServerRpc() {
        Active();
    }

    [ServerRpc(RequireOwnership = false)]
    public void ShootServerRpc()
    {
        ShootClientRpc();
        
    }

    [ClientRpc]
    public void ShootClientRpc()
    {
        
        Shoot();

    }

    [ServerRpc(RequireOwnership = false)]
    public void DropServerRpc(Vector2 pos)
    {
        DropClientRpc(pos);
    }

    [ClientRpc]
    public void DropClientRpc(Vector2 pos)
    {
        gameObject.transform.position = pos;

        gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
        gameObject.GetComponent<Ring>().SetEquiped(false);

    }


    public override void Drop()
    {
        startPassive = false;
        Vector2 pos = new Vector2(playerCharacter.transform.position.x + Random.Range(0, 2f), playerCharacter.transform.position.y + Random.Range(0, 2f));
        AkUnitySoundEngine.PostEvent("Play_SFX_DropLoot__itemnumber", this.gameObject);
        DropServerRpc(pos);
    }

    public override int GetForm()
    {
        return 2;
    }
}
