using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public float jumpForce;
    public float airMultiplier;
    public float jumpCooldown;
    bool readyToJump;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;
    public Transform orientation;

    float horizantalinput;
    float verticalinput;

    Vector3 moveDirection;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true; // freeze rotation of rigidbody
    }
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround); // check if player is grounded
        myInput();
        SpeedControl(); // limit velocity if needed
        if (grounded)
        {
            rb.linearDamping = groundDrag; // apply drag on ground
        }
        else
        {
            rb.linearDamping = 0; // remove drag in air
        }
    }

    void FixedUpdate()
    {
        MovePlayer();
    }
    private void myInput()
    {
        // get input
        horizantalinput = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right Arrow Keys
        verticalinput = Input.GetAxisRaw("Vertical"); // W/S or Up/Down Arrow Keys
        if (Input.GetKey(jumpKey) && readyToJump && grounded) // jump
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown); // reset jump after cooldown
        }


    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalinput + orientation.right * horizantalinput;
        if (grounded) // if grounded
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force); // calculate move direction
        else if (!grounded) // if not grounded
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force); // calculate move direction
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); // get flat velocity
        if (flatVel.magnitude > moveSpeed) // limit velocity if needed
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

    }
    private void ResetJump()
    {
        readyToJump = true;
    }
}

