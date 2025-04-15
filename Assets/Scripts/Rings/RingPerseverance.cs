using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class RingPerseverance : Ring
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
        if (equiped && Input.GetMouseButton(0) && canFire && playerCharacter.GetComponent<NetworkObject>().IsOwner)
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

        if (equiped && Input.GetKeyDown(KeyCode.LeftShift) && canActive && playerCharacter.GetComponent<NetworkObject>().IsOwner)
        {
            ActiveServerRpc();
        }
    }


    public override void Shoot()
    {
        canFire = false;
        Vector3 pos = firePoint.transform.position;
        
        Quaternion rot = bulletPrefab.transform.rotation*firePoint.transform.rotation;
        AkUnitySoundEngine.PostEvent("Play_FULL_Anneaux_Perseverence_Attack_TIR_FULL__itemnumber", this.gameObject);
        GameObject bullet = Instantiate(bulletPrefab, pos, rot);
        bullet.GetComponent<ProjectilePerseverance>().player = playerCharacter;
        bullet.GetComponent<NetworkObject>().Spawn();
        
    }

    public override void Active()
    {
        Vector3 pos = playerCharacter.transform.position;
        Quaternion rot = playerCharacter.transform.rotation;
        AkUnitySoundEngine.PostEvent("Play_FULL_Anneaux_Perseverence_Actif_Push_FULL__itemnumber", this.gameObject);
        AkUnitySoundEngine.PostEvent("Play_FULL_Anneaux_Perseverence_Actif_Start_FULL__itemnumber", this.gameObject);
        GameObject activeHitbox = Instantiate(activePrefab, pos, rot);
        activeHitbox.GetComponent<ActivePerseverance>().player = playerCharacter;
        activeHitbox.GetComponent<NetworkObject>().Spawn();
        
        canActive = false;
    }

    public override void Passive()
    {
        GameObject passiveHitbox = playerCharacter.transform.Find("PassivePerseverance").gameObject;
        passiveHitbox.GetComponent<PassivePerseverance>().player = playerCharacter;
        passiveHitbox.SetActive(true);
        
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
       // ShootClientRpc();
    }

    [ClientRpc]
    public void ShootClientRpc()
    {
        //Shoot();
    }

    [ServerRpc(RequireOwnership = false)]
    public void ActiveServerRpc()
    {
        ActiveClientRpc();
    }

    [ClientRpc]
    public void ActiveClientRpc()
    {
        Active();
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
        playerCharacter.transform.Find("PassivePerseverance").gameObject.SetActive(false);
        Vector2 pos = new Vector2(playerCharacter.transform.position.x + Random.Range(0, 2f), playerCharacter.transform.position.y + Random.Range(0, 2f));
        AkUnitySoundEngine.PostEvent("Play_SFX_DropLoot__itemnumber", this.gameObject);
        DropServerRpc(pos);
    }

    public override int GetForm()
    {
        return 6;
    }
}
