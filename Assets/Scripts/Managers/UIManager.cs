using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Textos del HUD")]
    public TextMeshProUGUI textMisionActual;
    public TextMeshProUGUI textContadorPasos;

    [Header("Menú de Fin de Nivel")]
    public GameObject levelCompleted; 

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (levelCompleted != null)
        {
            levelCompleted.SetActive(false);
        }
    }

    public void ActualizarTextos(string instruccion, int pasoActual, int totalPasos)
    {
        if (textMisionActual != null) 
        {
            textMisionActual.text = instruccion;
        }
        else
        {
            Debug.LogWarning("UIManager: No has asignado el componente 'Text Mision Actual' en el Inspector.");
        }

        if (textContadorPasos != null) 
        {
            textContadorPasos.text = $"{pasoActual + 1} / {totalPasos}";
        }
        else
        {
            Debug.LogWarning("UIManager: No has asignado el componente 'Text Contador Pasos' en el Inspector.");
        }
    }

    public void MostrarMensajeError(string mensaje)
    {
        Debug.LogWarning($"[UI ALERTA] {mensaje}");
    }

    public void MostrarMenuFinDeNivel()
    {
        if (levelCompleted != null)
        {
            levelCompleted.SetActive(true);
            
            if (textMisionActual != null) 
            {
                textMisionActual.text = "¡Procedimiento Completado con Éxito!";
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            UnityEngine.Time.timeScale = 0f; 
        }
    }

    public void ReiniciarNivel()
    {
        UnityEngine.Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SiguienteNivel()
    {
        UnityEngine.Time.timeScale = 1f; 
        
        // CÓDIGO DINÁMICO PARA PASAR AL SIGUIENTE NIVEL 
        int escenaActual = SceneManager.GetActiveScene().buildIndex;
        int siguienteEscena = escenaActual + 1;

        // Verificamos que la siguiente escena exista en el Build Settings
        if (siguienteEscena < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(siguienteEscena);
        }
        else
        {
            // Si ya no hay más escenas, puedes mostrar un mensaje o mandarlo al Menú Principal (0)
            Debug.Log("¡Completaste el último nivel del juego!");
            
            // Opcional: Descomenta la siguiente línea si quieres que regrese a un Menú Principal al terminar todo
            // SceneManager.LoadScene(0); 
        }
    }
}