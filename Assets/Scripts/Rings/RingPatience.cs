using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RingPatience : Ring
{
    public GameObject bulletPrefab;

    
    private GameObject playerCharacter;
    private Vector2 mousePos;
    private Camera cam;

    
    public float range;
    private Queue<GameObject> zones= new Queue<GameObject>();
    private Vector2 spawnPoint;

    public float activeCooldown;
    private float activeTimer;
    private bool canActive = true;

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
        if (equiped)
        {
            playerCharacter.transform.Find("RangePatience").gameObject.GetComponent<SpriteRenderer>().enabled = true;

        }
        else if(playerCharacter)
        {
            
            playerCharacter.transform.Find("RangePatience").gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        
        


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
            Active();
        }
    }


    public override void Shoot()
    {
        if (cam && IsOwner && equiped)
        {
            Debug.Log(cam.ScreenToWorldPoint(Input.mousePosition));
            mousePos = new Vector2(cam.ScreenToWorldPoint(Input.mousePosition).x, cam.ScreenToWorldPoint(Input.mousePosition).y);
            Vector2 playerPos = new Vector2(playerCharacter.transform.position.x, playerCharacter.transform.position.y);
            Vector2 direction = mousePos - playerPos;

            if (direction.magnitude > range)
            {
                spawnPoint = direction.normalized * range + playerPos;

            }
            else
            {
                spawnPoint = mousePos;
            }
        }

        canFire = false;
        GameObject bullet = Instantiate(bulletPrefab,spawnPoint, Quaternion.identity);
        bullet.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
        zones.Enqueue(bullet);
        AkUnitySoundEngine.PostEvent("Play_Anneaux_Patience_Attack_Trow_PT2__itemnumber", this.gameObject);
        if (zones.Count > 3)
        {
            GameObject toDestroy = zones.Dequeue();
            toDestroy.GetComponent<NetworkObject>().Despawn();
            Destroy(toDestroy);
        }

        
    }

    public override void Active()
    {
        if (zones.Count > 0)
        {
            AkUnitySoundEngine.PostEvent("Play_Anneaux_Patience_Attack_Trow_Impact_Damage__itemnumber", this.gameObject);
            foreach (GameObject zone in zones)
            {
                zone.GetComponent<ZonePatience>().Detonate();
            }
            zones.Clear();
        }
        
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
        playerCharacter.transform.Find("RangePatience").gameObject.SetActive(true);
        Vector2 pos = new Vector2(playerCharacter.transform.position.x + Random.Range(0, 2f), playerCharacter.transform.position.y + Random.Range(0, 2f));
        AkUnitySoundEngine.PostEvent("Play_SFX_DropLoot__itemnumber", this.gameObject);
        DropServerRpc(pos);
    }
    public void SetCamera(Camera camera)
    {
        cam = camera;
    }

    public override int GetForm()
    {
        return 5;
    }
}
