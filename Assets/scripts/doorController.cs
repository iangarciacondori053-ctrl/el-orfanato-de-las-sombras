using UnityEngine;
using UnityEngine.InputSystem;

public class DoorController : MonoBehaviour
{
    [Header("Configuración")]
    public float openAngle = 90f;
    public float openSpeed = 3f;

    [Header("Input")]
    [SerializeField]
    private InputAction interactAction =
        new InputAction("Interact", InputActionType.Button, "<Keyboard>/e");

    private bool isOpen = false;
    private bool playerNearby = false;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    void OnEnable()
    {
        interactAction.Enable();
    }

    void OnDisable()
    {
        interactAction.Disable();
    }

    void Start()
    {
        closedRotation = transform.localRotation;
        openRotation = closedRotation * Quaternion.Euler(0f, openAngle, 0f);
    }

    void Update()
    {
        if (playerNearby && interactAction.WasPressedThisFrame())
        {
            isOpen = !isOpen;
        }

        Quaternion targetRotation = isOpen ? openRotation : closedRotation;

        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            targetRotation,
            Time.deltaTime * openSpeed
        );
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            Debug.Log("Pulsa E para abrir o cerrar la puerta.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
}