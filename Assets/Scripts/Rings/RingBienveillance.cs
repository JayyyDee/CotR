using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RingBienveillance : Ring
{
    public GameObject bulletPrefab;
    private GameObject firePoint;
    private GameObject playerCharacter;


    public float fireForce = 1f;
    public float cooldown = 1f;
    private float timer;
    private bool canFire = true;
    private bool equiped = false;

    public GameObject activePrefab;
    public float activeCooldown = 5f;
    private float activeTimer;
    private bool canActive = true;

    public GameObject passivePrefab;

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
        if (equiped && Input.GetMouseButton(0) && canFire && IsOwner)
        {
            ShootServerRpc();
            canFire = false;
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
            canActive = false;
        }
    }


    public override void Shoot()
    {
        canFire = false;
        Vector3 pos = firePoint.transform.position;
        Quaternion rot = bulletPrefab.transform.rotation * firePoint.transform.rotation;
        GameObject bullet = Instantiate(bulletPrefab, pos, rot);
        bullet.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
        AkUnitySoundEngine.PostEvent("Play_FULL_Anneaux_Bienveillance_Attack_FULL__itemnumber", this.gameObject);
    }

    public override void Active()
    {
        Vector3 pos = firePoint.transform.position + (firePoint.transform.right * 3f);
        Quaternion rot = firePoint.transform.rotation;
        GameObject active = Instantiate(activePrefab, pos, rot);
        active.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
        AkUnitySoundEngine.PostEvent("Play_FULL_Anneaux_Bienveillance_Actif_FULL__itemnumber", this.gameObject);
        canActive = false;
    }

    public override void Passive()
    {
        PassiveServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void PassiveServerRpc()
    {
        GameObject passive = Instantiate(passivePrefab);
        passive.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
        AkUnitySoundEngine.PostEvent("Play_FULL_Anneaux_Bienveillance_Passif_FULL__itemnumber", this.gameObject);
    }

    [ClientRpc]
    public void PassiveClientRpc()
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


    [ServerRpc(RequireOwnership = false)]
    public void ShootServerRpc()
    {
        Shoot();
        //ShootClientRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void ActiveServerRpc()
    {
        Active();
    }

    

    public override void Drop()
    {
        Vector2 pos = new Vector2(playerCharacter.transform.position.x + Random.Range(0, 2f), playerCharacter.transform.position.y + Random.Range(0, 2f));
        AkUnitySoundEngine.PostEvent("Play_SFX_DropLoot__itemnumber", this.gameObject);
        DropServerRpc(pos);
    }

    public override int GetForm()
    {
        return 1;
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
