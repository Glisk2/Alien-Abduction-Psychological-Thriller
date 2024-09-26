using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float MoveSpeed = 10f;
    [SerializeField] float SprintMultiplier = 1.5f;

    [SerializeField] Transform cam;

    private bool isSprinting;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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

        //Inputs
        float horInput = Input.GetAxisRaw("Horizontal") * MoveSpeed;
        float verInput = Input.GetAxisRaw("Vertical") * MoveSpeed;

        float currentSpeed = isSprinting ? MoveSpeed * SprintMultiplier : MoveSpeed;

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
}
