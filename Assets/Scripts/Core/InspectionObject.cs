using UnityEngine;

// Este script va en cada objeto que el jugador puede inspeccionar
public class InspectionObject : MonoBehaviour
{
    [Header("Identificación del objetivo")]
    [Tooltip("Debe coincidir con el idObjeto del ScriptableObject correspondiente.\nSi está vacío, se usará el nombre del GameObject como fallback.")]
    public string idObjeto;

    [Tooltip("Actívalo si el objeto se recoge y desaparece (muestras, dinero, etc.).\nDesactívalo para paneles, válvulas, puertas que solo se interactúan.")]
    public bool destruirAlInteractuar = true;

    // Color del glow cuando el jugador apunta
    public Color colorGlow = Color.yellow;

    // Referencias internas
    private Renderer meshRenderer;
    private MaterialPropertyBlock propBlock;
    private Color colorOriginal;

    void Start()
    {
        // Obtener el renderer del objeto
        meshRenderer = GetComponent<Renderer>();

        if (meshRenderer == null)
        {
            Debug.LogWarning("InspectionObject: No se encontró Renderer en " + gameObject.name);
            return;
        }

        // Crear el bloque de propiedades (compatible con URP y cualquier shader)
        propBlock = new MaterialPropertyBlock();

        // Guardar el color original usando _BaseColor (URP) o _Color (Standard)
        meshRenderer.GetPropertyBlock(propBlock);
        if (meshRenderer.sharedMaterial.HasProperty("_BaseColor"))
            colorOriginal = meshRenderer.sharedMaterial.GetColor("_BaseColor");
        else
            colorOriginal = meshRenderer.sharedMaterial.color;

        Debug.Log("InspectionObject listo en: " + gameObject.name + " | Color original: " + colorOriginal);
    }

    // Se llama cuando el raycast detecta este objeto
    public void ActivarDeteccion()
    {
        if (meshRenderer == null) return;

        meshRenderer.GetPropertyBlock(propBlock);
        propBlock.SetColor("_BaseColor", colorGlow);
        meshRenderer.SetPropertyBlock(propBlock);

        Debug.Log("Detectado: " + gameObject.name);
    }

    // Se llama cuando el raycast deja de detectar este objeto
    public void DesactivarDeteccion()
    {
        if (meshRenderer == null) return;

        meshRenderer.GetPropertyBlock(propBlock);
        propBlock.SetColor("_BaseColor", colorOriginal);
        meshRenderer.SetPropertyBlock(propBlock);
    }
}