using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightController : MonoBehaviour
{
    public Light flashlight;

    private void Start()
    {
        if (flashlight == null)
            flashlight = GetComponent<Light>();

        Debug.Log("[Linterna] Start ejecutado. Luz: " + (flashlight != null ? flashlight.name : "NULL")
                  + " | Teclado: " + (Keyboard.current != null), this);
    }

    private void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.anyKey.wasPressedThisFrame)
            Debug.Log("[Linterna] Se pulso alguna tecla", this);

        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            if (flashlight == null) return;

            flashlight.enabled = !flashlight.enabled;
            Debug.Log("[Linterna] F pulsada. enabled = " + flashlight.enabled, this);
        }
    }
}