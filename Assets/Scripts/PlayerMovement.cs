using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine.UI;

public class PlayerMovement : NetworkBehaviour
{
    public float speed { get; private set; }
    [SerializeField] private float movementSpeed = 1500;
    [SerializeField] private float maxSpeed = 1500;
    [SerializeField] private List<Vector3> spawnPositionList;
    private float moveHorizontal;
    private float moveVertical;
    private Ring ring;

    //Camera
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject playerCamera;

    private Inventory inventory;

    private bool playingFootsteps = false; // To check Are we playing the sound of footsteps currently
    public float footstepsSpeed = 0.2f; //BASE Time between playing each Footsteps sound //How fast we walk //will have to modif to match selon le speed animation

    private Rigidbody2D rb;

   
    void Start() {

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        inventory = gameObject.GetComponent<Inventory>();

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
    }

    private void FixedUpdate() //FixedUpdate for physics
    {
        //If the player is not the owner of the playable, can't access the movement.
        if (!IsOwner)
        {
            return;
        }

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
        //Then, make every of the six players spawn at specific places mentionned in a list.
        transform.position = spawnPositionList[(int)OwnerClientId];

        base.OnNetworkSpawn();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
    void Death()
    {
        mainCamera.gameObject.SetActive(true);
        playerCamera.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
