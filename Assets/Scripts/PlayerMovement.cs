
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float MoveSpeed = 10f;
    [SerializeField] float SprintMultiplier = 1.5f;
    [SerializeField] float CrouchSpeed = 3f;
    [SerializeField] float CrouchHeight = 0.5f;
    [SerializeField] float StandHeight = 2f;
    [SerializeField] float LeanAngle = 15f;
    [SerializeField] float LeanSpeed = 5f;
    [SerializeField] float LeanOffset = 0.5f;

    [SerializeField] Transform cam;

    private bool isSprinting;
    private bool isCrouching;
    private float currentLeanAngle = 0f;
    private Vector3 defaultCameraPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        defaultCameraPosition = cam.localPosition;                //Store the default camera position
        transform.localScale = new Vector3(1f, StandHeight, 1f);  //This sets the player's standing height initially
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }

        if(Input.GetKey(KeyCode.LeftControl))
        {
            StartCrouch();
        }
        else
        {
            StopCrouch();
        }

        //Handle Leaning input
        float targetLeanAngle = 0f;
        Vector3 targetCamPosition = defaultCameraPosition;  // Reset to default camera position

        if(Input.GetKey(KeyCode.Q))
        {
            targetLeanAngle = LeanAngle; //Lean to the left
            targetCamPosition += cam.right * -LeanOffset;   //Move the camera to the left
        }
        else if(Input.GetKey(KeyCode.E))
        {
            targetLeanAngle = -LeanAngle; //Lean to the right
            targetCamPosition += cam.right * LeanOffset;   //Move the camera to the right
        }

        //Smoothly rotate the camera to the target lean angle
        currentLeanAngle = Mathf.Lerp(currentLeanAngle, targetLeanAngle, Time.deltaTime * LeanSpeed);
        LeanCamera(targetCamPosition);

        //Inputs
        float horInput = Input.GetAxisRaw("Horizontal") * MoveSpeed;
        float verInput = Input.GetAxisRaw("Vertical") * MoveSpeed;

        float currentSpeed = MoveSpeed;
        if(isCrouching)
        {
            currentSpeed = CrouchSpeed;
        }
        else if(isSprinting)
        {
            currentSpeed = MoveSpeed * SprintMultiplier;
        }

        //Camera direction
        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;

        camForward.y = 0;
        camRight.y = 0;

        //Creating relative cam direction
        Vector3 forwardRelative = verInput * camForward;
        Vector3 rightRelative = horInput * camRight;

        Vector3 moveDir = (forwardRelative + rightRelative).normalized * currentSpeed;

        //Movement
        rb.velocity = new Vector3 (moveDir.x, rb.velocity.y, moveDir.z);

        if(moveDir.magnitude > 0)
        {
            transform.forward = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }
    }

    private void LeanCamera(Vector3 targetCamPosition)
    {
        Vector3 leanOffsetVector = new Vector3(LeanOffset, 0, 0);
        
        //Apply the lean offset based on the direction of leaning
        if (currentLeanAngle > 0) //Leaning left
        {
            cam.localPosition = Vector3.Lerp(cam.localPosition, defaultCameraPosition - leanOffsetVector, Time.deltaTime * LeanSpeed);
            Debug.Log($"Leaning Left: {cam.localPosition}");
        }
        else if (currentLeanAngle < 0) //Leaning right
        {
            cam.localPosition = Vector3.Lerp(cam.localPosition, defaultCameraPosition + leanOffsetVector, Time.deltaTime * LeanSpeed);
            Debug.Log($"Leaning Right: {cam.localPosition}");
        }
        else
        {
            //Reset the camera to its default position if no leaning
            cam.localPosition = Vector3.Lerp(cam.localPosition, defaultCameraPosition, Time.deltaTime * LeanSpeed);
            Debug.Log($"Resetting Position: {cam.localPosition}");
        }

        //Apply the lean by rotating the camera around  its Z-axis
        cam.localRotation = Quaternion.Euler(cam.localRotation.eulerAngles.x, cam.localRotation.eulerAngles.y, currentLeanAngle);
    }

    private void StartCrouch()
    {
        isCrouching = true;
        transform.localScale = new Vector3(1f, CrouchHeight, 1f);
    }

    private void StopCrouch()
    {
        isCrouching = false;
        transform.localScale = new Vector3(1f, StandHeight, 1f);
    }
}
