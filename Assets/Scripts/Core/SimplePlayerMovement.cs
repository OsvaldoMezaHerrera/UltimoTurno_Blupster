using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

[RequireComponent(typeof(CharacterController))]
public class SimplePlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad       = 5f;
    public float velocidadCarrera = 9f;
    public float gravedad        = -20f;
    public float alturasSalto    = 1.2f;

    [Header("Camara")]
    public float sensibilidad        = 0.15f;
    public float limiteVerticalArriba = 80f;
    public float limiteVerticalAbajo  = -80f;

    private CharacterController cc;
    private Transform camara;
    private float rotacionVertical = 0f;
    private float velocidadVertical = 0f;

    void Start()
    {
        cc = GetComponent<CharacterController>();

        if (Camera.main != null)
            camara = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible   = false;

        Debug.Log("[SimplePlayerMovement] Listo. CC=" + (cc != null) + " Cam=" + (camara != null));
    }

    void Update()
    {
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current == null) return;

        // ESC toggle cursor
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            bool locked = Cursor.lockState == CursorLockMode.Locked;
            Cursor.lockState = locked ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible   = locked;
        }

        // Rotacion con mouse
        if (Cursor.lockState == CursorLockMode.Locked && camara != null && Mouse.current != null)
        {
            Vector2 delta    = Mouse.current.delta.ReadValue();
            transform.Rotate(0f, delta.x * sensibilidad, 0f);
            rotacionVertical = Mathf.Clamp(rotacionVertical - delta.y * sensibilidad,
                                           limiteVerticalAbajo, limiteVerticalArriba);
            camara.localRotation = Quaternion.Euler(rotacionVertical, 0f, 0f);
        }

        // WASD
        float h = (Keyboard.current.dKey.isPressed ? 1f : 0f)
                - (Keyboard.current.aKey.isPressed ? 1f : 0f);
        float v = (Keyboard.current.wKey.isPressed ? 1f : 0f)
                - (Keyboard.current.sKey.isPressed ? 1f : 0f);

        // DEBUG temporal — quitar despues
        if (h != 0f || v != 0f)
            Debug.Log("[WASD] h=" + h + " v=" + v + " grounded=" + cc.isGrounded + " ccEnabled=" + cc.enabled);

        float speed = Keyboard.current.leftShiftKey.isPressed ? velocidadCarrera : velocidad;

        // Gravedad y salto
        if (cc.isGrounded && velocidadVertical < 0f)
            velocidadVertical = -2f;

        if (cc.isGrounded && Keyboard.current.spaceKey.wasPressedThisFrame)
            velocidadVertical = Mathf.Sqrt(alturasSalto * -2f * gravedad);

        velocidadVertical += gravedad * Time.deltaTime;

        // Movimiento final
        Vector3 movimiento = (transform.right * h + transform.forward * v).normalized * speed
                           + Vector3.up * velocidadVertical;
        cc.Move(movimiento * Time.deltaTime);
#else
        Debug.LogError("[SimplePlayerMovement] Activa New Input System en Project Settings.");
#endif
    }
}
