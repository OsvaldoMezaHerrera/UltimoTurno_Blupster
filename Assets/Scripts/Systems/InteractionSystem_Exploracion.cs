using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

// Gestiona la interaccion del jugador con objetos (tecla E)
public class InteractionSystem_Exploracion : MonoBehaviour
{
    [Header("Lista de objetivos a encontrar")]
    public ObjectiveData[] objetivos;

    [Header("Referencia al RaycastDetector")]
    public RaycastDetector raycastDetector;

    void Update()
    {
        // Detectar cuando el jugador presiona E (compatible con New Input System)
        #if ENABLE_INPUT_SYSTEM
        if (Keyboard.current.eKey.wasPressedThisFrame)
        #else
        if (Input.GetKeyDown(KeyCode.E))
        #endif
        {
            Interactuar();
        }
    }

    void Interactuar()
    {
        // Obtener el objeto que esta siendo apuntado
        InspectionObject objetoActual = raycastDetector.GetObjetoActual();

        if (objetoActual == null)
        {
            Debug.Log("No hay objeto interactivo en frente");
            return;
        }

        // Usar idObjeto del InspectionObject si está configurado;
        // si no, usar el nombre del GameObject como fallback
        string id = !string.IsNullOrEmpty(objetoActual.idObjeto)
            ? objetoActual.idObjeto
            : objetoActual.gameObject.name;

        Debug.Log("[InteractionSystem] Interactuando con: " + objetoActual.gameObject.name + " | id enviado: " + id);

        // Notificar al ObjectiveManager
        if (ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.RegistrarObjetivoEncontrado(id);
        }
        else
        {
            Debug.LogWarning("No hay ObjectiveManager en la escena");
        }

        // Destruir solo si el objeto está marcado para ello
        if (objetoActual.destruirAlInteractuar)
            Destroy(objetoActual.gameObject);
    }
}
