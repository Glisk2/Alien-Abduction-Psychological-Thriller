using UnityEngine;

public class Sprinting : MonoBehaviour
{
    private PlayerMovement playerMovement;
    [SerializeField] private float sprintSpeed = 18f;


    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        //Check if sprint key is held
        if(Input.GetKey(KeyCode.LeftShift))
        {
            playerMovement.controller.Move(playerMovement.controller.velocity.normalized * sprintSpeed * Time.deltaTime);
        }
    }
}
