using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class Fase
{
    [TextArea(2, 4)]
    public string mensajeIntro;        // Mensaje que aparece al iniciar la fase
    public ObjectiveData[] objetivos;  // Objetivos de esta fase
}

// Controla las fases del juego.
// Coloca este script en un GameObject vacio llamado "FaseManager".
public class FaseManager : MonoBehaviour
{
    public static FaseManager Instance;

    [Header("Fases del juego")]
    public Fase[] fases;

    private int faseActual = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (fases == null || fases.Length == 0) return;

        if (ObjectiveManager.Instance != null)
            ObjectiveManager.Instance.OnJuegoCompletado += OnFaseCompletada;

        // Esperar un frame para que todo esté inicializado
        StartCoroutine(IniciarFase0());
    }

    IEnumerator IniciarFase0()
    {
        yield return null; // esperar un frame

        if (fases[0].objetivos != null && fases[0].objetivos.Length > 0)
            ObjectiveManager.Instance.CargarNuevaFase(fases[0].objetivos);

        // Mostrar mensaje de la primera fase
        yield return new WaitForSeconds(0.5f);
        if (!string.IsNullOrEmpty(fases[0].mensajeIntro) && FeedbackPanel.Instance != null)
            FeedbackPanel.Instance.MostrarMensaje(fases[0].mensajeIntro, 4f);
    }

    void OnDestroy()
    {
        if (ObjectiveManager.Instance != null)
            ObjectiveManager.Instance.OnJuegoCompletado -= OnFaseCompletada;
    }

    void OnFaseCompletada()
    {
        faseActual++;
        Debug.Log("[FaseManager] Fase completada. Siguiente fase: " + faseActual);

        if (faseActual >= fases.Length)
        {
            Debug.Log("[FaseManager] Juego completado totalmente.");
            if (FeedbackPanel.Instance != null)
                FeedbackPanel.Instance.MostrarMensaje("¡Misión completada! Has sobrevivido.", 5f);
            return;
        }

        StartCoroutine(TransicionFase());
    }

    IEnumerator TransicionFase()
    {
        Debug.Log("[FaseManager] Iniciando transición a fase " + faseActual);
        yield return new WaitForSeconds(2f);

        Fase siguiente = fases[faseActual];

        // Mostrar alerta prominente si existe AlertaPanel, si no usar FeedbackPanel
        if (AlertaPanel.Instance != null)
        {
            bool alertaTerminada = false;
            AlertaPanel.Instance.MostrarAlerta(
                "⚠ ALERTA DE EMERGENCIA",
                siguiente.mensajeIntro,
                4f,
                () => alertaTerminada = true
            );
            // Esperar a que termine la alerta
            yield return new WaitUntil(() => alertaTerminada);
        }
        else if (FeedbackPanel.Instance != null)
        {
            FeedbackPanel.Instance.MostrarMensaje(siguiente.mensajeIntro, 5f);
            yield return new WaitForSeconds(5f);
        }

        Debug.Log("[FaseManager] Cargando nueva fase con " + siguiente.objetivos.Length + " objetivos.");
        if (ObjectiveManager.Instance != null)
            ObjectiveManager.Instance.CargarNuevaFase(siguiente.objetivos);
        else
            Debug.LogWarning("[FaseManager] ObjectiveManager.Instance es null!");
    }
}
