using UnityEngine;

public class playerFPS : MonoBehaviour
{
    public Vector3 Velocity;
    public Vector3 PlayerMovementInput;
    public Vector2 PlayerMouseInput;
    private float xRotation;

    public Transform PlayerCamera;
    public CharacterController Controller;
    public Transform Player;

    [Header("Movement")]
    [SerializeField] private float Speed;
    [SerializeField] private float JumpForce;
    [SerializeField] private float Sensitivity;
    [SerializeField] private float Gravity = 9.81f;
    [SerializeField] private float sprintMoveSpeed;

    [Header("Sneaking")]
    private bool Sneak = false;
    private bool isSneaking = false;
    private bool isOnGround;
    public float SneakSpeed;

    //bool crouched = false;


    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    // Update is called once per frame
    void Update()
    {
        playerMovement();
        MoveCamera();
        Jump();
        Crouch();

        //run a check for crouch or jump keys, then set a boolean equal to true if requested
    }

    private void FixedUpdate()
    {
        //inside fixedUpdate we put all the physics or simulated physics
        //updates - like player move

        //first call a direction to find current requested inputs
        //if the player has requested jump or crouch, update status and run jump or crouch functions
        //update player state and booleans for jump,crouch,etc
        //based on status, run movement functions here
    }

    private void LateUpdate()
    {
        //if Update produces jitter, which it usually does for cameras,
        //then run your camera code inside LateUpdate instead
    }

    private void playerMovement()
    {
        Controller.Move(Velocity * Time.deltaTime);

        //WASD movement
        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        //Aim movement (First Person)
        PlayerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        // If left shift is held down, use the runningSpeed as the speed
        if (Input.GetKey(KeyCode.LeftShift))
        {
            //rb.AddForce(Speed * sprintMoveSpeed, ForceMode.Force);
        }
        // Else (left shift is not held down) use the normal speed
        else
        {
            // Move the player
            //rb.AddForce(movementDirection * speed, ForceMode.Force);
        }
    }

    private void Walk()
    {
       
    }
    private void Jump()
    {
        //make the player jump
        if (Controller.isGrounded)
        {
            Velocity.y = -1f;
            isOnGround = true;

            if (Input.GetKeyDown(KeyCode.Space) && isSneaking == false)
            {
                Velocity.y = JumpForce;
            }
        }
        else
        {
            Velocity.y += Gravity * -2f * Time.deltaTime;
            isOnGround = false;
        }
    }

    private void EnterCrouch()
    {
        //update booleans or enums in here
        //one thing we might check for is grounded status
        //if grounded fails, reject the attempt to enterCrouch  
    }
    private void ExitCrouch()
    {

    }

    void Crouch()
    {
        //run the actual movement code
        //when leftCTRL pressed and sneak is false: crouch and tell unity we are sneaking
        if (Input.GetKey(KeyCode.LeftControl) && Sneak)
        {
            Player.localScale = new Vector3(1f, 0.5f, 1f);
            isSneaking = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            Player.localScale = new Vector3(1f, 1f, 1f);
            isSneaking = false;
        }

        Vector3 MoveVector = transform.TransformDirection(PlayerMovementInput); 

        //when sneaking is true, decrease speed: have normal speed otherwise
        if (isSneaking)
        {
            Controller.Move(MoveVector * SneakSpeed * Time.deltaTime);
        }
        else
        {
            Controller.Move(MoveVector * Speed * Time.deltaTime);
        }
    }
    private void MoveCamera()
    {
        xRotation -= PlayerMouseInput.y * Sensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.Rotate(0f, PlayerMouseInput.x * Sensitivity, 0f);
        PlayerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
