using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class InteractionSystem : MonoBehaviour
{
    [Header("Configuración de Interacción")]
    public float distanciaInteraccion = 3f; 
    public LayerMask capaInteractuable;     

    [Header("Referencias")]
    public Transform camaraPrincipal;
    public TextMeshProUGUI textoInteraccion; 

    private InteractiveTool objetoActual; 

    void Update()
    {
        DetectarObjeto();
    }

    void DetectarObjeto()
    {
        Ray rayo = new Ray(camaraPrincipal.position, camaraPrincipal.forward);

        if (Physics.Raycast(rayo, out RaycastHit impacto, distanciaInteraccion, capaInteractuable))
        {
            InteractiveTool objetoTocado = impacto.collider.GetComponent<InteractiveTool>();

            if (objetoTocado != null)
            {
                // Filtro para verificar si el objeto pertenece al paso actual
                if (objetoTocado.idPasoRequerido != "" && ProcedureManager.Instance != null && ProcedureManager.Instance.ObtenerPasoActualID() != objetoTocado.idPasoRequerido)
                {
                    ApagarObjetoActual();
                    return; 
                }

                if (objetoTocado != objetoActual)
                {
                    if (objetoActual != null) objetoActual.Desenfocar(); 
                    objetoActual = objetoTocado;
                    objetoActual.Enfocar(); 
                    if (textoInteraccion != null) textoInteraccion.gameObject.SetActive(true);
                }

                if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
                {
                    objetoActual.Interactuar();
                }
            }
        }
        else
        {
            ApagarObjetoActual();
        }
    }

    void ApagarObjetoActual()
    {
        if (objetoActual != null)
        {
            objetoActual.Desenfocar();
            objetoActual = null;
            if (textoInteraccion != null) textoInteraccion.gameObject.SetActive(false);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (camaraPrincipal == null) return;

        Ray rayo = new Ray(camaraPrincipal.position, camaraPrincipal.forward);
        
        // Dibuja verde si toca un objeto en la capa correcta, azul si no toca nada
        if (Physics.Raycast(rayo, out RaycastHit impacto, distanciaInteraccion, capaInteractuable))
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(camaraPrincipal.position, impacto.point);
        }
        else
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(camaraPrincipal.position, camaraPrincipal.position + camaraPrincipal.forward * distanciaInteraccion);
        }
    }
}