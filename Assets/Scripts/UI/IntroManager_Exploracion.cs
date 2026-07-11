using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Muestra una pantalla de intro con texto y luego la oculta
public class IntroManager_Exploracion : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject panelIntro;
    public TextMeshProUGUI textoIntro;

    [Header("Configuración")]
    [TextArea(3, 6)]
    public string mensaje = "Se detectó una emergencia...\n\nEncuentra todos los objetos que estan en el checklist antes de que sea tarde.";
    public float tiempoEspera   = 3f;   // segundos antes de empezar a desvanecer
    public float tiempoFadeOut  = 1.5f; // segundos que dura el fade

    private CanvasGroup canvasGroup;

    void Start()
    {
        if (panelIntro == null) return;

        canvasGroup = panelIntro.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = panelIntro.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 1f;
        panelIntro.SetActive(true);

        // Fondo negro que cubre toda la pantalla
        UnityEngine.UI.Image img = panelIntro.GetComponent<UnityEngine.UI.Image>();
        if (img != null)
            img.color = new Color(0f, 0f, 0f, 1f);

        RectTransform panelRT = panelIntro.GetComponent<RectTransform>();
        if (panelRT != null)
        {
            panelRT.anchorMin = Vector2.zero;
            panelRT.anchorMax = Vector2.one;
            panelRT.offsetMin = Vector2.zero;
            panelRT.offsetMax = Vector2.zero;
        }

        // Texto centrado
        if (textoIntro != null)
        {
            textoIntro.text      = mensaje;
            textoIntro.fontSize  = 36;
            textoIntro.color     = Color.white;
            textoIntro.alignment = TextAlignmentOptions.Center;

            RectTransform textRT = textoIntro.GetComponent<RectTransform>();
            textRT.anchorMin = Vector2.zero;
            textRT.anchorMax = Vector2.one;
            textRT.offsetMin = new Vector2(80f, 80f);
            textRT.offsetMax = new Vector2(-80f, -80f);
        }

        // Bloquear movimiento del jugador durante la intro
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible   = true;

        StartCoroutine(EjecutarIntro());
    }

    IEnumerator EjecutarIntro()
    {
        // Esperar que el jugador lea
        yield return new WaitForSeconds(tiempoEspera);

        // Fade out
        float tiempo = 0f;
        while (tiempo < tiempoFadeOut)
        {
            tiempo += Time.deltaTime;
            canvasGroup.alpha = 1f - (tiempo / tiempoFadeOut);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        panelIntro.SetActive(false);

        // Activar cursor FPS al terminar la intro
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible   = false;
    }
}
