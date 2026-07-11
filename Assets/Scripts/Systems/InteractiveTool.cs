using System.Collections; // 👈 ¡Súper importante para la Corrutina!
using UnityEngine;
using UnityEngine.Events;

public class InteractiveTool : MonoBehaviour
{
    [Header("Configuración del Paso Táctico")]
    public string idPasoRequerido; 
    public bool esOpcionCorrecta = true; 
    [TextArea] public string mensajeDeError; 

    [Header("Eventos Extras")]
    public UnityEvent alInteractuar; 

    private MeshRenderer meshRenderer;
    private Color colorOriginal; 
    private bool estaEnfocado = false;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null) 
        {
            colorOriginal = meshRenderer.material.color; 
        }
    }

    public void Interactuar()
    {
        // Ejecuta los eventos configurados para este objeto
        alInteractuar.Invoke(); 

        if (StepValidator.Instance != null)
        {
            StepValidator.Instance.ValidarAccion(this);
        }
    }

    public void Enfocar()
    {
        estaEnfocado = true;
        if (meshRenderer != null)
        {
            meshRenderer.material.color = Color.yellow; 
            meshRenderer.material.EnableKeyword("_EMISSION");
            meshRenderer.material.SetColor("_EmissionColor", new Color(0.4f, 0.3f, 0.0f)); 
        }
    }

    public void Desenfocar()
    {
        estaEnfocado = false;
        if (meshRenderer != null)
        {
            meshRenderer.material.color = colorOriginal; 
            meshRenderer.material.DisableKeyword("_EMISSION");
        }
    }

    public void MostrarErrorVisual()
    {
        if (meshRenderer != null)
        {
            StartCoroutine(EfectoParpadeoError());
        }
    }

    private IEnumerator EfectoParpadeoError()
    {
        
        meshRenderer.material.color = Color.red;
        meshRenderer.material.EnableKeyword("_EMISSION");
        meshRenderer.material.SetColor("_EmissionColor", new Color(0.6f, 0.0f, 0.0f));

        yield return new WaitForSeconds(0.3f);

        if (estaEnfocado)
        {
            Enfocar();
        }
        else
        {
            Desenfocar();
        }
    }
}