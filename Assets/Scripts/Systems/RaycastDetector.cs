using UnityEngine;

public class RaycastDetector : MonoBehaviour
{
    public float distanciaRaycast = 20f;

    // Si está en true, detecta TODOS los objetos (sin filtro de layer)
    // Ponlo en false cuando el layer "Interactivo" esté configurado
    public bool detectarTodo = true;

    private int layerMask;
    private InspectionObject objetoActual = null;

    void Start()
    {
        // Obtener la máscara del layer "Interactivo"
        layerMask = LayerMask.GetMask("Interactivo");

        if (layerMask == 0)
            Debug.LogWarning("⚠️ RaycastDetector: No existe el layer 'Interactivo'. Activando modo detectarTodo.");

        Debug.Log("RaycastDetector iniciado. DetectarTodo: " + detectarTodo + " | LayerMask: " + layerMask);
    }

        // Devuelve el objeto actualmente detectado
    public InspectionObject GetObjetoActual()
    {
        return objetoActual;
    }

    void Update()
    {
        Ray rayo = new Ray(transform.position, transform.forward);
        RaycastHit golpe;

        // Dibujar el rayo en la vista Scene (línea roja)
        Debug.DrawRay(rayo.origin, rayo.direction * distanciaRaycast, Color.red);

        // Decidir si usar layer o detectar todo
        bool impacto = detectarTodo
            ? Physics.Raycast(rayo, out golpe, distanciaRaycast)
            : Physics.Raycast(rayo, out golpe, distanciaRaycast, layerMask);

        if (impacto)
        {
            InspectionObject objeto = golpe.collider.GetComponent<InspectionObject>();

            if (objeto != null)
            {
                // Si es un objeto nuevo, cambiar detección
                if (objetoActual != objeto)
                {
                    if (objetoActual != null)
                        objetoActual.DesactivarDeteccion();

                    objetoActual = objeto;
                    objetoActual.ActivarDeteccion();
                }
            }
            else
            {
                // Golpeó algo que no es interactivo
                if (objetoActual != null)
                {
                    objetoActual.DesactivarDeteccion();
                    objetoActual = null;
                }
            }
        }
        else
        {
            // No golpeó nada
            if (objetoActual != null)
            {
                objetoActual.DesactivarDeteccion();
                objetoActual = null;
            }
        }
    }
}