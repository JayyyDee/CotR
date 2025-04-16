using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class RingPassion : Ring
{
    public GameObject bulletPrefab;
    private GameObject firePoint;
    private GameObject playerCharacter;
    

    public float bonusASpeed;

    public float fireForce = 1f;
    public float cooldown = 1f;
    private float timer;
    private bool canFire = true;
    private bool equiped = false;

    public float activeCooldown = 5f;
    private float activeTimer;
    private bool canActive = true;
    private bool activeBoost = false;

    public int damage = 100;

    /*
    1 is normal speed
    <1 is slower
    >1 is faster
    */
    private float attackSpeed = 1f;



    // Update is called once per frame
    void Update()
    {
        
        if (!canFire)
        {
            timer += (Time.deltaTime);
            if (timer > (cooldown / attackSpeed))
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
       
            if (equiped && Input.GetMouseButton(0) && canFire && IsOwner)
        {
            ShootServerRpc();
            canFire = false;
        }

        if (equiped && Input.GetKeyDown(KeyCode.LeftShift) && canActive && IsOwner)
        {
            Active();
        }
    }


    public override void Shoot()
    {
        
        Vector3 pos = firePoint.transform.position;
        Quaternion rot = firePoint.transform.rotation;

        GameObject bullet = Instantiate(bulletPrefab, pos, rot);
        AkUnitySoundEngine.PostEvent("Play_Anneaux_Passion_Anneaux_Passion_Attack_Pt1__itemnumber", this.gameObject);
        bullet.GetComponent<ProjectilePassion>().fireForce = fireForce;
        
        if (activeBoost)
        {
            bullet.GetComponent<SpriteRenderer>().color = new Color(1.00f, 0.65f, 0.00f);
            bullet.GetComponent<ProjectilePassion>().SetBoost(true);
            
            activeBoost = false;
        }
        bullet.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
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

    public override void Active()
    {
        activeBoost = true;
        canActive = false;
        AkUnitySoundEngine.PostEvent("Play_Anneaux_Passion_Anneaux_Passion_Actif_Full__itemnumber", this.gameObject);
    }

    public override void Passive()
    {
        playerCharacter.GetComponent<Inventory>().SetASpeed(bonusASpeed);
    }

    public override void SetAttackSpeed(float speed)
    {
        attackSpeed = speed;
    }

    [ServerRpc(RequireOwnership = false)]
    public void ShootServerRpc()
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
        Vector2 pos = new Vector2(playerCharacter.transform.position.x + Random.Range(0, 2f), playerCharacter.transform.position.y + Random.Range(0, 2f));
        AkUnitySoundEngine.PostEvent("Play_SFX_DropLoot__itemnumber", this.gameObject);
        DropServerRpc(pos);
    }

    public override int GetForm()
    {
        return 4;
    }
}
