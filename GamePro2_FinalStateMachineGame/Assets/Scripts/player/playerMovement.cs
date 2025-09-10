using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using Unity.Burst;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    float currentMoveSpeed;
    float desiredMoveSpeed;
    float lastDesiredMoveSpeed;
    public float walkSpeed;
    public float wallrunSpeed;
    public float climbSpeed;
    float drag;

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
    float BoostTimeLeft;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;
    Bounce_Pad Standing_On;

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
    //public ParticleSystem speedParticle;
    public GameObject speedParticle;

    public float verticalVelocity = 0f;

    Rigidbody rb;
    Vector3 spawnPoint;
    Vector3 moveDirection;
    float horizontalInput;
    float verticalInput;

    public MovementState state;
    public enum MovementState
    {
        walking,
        wallrunning,
        climbing,
        air,
        boosted
    }
    public bool walking, inAir, wallrunning, climbing, playerIsMoving;
    void Start()
    {
        spawnPoint = transform.position;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        //speedParticle.Stop();
    }

    void Update()
    {
        GroundDetection();

        walking = (Mathf.Abs(Input.GetAxisRaw("Horizontal")) + Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.2f);
        playerIsMoving = (Mathf.Abs(Input.GetAxisRaw("Horizontal")) + Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.1f);

        MyInput();
        SpeedControl();
        StateHandler();

        // handle drag
        if (grounded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;

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

        QuadraticDrag(drag);
    }

    void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

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
        switch(state)
        {
            case MovementState.walking:
                desiredMoveSpeed = walkSpeed;
                drag = 1f;
                break;
            case MovementState.wallrunning:
                desiredMoveSpeed = wallrunSpeed;
                drag = 3f;
                break;
            case MovementState.climbing:
                desiredMoveSpeed = climbSpeed;
                drag = 1f;
                break;
            case MovementState.air:
                drag = 15f;
                break;
            case MovementState.boosted:
                desiredMoveSpeed = walkSpeed * speedBoostMultiplier;
                cam.DoFov(95f);
                if (speedParticle != null)
                    speedParticle.SetActive(true);
                BoostTimeLeft -= Time.deltaTime;

                if (BoostTimeLeft <= 0f)
                {
                    //Has_Boost = false;
                    cam.DoFov(80f);
                    if(speedParticle != null)
                    speedParticle.SetActive(false);
                    state = MovementState.walking;
                }
                break;
        }

        //check if desiredMoveSpeed has changed drastically
        if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 20f && currentMoveSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            currentMoveSpeed = desiredMoveSpeed;
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
    }

    IEnumerator SmoothlyLerpMoveSpeed()
    {
        //smooothly lerp movementSpeed to desired value
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - currentMoveSpeed);
        float startValue = currentMoveSpeed;

        while (time < difference)
        {
            currentMoveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            if (OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeAngleIncrease;//slopeIncreaseMultiplier 
            }
            else
                time += Time.deltaTime * speedIncreaseMultiplier;

            yield return null;
        }

        currentMoveSpeed = desiredMoveSpeed;
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

        // calculate movement direction and walk in the direction you are looking
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * currentMoveSpeed * 20f, ForceMode.Force);

            if(rb.linearVelocity.y > 0)
                rb.AddForce(Vector3.down * 1f, ForceMode.Force);
        }

        // on ground
        else if (grounded || wallrunning)
            rb.AddForce(moveDirection.normalized * currentMoveSpeed * 10f, ForceMode.Force);

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
    }

    // Method to calculate drag force
    public static double CalculateDragForce(double dragCoefficient, double airDensity, double crossSectionalArea, double velocity)
    {
        // Applying the drag equation: Fd = 0.5 * Cd * rho * A * v^2
        double dragForce = 0.5 * dragCoefficient * airDensity * crossSectionalArea * Math.Pow(velocity, 2);
        return dragForce;
    }

    public double QuadraticDrag(double dragCoefficient)
    {
        // Example values for the drag force calculation
        //double dragCoefficient = 1;//0.47; // for a typical car
        double airDensity = 1.225;     // air density at sea level in kg/m³
        double crossSectionalArea = 2.5;  // in m² (example car)
        double velocity = currentMoveSpeed;      // speed in m/s

        double dragForce = CalculateDragForce(dragCoefficient, airDensity, crossSectionalArea, velocity);
        return -dragForce / 1;
    }

    void Jump()
    {
        exitingSlope = true;

        //SoundManager.PlaySound(SoundSource.Player, SoundType.Player_Jumping, 0.2f, System.Random(0.9f, 1.2f);

        // reset y velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(transform.up * SetBounceStrength(), ForceMode.Impulse);
    }
    void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }
    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
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

    void GroundDetection()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
    }
    void OnTriggerEnter(Collider other)
    {
        //Death
        if (other.gameObject.CompareTag("Void"))
        {
            RespawnPlayer();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Boost"))
        {
            state = MovementState.boosted;
            BoostTimeLeft = Boost_Timer;
        }
        if (other.gameObject.CompareTag("Grass"))
        {
            state = MovementState.boosted;
            BoostTimeLeft = 3f;
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

        if (other.gameObject.CompareTag("Slope"))
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        }
    }

    public float SetBounceStrength()
    {
        if (Standing_On) return Standing_On.Bounce_Strength;
        return jumpForce;
        
    }
}