using UnityEngine;

public class LightSwitch : MonoBehaviour, IInteractable
{
    public Light lightSource; // Reference to the Light in the scene

    public void Interact()
    {
        // Toggle light on/off
        lightSource.enabled = !lightSource.enabled;
        Debug.Log("Light toggled");
    }
}
