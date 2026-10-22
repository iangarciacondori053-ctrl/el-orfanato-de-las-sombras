using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 3.5f;
    public float runSpeed = 6f;
    public float gravity = -20f;

    [Header("Camera")]
    public Transform playerCamera;
    public float mouseSensitivity = 0.1f; // El Input System da pixeles, por eso el valor es mas bajo
    public float maxLookAngle = 80f;

    private CharacterController controller;
    private float verticalVelocity;
    private float cameraPitch;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (playerCamera == null)
        {
            Camera cam = GetComponentInChildren<Camera>();
            if (cam != null)
                playerCamera = cam.transform;
        }

        LockCursor();
    }

    void Update()
    {
        Keyboard keyboard = Keyboard.current;
        Mouse mouse = Mouse.current;
        if (keyboard == null || mouse == null) return;

        LookAround(mouse);
        MovePlayer(keyboard);

        // Escape libera el cursor
        if (keyboard.escapeKey.wasPressedThisFrame)
            UnlockCursor();

        // Clic izquierdo vuelve a bloquearlo
        if (Cursor.lockState != CursorLockMode.Locked && mouse.leftButton.wasPressedThisFrame)
            LockCursor();
    }

    void LookAround(Mouse mouse)
    {
        if (Cursor.lockState != CursorLockMode.Locked) return;

        Vector2 mouseDelta = mouse.delta.ReadValue() * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseDelta.x);

        cameraPitch -= mouseDelta.y;
        cameraPitch = Mathf.Clamp(cameraPitch, -maxLookAngle, maxLookAngle);

        if (playerCamera != null)
            playerCamera.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
    }

    void MovePlayer(Keyboard keyboard)
    {
        float x = 0f;
        float z = 0f;

        if (keyboard.dKey.isPressed) x += 1f;
        if (keyboard.aKey.isPressed) x -= 1f;
        if (keyboard.wKey.isPressed) z += 1f;
        if (keyboard.sKey.isPressed) z -= 1f;

        Vector3 move = transform.right * x + transform.forward * z;
        move = Vector3.ClampMagnitude(move, 1f);

        float speed = keyboard.leftShiftKey.isPressed ? runSpeed : moveSpeed;

        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = move * speed + Vector3.up * verticalVelocity;

        controller.Move(velocity * Time.deltaTime);
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}