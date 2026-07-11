using UnityEngine;
using UnityEngine.InputSystem; // ¡Obligatorio para el Nuevo Sistema!

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;
    public float gravedad = -9.81f;
    
    [Header("Cámara y Mouse")]
    public Transform camaraJugador;
    public float sensibilidadMouse = 0.1f; 

    private CharacterController controller;
    private Vector3 velocidadVertical;
    private float rotacionX = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Bloquea el cursor en el centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (UIManager.Instance != null && UIManager.Instance.levelCompleted != null && UIManager.Instance.levelCompleted.activeSelf)
        {
            // Si el menú de nivel completado está activo, no permitimos mover al jugador
            return;
        }
        ManejarRotacion();
        ManejarMovimiento();
    }

    void ManejarMovimiento()
    {
        float moverHorizontal = 0f;
        float moverVertical = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) moverVertical += 1f;
            if (Keyboard.current.sKey.isPressed) moverVertical -= 1f;
            if (Keyboard.current.dKey.isPressed) moverHorizontal += 1f;
            if (Keyboard.current.aKey.isPressed) moverHorizontal -= 1f;
        }

        // Calcular dirección relativa a la vista del jugador
        Vector3 direccion = transform.right * moverHorizontal + transform.forward * moverVertical;
        controller.Move(direccion * velocidad * Time.deltaTime);

        // Gravedad
        if (controller.isGrounded && velocidadVertical.y < 0)
        {
            velocidadVertical.y = -2f;
        }

        velocidadVertical.y += gravedad * Time.deltaTime;
        controller.Move(velocidadVertical * Time.deltaTime);
    }

    void ManejarRotacion()
    {
        float mouseX = 0f;
        float mouseY = 0f;

        if (Mouse.current != null)
        {
            Vector2 deltaMouse = Mouse.current.delta.ReadValue();
            mouseX = deltaMouse.x * sensibilidadMouse;
            mouseY = deltaMouse.y * sensibilidadMouse;
        }

        // Rotar cuerpo (Eje Y)
        transform.Rotate(Vector3.up * mouseX);

        // Rotar cámara (Eje X)
        rotacionX -= mouseY;
        rotacionX = Mathf.Clamp(rotacionX, -80f, 80f);
        camaraJugador.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);
    }
}