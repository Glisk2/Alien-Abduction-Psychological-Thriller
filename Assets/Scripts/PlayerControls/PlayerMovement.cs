using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;
    public Transform groundCheck;

    [SerializeField] private float normalSpeed = 12f;
    [SerializeField] private float sprintSpeed = 18f;
    [SerializeField] private float crouchSpeed = 6f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;
    
    private Vector3 velocity;
    private bool isGrounded;

    // Crouching scale ------------------------------------------
    private Vector3 originalScale;
    [SerializeField] private Vector3 crouchScale = new Vector3(1f, 0.5f, 1f); // Scale when crouching
    private bool isCrouching = false;
    private float currentSpeed;

    void Start()
    {
        // Store the original scale of the player object
        originalScale = transform.localScale;
        currentSpeed = normalSpeed;
    }

    
    void Update()
    {
        currentSpeed = normalSpeed;

        HandleGroundCheck();
        HandleCrouch();
        HandleSprint();
        HandleMovement();
        HandleJump();
        ApplyGravity();

    }  
    
    // Ground check Logic
    private void HandleGroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    // Basic movement logic
    public void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Determine movement direction
        Vector3 move = transform.right * x + transform.forward * z;

        // Apply movement
        controller.Move(move* currentSpeed * Time.deltaTime);
    }

    // Sprinting logic
    private void HandleSprint()
    {
        if(Input.GetKey(KeyCode.LeftShift) && isGrounded && !isCrouching)
        {
            currentSpeed = sprintSpeed; // Sprint speed
        }
    }

    // Crouching logic
    private void HandleCrouch()
    {
        if(Input.GetKey(KeyCode.LeftControl))
        {
            // Crouch: Scale down the player
            transform.localScale = crouchScale;
            currentSpeed = crouchSpeed; // Crouch speed
            isCrouching = true;
        }
        else
        {
            // Stand Up: reset the scale
            transform.localScale = originalScale;
            isCrouching = false;
        }
    }

    // Jumping logic
    private void HandleJump()
    {
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    // Apply gravity
    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

}
