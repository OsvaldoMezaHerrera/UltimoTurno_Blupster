using System.Collections.Generic;
using UnityEngine;

public class ProcedureManager : MonoBehaviour
{
    [Header("Configuración del Nivel")]
    [Tooltip("Este valor debe coincidir con el valor de idProcedure del procedimiento deseado en el archivo StreamingAssets/procedures.json")]
    [SerializeField] public string idDelProcedimiento;

    private Procedure procedimientoDelNivel;
    private int currentStep = 0;

    public static ProcedureManager Instance;

    void Awake()
    {
        Instance = this;
    }

    // Se ejecuta cada que carga una escena
    void Start()
    {
        // 1. Valida que exista una instancia de GameManager y el procedimiento por su id
        if (GameManager.Instance != null && GameManager.Instance.procedures.proceduresDictionary.ContainsKey(idDelProcedimiento))
        {
            procedimientoDelNivel = GameManager.Instance.procedures.proceduresDictionary[idDelProcedimiento];
            // 2. Verificamos que el archivo tenga pasos configurados
            if (procedimientoDelNivel.procedureSteps == null || procedimientoDelNivel.procedureSteps.Count == 0)
            {
                Debug.LogWarning($"ADVERTENCIA: El ScriptableObject '{procedimientoDelNivel.name}' no tiene pasos configurados en su lista.");
            }
            else
            {
                Debug.Log($"Sistema listo. Nivel cargado: '{procedimientoDelNivel.idProcedure}' con {procedimientoDelNivel.procedureSteps.Count} pasos.");
            }
            // 3. Inicialización segura de la interfaz gráfica
            if (ProgressUI.Instance != null) 
                ProgressUI.Instance.InicializarBarra(ObtenerTotalPasos());
            
            if (UIManager.Instance != null) 
                UIManager.Instance.ActualizarTextos(ObtenerPasoActualInstruccion(), currentStep, ObtenerTotalPasos());
        } else
        {
            Debug.LogError($"ERROR: No se pudo encontrar el procedimiento con id '{idDelProcedimiento}'." +
                $"\n1. Asegurate de tener un GameObject con el script GameManager asignado y con su ScriptableObject correspondiente en la escena principal." +
                $"\n2. Verifica que el archivo StreamingAssets/procedures.json contenga el id deseado '{idDelProcedimiento}' en las claves idProcedure.");
            return;
        }
    }

    public void AvanzarPaso() 
    {
        currentStep++;
    }
    
    public string ObtenerPasoActualID()
    {
        if (procedimientoDelNivel == null || procedimientoDelNivel.procedureSteps == null || currentStep >= procedimientoDelNivel.procedureSteps.Count) return "";
        return procedimientoDelNivel.procedureSteps[currentStep].id;
    }
    
    public string ObtenerPasoActualInstruccion()
    {
        if (procedimientoDelNivel == null || procedimientoDelNivel.procedureSteps == null || currentStep >= procedimientoDelNivel.procedureSteps.Count) return "Procedimiento Completado";
        return procedimientoDelNivel.procedureSteps[currentStep].directionText;
    }
    
    public int ObtenerPasoActualIndice() => currentStep;
    
    public int ObtenerTotalPasos() => (procedimientoDelNivel != null && procedimientoDelNivel.procedureSteps != null) ? procedimientoDelNivel.procedureSteps.Count : 0;
    
    public bool EsUltimoPaso() => currentStep >= ObtenerTotalPasos();
}