using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : NetworkBehaviour
{
    public float speed { get; private set; }
    [SerializeField] private float movementSpeed = 1500;
    [SerializeField] private float maxSpeed = 1500;
    [SerializeField] private Transform spawnPoint;
    private float moveHorizontal;
    private float moveVertical;

    //Camera
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject playerCamera;


    private bool playingFootsteps = false; // To check Are we playing the sound of footsteps currently
    public float footstepsSpeed = 0.2f; //BASE Time between playing each Footsteps sound //How fast we walk //will have to modif to match selon le speed animation

    private List<GameObject> inventory = new List<GameObject>();

    [SerializeField] public Image gemIcon;
    private int gemCounter = 0;

    public HealthBarManager healthBar;
    public int maxHealth = 1000;
    private int currentHealth;
    //private NetworkVariable<int> maxHealth = new NetworkVariable<int>(1000);
    //private NetworkVariable<int> currentHealth = new NetworkVariable<int>(); //NetworkVariable = Every time this value is changed, all of the client gets updated

    private Rigidbody2D rb;

    public Ring ring;
    private Vector2 mousePosition;

    //Gamemode Gemme
    float currentTime;
    public float startingTime = 22f;

    [SerializeField] Text countdownText;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        //Setup for the healthbar
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);


        //Set the player's spawn point on the spawnpoint location.
        gameObject.transform.position = spawnPoint.position;

        //Gamemode Gemme Timer
        countdownText.enabled = false;
        currentTime = startingTime;

    }



    void Update() 
    {
        //Get the value (1 or -1) for the movement
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        //StartFootsteps AKA the condition to enable or disable the sounds of walking
        if (moveVertical > 0 && !playingFootsteps || moveVertical < 0 && !playingFootsteps || moveHorizontal > 0 && !playingFootsteps || moveHorizontal < 0 && !playingFootsteps) 
        {
            StartFootsteps();
        }
        else if (moveVertical == 0 && moveHorizontal == 0) 
        {
            StopFootsteps();
        }

        //For animation, the animation will start on the front.
        speed = moveVertical;

        //Test for aiming with mouse 
        //mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //To test for damage, press O
        if (Input.GetKeyDown(KeyCode.O))
        {
            TakeDamageServerRpc(100);
        }

        //To test for healing, press P
        if (Input.GetKeyDown(KeyCode.P))
        {
            HealingServerRpc(100);
        }
        //To shoot a bullet
        if (Input.GetKeyDown(KeyCode.E))
        {
            ring.Shoot(transform);
        }

    }

    private void FixedUpdate() //FixedUpdate for physics
    {
        //If the player is not the owner of the playable, can't access the movement.
        if (!IsOwner)
        {
            return;
        }

        ////Testing the aiming (again)
        //Vector2 aimDirection = mousePosition - rb.position;
        //float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        //rb.rotation = aimAngle;

        PlayerMov();

    }

    public override void OnNetworkSpawn()
    {
        //When the player spawns, attach the playerCamera to the player that enters the lobby.
        if (IsOwner) {

            playerCamera.gameObject.SetActive(true);
            mainCamera.gameObject.SetActive(false);
            playerCamera.GetComponent<CameraMovement>().player = this.transform;
        }

        base.OnNetworkSpawn();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Move in GemManager, when colliding with the gem, set inactive and play sound.
        if (collision.CompareTag("Gemme"))
        {
            collision.gameObject.SetActive(false);
            gemIcon.gameObject.SetActive(true);
            gemCounter = 1;
            AkUnitySoundEngine.PostEvent("Event_Jadeide_Slow__Pickup", this.gameObject); // The Event to play sounds of collecting the Jadeide

            countdownText.enabled = true;
            currentTime = -1 * Time.deltaTime;
            countdownText.text = currentTime.ToString("0");

            if (currentTime <= 0)
            {
                currentTime = 0;
                // Your Code Here
            }
        }

        if (collision.CompareTag("Ring"))
        {
            
            inventory.Add(collision.gameObject);
            ring = collision.gameObject.GetComponent<Ring>();
            collision.gameObject.SetActive(false);
            string fullInv = "";
            foreach(GameObject gameObject in inventory)
            {
                fullInv += (gameObject.name + " ");
            }
            Debug.Log(fullInv);
        }

    }
    private void PlayerMov()
    {
        if (speed == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }

        Vector3 movement = new Vector3(moveHorizontal, moveVertical);
        //Apply force to the rigidbody to move it.
        rb.AddForce(movement * movementSpeed * Time.fixedDeltaTime);

        //If the horizontal velocity is more than the max speed, set velocity to the maxSpeed to not go pass it.
        if (rb.velocity.x > maxSpeed)
        {
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
        }

        //If the vertical velocity is more than the max speed, set velocity to the maxSpeed to not go pass it.
        if (rb.velocity.y > maxSpeed)
        {
            rb.velocity = new Vector2(maxSpeed, rb.velocity.x);
        }
    }

    void StartFootsteps()  //Start walking sound effect
    {
        playingFootsteps = true;
        InvokeRepeating(nameof(PlayFootsteps), 0f, footstepsSpeed);
    }
    void StopFootsteps() //Stop walking sound effect
    {
        playingFootsteps = false;
        CancelInvoke(nameof(PlayFootsteps));
    }

    void PlayFootsteps()  //The Event to play sounds of footsteps
    {
        AkUnitySoundEngine.PostEvent("Event_Footstep", this.gameObject);
    }

    [ServerRpc]
    private void TakeDamageServerRpc(int damage)
    {
        TakeDamageClientRpc(damage);
    }

    [ClientRpc]
    private void TakeDamageClientRpc(int damage)
    {
            if (currentHealth > 0)
            {
                currentHealth -= damage;
                healthBar.SetHealth(currentHealth);
            }

            if (currentHealth <= 0)
            {
                gameObject.SetActive(false);
                Invoke("Death", 2);

            }
    }
    [ServerRpc]
    private void HealingServerRpc(int heal)
    {
        HealingClientRpc(heal);
    }

    [ClientRpc]
    void HealingClientRpc(int heal)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += heal;
            healthBar.SetHealth(currentHealth);
        }
    }

    void Death ()
    {
        mainCamera.gameObject.SetActive(true);
        playerCamera.gameObject.SetActive(false);
        gameObject.SetActive(false);
        gameObject.transform.position = spawnPoint.position;
    }
}
