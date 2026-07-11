using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Panel de alerta prominente para mensajes importantes entre fases.
// Coloca este script en un GameObject vacio en la escena y asigna las referencias.
public class AlertaPanel : MonoBehaviour
{
    public static AlertaPanel Instance;

    [Header("Referencias UI")]
    public GameObject panel;
    public TextMeshProUGUI titulo;
    public TextMeshProUGUI mensaje;

    void Awake()
    {
        Instance = this;
        if (panel != null) panel.SetActive(false);
    }

    // Muestra la alerta y llama accion al terminar
    public void MostrarAlerta(string tituloTexto, string mensajeTexto, float duracion, System.Action alTerminar = null)
    {
        StartCoroutine(MostrarCoroutine(tituloTexto, mensajeTexto, duracion, alTerminar));
    }

    IEnumerator MostrarCoroutine(string tituloTexto, string mensajeTexto, float duracion, System.Action alTerminar)
    {
        if (panel == null) yield break;

        if (titulo  != null) titulo.text  = tituloTexto;
        if (mensaje != null) mensaje.text = mensajeTexto;

        panel.SetActive(true);

        // Fade in
        CanvasGroup cg = panel.GetComponent<CanvasGroup>();
        if (cg == null) cg = panel.AddComponent<CanvasGroup>();

        cg.alpha = 0f;
        float t = 0f;
        while (t < 0.5f) { t += Time.deltaTime; cg.alpha = t / 0.5f; yield return null; }
        cg.alpha = 1f;

        yield return new WaitForSeconds(duracion);

        // Fade out
        t = 0f;
        while (t < 0.5f) { t += Time.deltaTime; cg.alpha = 1f - (t / 0.5f); yield return null; }
        cg.alpha = 0f;

        panel.SetActive(false);

        alTerminar?.Invoke();
    }
}
