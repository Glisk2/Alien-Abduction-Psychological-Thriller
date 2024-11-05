using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    public float openAngle = 90f;   // Angle to open the door
    public float openSpeed = 2f;    // Speed of door opening/closing
    public bool isOpen = false;     // Current state of the door

    private Quaternion closedRotation; // Initial rotation (closed)
    private Quaternion openRotation;   // Target rotation (open)

    // Start is called before the first frame update
    void Start()
    {
        closedRotation = transform.localRotation;
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0); // Rotate around Y 
        Debug.Log("Closed Rotation: " + closedRotation);
        Debug.Log("Open Rotation: " + openRotation);
    }

    public void Interact()
    {
        Debug.Log("Interact called");
        StopAllCoroutines(); // Stops any existing rotation animations
        StartCoroutine(RotateDoor(isOpen ? closedRotation: openRotation));
        isOpen = !isOpen; // Toggle door state
    }

    private IEnumerator RotateDoor(Quaternion targetRotation)
    {
        while (Quaternion.Angle(transform.localRotation, targetRotation) > 0.1f)
        {
            Debug.Log("Current Rotation: " + transform.localRotation.eulerAngles);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * openSpeed);
            yield return null;
        }
        transform.localRotation = targetRotation; // Snap to target rotation
        Debug.Log("Final Rotation Reached: " + transform.localRotation.eulerAngles);
    }
}
