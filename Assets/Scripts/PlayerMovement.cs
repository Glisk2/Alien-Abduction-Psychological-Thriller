using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;
    public Transform groundCheck;

    [SerializeField] private float speed = 12f;
    [SerializeField] private float sprintSpeed = 18f;
    [SerializeField] private float crouchSpeed = 3f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;
    
    Vector3 velocity;
    bool isGrounded;

    // Crouching scale ------------------------------------------
    private Vector3 originalScale;
    [SerializeField] private Vector3 crouchScale = new Vector3(1f, 0.5f, 1f); // Scale when crouching
    private bool isCrouching = false; //Track crouch state

    void Start()
    {
        // Store the original scale of the player object
        originalScale = transform.localScale;
    }

    
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Determine movement direction ----------------------------------------
        Vector3 move = transform.right * x + transform.forward * z;



        float currentSpeed = speed; //Start with normal speed


        // Handle Sprinting with left shift -------------------------------

        if(Input.GetKey(KeyCode.LeftShift) && isGrounded && ! isCrouching)
        {
            currentSpeed = sprintSpeed; //Use sprint speed
        }





        // Handle crouching with left control  ------------------------------------------
        if(Input.GetKey(KeyCode.LeftControl))
        {
            // Crouch: Scale down the player
            transform.localScale = crouchScale;
            currentSpeed = crouchSpeed; //Move at crouch speed
            isCrouching = true;
        }
        else
        {
            // Stand up: reset the scale
            transform.localScale = originalScale;
            isCrouching = false;
        }





        // Apply Movement ---------------------------------------------
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Jump Logic ------------------------------------------
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }


        // Gravity ------------------------------------------
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
