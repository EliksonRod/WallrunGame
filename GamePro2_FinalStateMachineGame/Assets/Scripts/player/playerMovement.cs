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
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Settings")]
    public float normalSpeed = 11;
    public float groundDrag = 5;
    public float dashSpeed = 12;
    public float jumpForce = 13;
    [SerializeField] float airMovementMultiplier;
    [SerializeField] float Gravity = -30f;
    public bool canUseInput = true;

    [Header("Movement")]
    float currentMoveSpeed;

    [Header("Jumping")]
    float jumpCooldown = 0.25f;
    bool readyToJump;

    [Header("Dash Settings")]
    public float dashForce;
    public float dashFov;
    public float dashUpwardForce;
    public float maxDashYSpeed;
    public float dashDuration;
    public RectTransform[] DashCounters;
    float barWidth;
    int numberOfDashes = 3;

    [Header("Dash Options")]
    [SerializeField] bool InfiniteDashes = false;
    public bool useCameraForward = true;
    public bool allowAllDirections = true;
    public bool disableGravity = false;
    public bool resetVel = true;

    [Header("Dash Cooldown")]
    public float TimeBeforeDashRecharge = 1f;
    public float DashRechargeTimer;

    [Header("SpeedBoost Options")]
    public bool InfiniteBoost;

    [Header("Power Settings")]
    public float speedBoostMultiplier;

    [HideInInspector]
    public float maxYSpeed;

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
    public playerCam cam;
    public Transform orientation;
    public Climbing climbingScript; 
    public GameObject pauseMenu;
    public Animator deathAnim;

    [Header("Boost")]
    public GameObject BoostBarMeter;
    public GameObject speedParticle;

    [Header("Effects")]
    bool SpeedBoosted;
    bool Confused;

    Collider coll;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Vector2 moveInput;
    Vector3 spawnPoint;
    Vector3 moveDirection;

    public MovementState movementState;
    public enum MovementState
    {
        Default,
        Wallrunning,
        Dashing
    }
    public PlayerState playerState;
    public enum PlayerState
    {
        Nothing,
        //Boosted,
        Confused,
    }

    public bool walking, inAir, wallrunning, climbing, playerIsMoving, dashing;

    void Start()
    {
        Physics.gravity = new Vector3(0, Gravity, 0);

        spawnPoint = transform.position;
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        rb.freezeRotation = true;
        readyToJump = true;

        //Dash UI Setup
        for (int i = 0; i < DashCounters.Length; i++)
        {
            barWidth = DashCounters[i].anchorMax.y;
        }
    }
    public void Move(InputAction.CallbackContext context)
    {
        if (canUseInput == false) return;
        moveInput = context.ReadValue<Vector2>();
        Debug.Log(moveInput);
    }

    void Update()
    {
        GroundCheck();
        ChangeGravity(Gravity);
        walking = (Mathf.Abs(Input.GetAxisRaw("Horizontal")) + Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.2f);
        playerIsMoving = (Mathf.Abs(Input.GetAxisRaw("Horizontal")) + Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.1f);

        if (!pauseMenu.activeInHierarchy)
        MyInput();
        //Manages drag and different player speeds
        SpeedControl();
        //Manages the different player states
        StateHandler();
        //Handles timers for ability cooldowns
        DashUI();
        Boosted();

        if (gameObject.transform.position.y < -35f)
        {
            StartCoroutine(Die());
        }
    }
    void FixedUpdate()
    {
        if (movementState != MovementState.Dashing)
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
    void StateHandler()
    {
        switch (movementState)
        {
            case MovementState.Default:
                currentMoveSpeed = normalSpeed;
                break;
            //case MovementState.Boosted:
                //Boosted();
                //break;
        }

        if (grounded || wallrunning || climbing)
        {
            coll.material.frictionCombine = PhysicsMaterialCombine.Average;
            //Debug.Log("Average Friction");
        }
        else
        {
            coll.material.frictionCombine = PhysicsMaterialCombine.Minimum;
            //Debug.Log("Minimum Friction");
        }
    }

    void MyInput()
    {
        if (Input.GetKey(KeyCode.P) || playerState == PlayerState.Confused)
        {
            //Reverse inputs when in confused state(A key moves player right, etc)
            moveDirection = orientation.forward * -moveInput.y + orientation.right * -moveInput.x;
        }
        else
        {
            //Normal Inputs(A key move player left, etc)
            moveDirection = orientation.forward * moveInput.y + orientation.right * moveInput.x;
        }
    }
    public void PauseGame(InputAction.CallbackContext context)
    {
        pauseMenu.SetActive(true);
    }
    void MovePlayer()
    {
        if (climbingScript.exitingWall) return;

        //on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * currentMoveSpeed * 20f, ForceMode.Force);

            if (rb.linearVelocity.y > 0)
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
            rb.AddForce(moveDirection.normalized * currentMoveSpeed * 10f * airMovementMultiplier, ForceMode.Force);

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

    void GroundCheck()
    {
        // Ground Check 
        grounded = Physics.BoxCast(transform.position, transform.localScale * 0.25f, Vector3.down, transform.rotation, 1.2f, whatIsGround);
        //Debug.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - 1.2f, transform.position.z), Color.magenta);

        // Handle drag
        rb.linearDamping = (grounded) ? groundDrag : 0;
    }

    public void UpdateCheckpoint(Vector3 pos)
    {
        spawnPoint = pos;
    } 
    public void Respawn()
    {
        gameObject.transform.position = spawnPoint;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    IEnumerator Die()
    {
        deathAnim.Play("ScreenFade_In");
        yield return new WaitForSeconds(1.45f);
        Respawn();
        deathAnim.Play("ScreenFade_Out");
    }

    public void ChangeGravity(float gravityValue)
    {
        Physics.gravity = new Vector3(0, gravityValue, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        //Death
        if (other.gameObject.CompareTag("Void"))
        {
            Respawn();
        }

        if (other.gameObject.GetComponent<Checkpoints>())
        {
            GameObject checkpoint = other.gameObject.GetComponent<GameObject>();
        }

        //Instantly ends boost state if touched
        if (other.gameObject.CompareTag("EndBoost"))
        {
            StopBoost();
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Boost"))
        {
            SpeedBoosted = true;
            BoostTimeLeft = Boost_Timer;
        }
        if (other.gameObject.CompareTag("Grass"))
        {
            //playerState = PlayerState.Boosted;
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
    }

    private void OnCollisionExit(Collision other)
    {
        Bounce_Pad bouncePad = other.gameObject.GetComponent<Bounce_Pad>();
        if (bouncePad != null)
        {
            Standing_On = null;
        }

        if (other.gameObject.CompareTag("Grass") && grounded)
        {
            movementState = MovementState.Default;
        }
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (!canUseInput) return;

        if (readyToJump && grounded)
        {
            readyToJump = false;
            exitingSlope = true;

            // reset y velocity
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            rb.AddForce(transform.up * SetBounceStrength(), ForceMode.Impulse);
            //SoundManager.PlaySound(SoundSource.Player, SoundType.Player_Jumping, 0.2f, System.Random(0.9f, 1.2f);
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }
    public float SetBounceStrength()
    {
        if (Standing_On) return Standing_On.Bounce_Strength;
        return jumpForce;
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (!canUseInput) return;

        if ((wallrunning) || numberOfDashes <= 0) return;

        canUseInput = false;
        numberOfDashes -= 1;

        StopAllCoroutines();
        StartCoroutine(DashCooldown());

        movementState = MovementState.Dashing;
        maxYSpeed = maxDashYSpeed;

        cam.DoFov(dashFov);

        Transform forwardT;

        if (useCameraForward)
            forwardT = cam.camHolder; /// where you're looking
        else
            forwardT = orientation; /// where you're facing (no up or down)

        Vector3 direction = GetDashDirection(forwardT);

        Vector3 forceToApply = orientation.forward * (dashForce * 10) + orientation.up * dashUpwardForce;

        if (disableGravity)
            rb.useGravity = false;

        delayedForceToApply = forceToApply;

        //DelayedDashForce();
        Invoke(nameof(DelayedDashForce), 0.035f);

        Invoke(nameof(ResetDash), dashDuration);
    }
    private Vector3 delayedForceToApply;
    private void DelayedDashForce()
    {
        if (resetVel)
            rb.linearVelocity = Vector3.zero;

        rb.AddForce(delayedForceToApply, ForceMode.Impulse);

        movementState = MovementState.Default;
    }
    private void ResetDash()
    {
        maxYSpeed = 0;

        cam.DoFov(85f);

        if (disableGravity)
            rb.useGravity = true;

        canUseInput = true;
    }

    //Multidirectional dash support
    private Vector3 GetDashDirection(Transform forwardT)
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3();

        if (allowAllDirections)
            direction = forwardT.forward * verticalInput + forwardT.right * horizontalInput;
        else
            direction = forwardT.forward;

        if (verticalInput == 0 && horizontalInput == 0)
            direction = forwardT.forward;

        return direction.normalized;
    }
    ///Replenish dash charges over time with delay before starting recharge
    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(TimeBeforeDashRecharge);
        yield return new WaitForSeconds(DashRechargeTimer);

        if (numberOfDashes < 3)
        {
            numberOfDashes += 1;
        }

        yield return new WaitForSeconds(DashRechargeTimer);

        if (numberOfDashes < 3)
        {
            numberOfDashes += 1;
        }

        yield return new WaitForSeconds(DashRechargeTimer);

        if (numberOfDashes < 3)
        {
            numberOfDashes += 1;
        }
    }

    //Handle dash counter UI
    void DashUI()
    {
        if (numberOfDashes == 3)
        {
            DashCounters[0].anchorMax = new Vector2(DashCounters[0].anchorMax.x, barWidth);
            DashCounters[1].anchorMax = new Vector2(DashCounters[1].anchorMax.x, barWidth);
            DashCounters[2].anchorMax = new Vector2(DashCounters[2].anchorMax.x, barWidth);
        }
        else if (numberOfDashes == 2)
        {
            DashCounters[0].anchorMax = new Vector2(DashCounters[0].anchorMax.x, 0f);
            DashCounters[1].anchorMax = new Vector2(DashCounters[1].anchorMax.x, barWidth);
            DashCounters[2].anchorMax = new Vector2(DashCounters[2].anchorMax.x, barWidth);
        }
        else if (numberOfDashes == 1)
        {
            DashCounters[0].anchorMax = new Vector2(DashCounters[0].anchorMax.x, 0f);
            DashCounters[1].anchorMax = new Vector2(DashCounters[1].anchorMax.x, 0f);
            DashCounters[2].anchorMax = new Vector2(DashCounters[2].anchorMax.x, barWidth);
        }
        else if (numberOfDashes <= 0)
        {
            DashCounters[0].anchorMax = new Vector2(DashCounters[0].anchorMax.x, 0f);
            DashCounters[1].anchorMax = new Vector2(DashCounters[1].anchorMax.x, 0f);
            DashCounters[2].anchorMax = new Vector2(DashCounters[2].anchorMax.x, 0f);
        }
    }
    void Boosted()
    {
        //If Infinite, boost ends when colliding with tag
        if (!SpeedBoosted) return;

        currentMoveSpeed = normalSpeed * speedBoostMultiplier;

        cam.DoFov(95f);
        if (speedParticle != null)
            speedParticle.SetActive(true);

        //If timed, use timer bar and state will end
        if (!InfiniteBoost)
        {
            BoostTimeLeft = Mathf.Clamp(BoostTimeLeft, 0, Boost_Timer);
            BoostTimeLeft -= Time.deltaTime;

            if (BoostBarMeter != null)
                BoostBarMeter.gameObject.SetActive(true);

            if (BoostTimeLeft <= 0f)
            {
                cam.DoFov(80f);
                if (BoostBarMeter != null)
                    BoostBarMeter.gameObject.SetActive(false);
                if (speedParticle != null)
                    speedParticle.SetActive(false);

                SpeedBoosted = false;
            }
        }
    }

    void StopBoost()
    {
        cam.DoFov(80f);

        if (BoostBarMeter != null)
            BoostBarMeter.gameObject.SetActive(false);

        if (speedParticle != null)
            speedParticle.SetActive(false);

        SpeedBoosted = false;
    }
}