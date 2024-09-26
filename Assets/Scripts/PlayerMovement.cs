using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float MoveSpeed = 10;
    [SerializeField] float Jump = 5;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal") * MoveSpeed;
        float verticalInput = Input.GetAxisRaw("Vertical") * MoveSpeed;

        rb.velocity = new Vector3(horizontalInput, 0, verticalInput);
        
        if(Input.GetButtonDown("Jump")) rb.velocity = new Vector3(rb.velocity.x, Jump, rb.velocity.z);
    }   
}
