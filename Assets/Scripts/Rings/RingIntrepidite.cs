using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.Netcode;
using UnityEngine;

public class RingIntrepidite : Ring
{
    public GameObject bulletPrefab;
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

    void Update()
    {
        //Debug.Log(timer + " Intr�pidit�");

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
        if (equiped && Input.GetMouseButton(0) && canFire && IsOwner)
        {
            ShootServerRpc();
            canFire = false;
            AkUnitySoundEngine.PostEvent("Play_Anneaux_Intr_pidit__Attack_Throw_TYPE1__itemnumber", this.gameObject);
        }
        if (equiped && Input.GetKeyDown(KeyCode.LeftShift) && canActive && IsOwner)
        {
            Active();
            canActive = false;
        }
    }


    public override void Shoot()
    {
        Vector3 pos = firePoint.transform.position;
        Quaternion rot = firePoint.transform.rotation;
        StartCoroutine(TripleShot(pos, rot));
    }

    IEnumerator TripleShot(Vector3 pos, Quaternion rot)
    {
        for (int i = 0; i < 3; i++)
        {
            
            GameObject bullet = Instantiate(bulletPrefab, pos, rot);
            bullet.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
            yield return new WaitForSeconds(0.05f);
        }
    }

    public override void Active()
    {
        canActive = false;
        AkUnitySoundEngine.PostEvent("Play_FULL_Anneaux_Intr_pidit__Actif_Full__itemnumber", this.gameObject);
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
        Shoot();
    }

    [ClientRpc]
    public void ShootClientRpc() {
        
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
        return 3;
    }

    public override float GetActiveCooldown()
    {
        return activeCooldown - activeTimer;
    }

    public override float GetActiveMaxCooldown()
    {
        return activeCooldown;
    }
}
