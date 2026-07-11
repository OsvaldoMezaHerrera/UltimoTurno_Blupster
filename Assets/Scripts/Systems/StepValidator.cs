using UnityEngine;

public class StepValidator : MonoBehaviour
{
    public static StepValidator Instance;

    void Awake()
    {
        Instance = this;
    }

    public void ValidarAccion(InteractiveTool herramientaUsada)
    {
        if (herramientaUsada.esOpcionCorrecta)
        {
            Debug.Log($"Acción Exitosa con: {herramientaUsada.gameObject.name}");
            
            ProcedureManager.Instance.AvanzarPaso();
            ProgressUI.Instance.ActualizarBarraVisual(ProcedureManager.Instance.ObtenerPasoActualIndice());

            if (ProcedureManager.Instance.EsUltimoPaso())
            {
                Debug.Log("Nivel completado con éxito. Mostrando menú de fin de nivel...");
                
                if (UIManager.Instance != null)
                {
                    UIManager.Instance.MostrarMenuFinDeNivel();
                }
            }
            else
            {
                UIManager.Instance.ActualizarTextos(
                    ProcedureManager.Instance.ObtenerPasoActualInstruccion(),
                    ProcedureManager.Instance.ObtenerPasoActualIndice(),
                    ProcedureManager.Instance.ObtenerTotalPasos()
                );
            }

            herramientaUsada.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning($"Herramienta incorrecta: {herramientaUsada.mensajeDeError}");
            
            herramientaUsada.MostrarErrorVisual();
            
            if (UIManager.Instance != null)
            {
                UIManager.Instance.MostrarMensajeError(herramientaUsada.mensajeDeError);
            }
        }
    }
}