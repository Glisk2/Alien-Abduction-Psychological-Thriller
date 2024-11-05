using UnityEngine;
using TMPro;


public class PlayerPickup : MonoBehaviour
{
    public Transform playerCamera;
    public Transform holdPosition;
    public LayerMask interactableLayer;
    public float pickupRange = 5f;
    public float throwForce = 5f;

    private GameObject heldObject;
    private Rigidbody heldObjectRb;
    private Vector3 originalScale;
    
    public TMP_Text interactText;

    void Start()
    {
        if(interactText != null)
        {
            interactText.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(heldObject == null)
            {
                TryPickUpOrInteract();
                HideInteractText();
            }
            else
            {
                DropObject();
            }
        }

        // Keep object scale consistent even if the player crouches or changes size
        if(heldObject != null)
        {
            UpdateHeldObjectPosition();
            heldObject.transform.localScale = originalScale; // Ensure scale remains constant
        }

        CheckForInteractable();
        Debug.DrawRay(playerCamera.position, playerCamera.forward * pickupRange, Color.red);

    }

    void CheckForInteractable()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupRange, interactableLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                ShowInteractText("Press E to interact");
            }
            else if (hit.collider.CompareTag("Pickable"))
            {
                ShowInteractText("Press E to pick up");
            }
            
        }
        else
        {
            HideInteractText();
        }
    }

    void ShowInteractText(string message)
    {
        if(interactText != null)
        {
            interactText.text = message;
            interactText.enabled = true;
        }
    }

    void HideInteractText()
    {
        if(interactText != null)
        {
            interactText.enabled = false;
        }
    }

    // Try to pick up the object
    void TryPickUpOrInteract()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward); // Cast ray from camera
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, pickupRange, interactableLayer)) // If ray hits something in range
        {
            // Check if the object hit is interactable
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact(); // Interact with the object
            }

            else if(hit.collider.CompareTag("Pickable")) // Check if object has the "Pickable" tag
            {

                PickUpObject(hit.collider.gameObject);
            }
        }
    }


    // Pick up the object
    void PickUpObject(GameObject pickableObject)
    {
        heldObject = pickableObject;
        heldObjectRb = heldObject.GetComponent<Rigidbody>();
        heldObjectRb.isKinematic = true; // Disable physics while holding

        // Save the original scale of the object
        originalScale = heldObject.transform.localScale;

        // Set initial position and rotation
        UpdateHeldObjectPosition();

        // Reset the scale to the original size
        heldObject.transform.localScale = originalScale;
    }


    // Drop or throw the object
    void DropObject()
    {
        // Detach the object from the player
        heldObject.transform.SetParent(null);

        // Set object to non-kinematic so it interacts with physics again
        heldObjectRb.isKinematic = false;

        // Optionally add a throw force if you want the player to throw it
        heldObjectRb.AddForce(playerCamera.forward * throwForce);

        // Reset the object's scale to its original size when dropping
        heldObject.transform.localScale = originalScale;

        // Clear the held object
        heldObject = null;
        heldObjectRb = null;
    }

    void UpdateHeldObjectPosition()
    {
        if(heldObject == null) return;

        heldObject.transform.position = holdPosition.position;

        // Check if the object has a PickableObject component and apply its custom holding
        PickableObject pickable = heldObject.GetComponent<PickableObject>();
        if(pickable != null)
        {
            // Apply the custom holding rotation for X and Z, but align Y with the player's forward
            Vector3 customRotation = pickable.holdingRotation;

            // Align the Y-axis rotation with the player's current forward direction
            float playerYRotation = playerCamera.eulerAngles.y;

            // Apply the rotation: player's Y-axis + custom holding x and z
            heldObject.transform.rotation = Quaternion.Euler(customRotation.x, playerYRotation, customRotation.z);
        }
        else
        {
            // If no custom holding rotation is set, align Y with player's forward and set X/Z to default
            float playerYRotation = playerCamera.eulerAngles.y;
            heldObject.transform.rotation = Quaternion.Euler(0f, playerYRotation, 0f);
        }
    }
}
