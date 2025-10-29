using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using Unity.Burst;
using System.Drawing;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;
using UnityEngine.Rendering;
using DG.Tweening.Core.Easing;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    float currentMoveSpeed;
    public float normalSpeed;
    public float dashSpeed;

    [HideInInspector]
    public float maxYSpeed;

    public float speedIncreaseMultiplier;
    public float speedBoostMultiplier;
    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    [SerializeField] float airMultiplier;
    bool readyToJump;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode pauseKey = KeyCode.Escape;

    [Header("Timers")]
    public float walkingSound_Timer = 0f, Boost_Timer = 5f;
    public float BoostTimeLeft;

    [Header("Ground Check")]
    public LayerMask whatIsGround;
    public bool grounded;
    //For Teleport Return Ability
    public bool overGround;
    Bounce_Pad Standing_On;
    Checkpoints checkpoints;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    RaycastHit slopeHit;
    bool exitingSlope;

    [Header("Blob Shadow")]
    public GameObject shadow;
    public RaycastHit hit;
    public float offset;

    [Header("References")]
    public Climbing climbingScript;
    public GameObject pauseMenu;
    public Transform orientation;
    [SerializeField] Animator deathAnim;
    public playerCam cam;
    Collider coll;

    [Header("Boost")]
    public GameObject BoostBarMeter;
    public GameObject speedParticle;

    Rigidbody rb;
    Vector3 spawnPoint;
    Vector3 moveDirection;
    float horizontalInput;
    float verticalInput;

    public MovementState movementState;
    public enum MovementState
    {
        NormalSpeed,
        BoostedSpeed,
        confused
    }
    public bool walking, inAir, wallrunning, climbing, playerIsMoving, dashing;

    void Start()
    {
        Physics.gravity = new Vector3(0, -30f, 0);

        spawnPoint = transform.position;
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        rb.freezeRotation = true;
        readyToJump = true;
    }

    void Update()
    {
        GroundCheck();

        walking = (Mathf.Abs(Input.GetAxisRaw("Horizontal")) + Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.2f);
        playerIsMoving = (Mathf.Abs(Input.GetAxisRaw("Horizontal")) + Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.1f);

        if (!pauseMenu.activeInHierarchy)
        MyInput();
        //Manages drag and different player speeds
        SpeedControl();
        //Manages the different player states
        StateHandler();
        //Handles timers for ability cooldowns
        //AbilityCooldownManager();

        if (gameObject.transform.position.y < -25f)
        {
            StartCoroutine(DeathScene());
        }

        
    }
    void FixedUpdate()
    {
        MovePlayer();
        
        Ray downRay = new Ray(new Vector3(this.transform.position.x, this.transform.position.y - offset, this.transform.position.z), -Vector3.up);

        //gets the hit from the raycast and converts it unto a vector3
        Vector3 hitPosition = hit.point;
        //transform the shadow to the location
        shadow.transform.position = hitPosition;

        //Cast a ray straight downwards, reads back where it leads
        if (Physics.Raycast(downRay, out hit))
        {
            print(hit.transform.tag);
        }
    }

    void MyInput()
    {
        if (Input.GetKey(KeyCode.P) || movementState == MovementState.confused)
        {
            //Reverse inputs when in confused state(A key moves player right, etc)
            horizontalInput = -Input.GetAxisRaw("Horizontal");
            verticalInput = -Input.GetAxisRaw("Vertical");
        }
        else
        {
            //Normal Inputs(A key move player left, etc)
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
        }

        // Calculate direction and walk in the direction you are looking
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        //Pause
        if (Input.GetKeyDown(pauseKey))
        {
            pauseMenu.SetActive(true);
        }
    }
    void StateHandler()
    {
        switch (movementState)
        {
            case MovementState.NormalSpeed:
                currentMoveSpeed = normalSpeed;
                break;
            case MovementState.BoostedSpeed:
                Boosted();
                break;
        }


        if (grounded || wallrunning || climbing)
        {
            coll.material.frictionCombine = PhysicsMaterialCombine.Average;
            Debug.Log("Average Friction");
        }
        else
        {
            coll.material.frictionCombine = PhysicsMaterialCombine.Minimum;
            Debug.Log("Minimum Friction");
        }
    }
    IEnumerator DeathScene()
    {
        deathAnim.Play("ScreenFade_In");
        yield return new WaitForSeconds(1.45f);
        RespawnPlayer();
        deathAnim.Play("ScreenFade_Out");
    }

    void MovePlayer()
    {
        if (climbingScript.exitingWall) return;

        //on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * currentMoveSpeed * 20f, ForceMode.Force);

            if(rb.linearVelocity.y > 0)
                rb.AddForce(Vector3.down * 1f, ForceMode.Force);
        }
        // on ground
        else if (grounded || wallrunning)
        {
            rb.AddForce(moveDirection.normalized * currentMoveSpeed * 10f, ForceMode.Force);
            //rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, desiredMoveSpeed);
        }
        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * currentMoveSpeed * 10f * airMultiplier, ForceMode.Force);

        //turn gravity off while on slope
        if (!wallrunning) rb.useGravity = !OnSlope();
    }

    void SpeedControl()
    {
        //limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.linearVelocity.magnitude > currentMoveSpeed)
                rb.linearVelocity = rb.linearVelocity.normalized * currentMoveSpeed;
        }

        //limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > currentMoveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * currentMoveSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }
        }

        //limit max y velocity
        if (maxYSpeed != 0  && rb.linearVelocity.y > maxYSpeed)
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, maxYSpeed, rb.linearVelocity.z);
    }

    void Jump()
    {
        exitingSlope = true;

        // reset y velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(transform.up * SetBounceStrength(), ForceMode.Impulse);
        //SoundManager.PlaySound(SoundSource.Player, SoundType.Player_Jumping, 0.2f, System.Random(0.9f, 1.2f);
    }
    void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, 1.2f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }
    public Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

   public void UpdateCheckpoint(Vector3 pos)
    {
        spawnPoint = pos;
    } 
    public void RespawnPlayer()
    {
        gameObject.transform.position = spawnPoint;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
    void GroundCheck()
    {
        // Ground Check 
        grounded = Physics.BoxCast(transform.position, transform.localScale * 0.25f, Vector3.down, transform.rotation, 1.2f, whatIsGround);
        //Debug.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - 1.2f, transform.position.z), Color.magenta);

        // Handle drag
        rb.linearDamping = (grounded) ? groundDrag : 0;
    }
    void OnTriggerEnter(Collider other)
    {
        //Death
        if (other.gameObject.CompareTag("Void"))
        {
            RespawnPlayer();
        }

        if (other.gameObject.GetComponent<Checkpoints>())
        {
            GameObject checkpoint = other.gameObject.GetComponent<GameObject>();
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Boost"))
        {
            movementState = MovementState.BoostedSpeed;
            BoostTimeLeft = Boost_Timer;
        }
        if (other.gameObject.CompareTag("Grass"))
        {
            movementState = MovementState.BoostedSpeed;
            BoostTimeLeft = 1.2f;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Bounce_Pad bouncePad = other.gameObject.GetComponent<Bounce_Pad>();
        if (bouncePad != null)
        {
            Standing_On = bouncePad;
        }

        /*if (other.gameObject.LayerMask("Ladder"))
        {
            if (Input.GetKey(jumpKey) && readyToJump && grounded)
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        }*/
    }

    private void OnCollisionExit(Collision other)
    {
        Bounce_Pad bouncePad = other.gameObject.GetComponent<Bounce_Pad>();
        if (bouncePad != null)
        {
            Standing_On = null;
        }

        if (other.gameObject.CompareTag("Slope"))
        {
            if (Input.GetKey(jumpKey) && readyToJump && grounded)
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        }

        if (other.gameObject.CompareTag("Grass") && grounded)
        {
            movementState = MovementState.NormalSpeed;
        }
    }
    public float SetBounceStrength()
    {
        if (Standing_On) return Standing_On.Bounce_Strength;
        return jumpForce;
    }

    void Boosted()
    {
        currentMoveSpeed = normalSpeed * speedBoostMultiplier;
        BoostTimeLeft -= Time.deltaTime;

        cam.DoFov(95f);
        if (BoostBarMeter != null)
            BoostBarMeter.gameObject.SetActive(true);

        if (speedParticle != null)
            speedParticle.SetActive(true);

        if (BoostTimeLeft <= 0f)
        {
            cam.DoFov(80f);
            if (BoostBarMeter != null)
                BoostBarMeter.gameObject.SetActive(false);
            if (speedParticle != null)
                speedParticle.SetActive(false);
            movementState = MovementState.NormalSpeed;
        }
    }
}