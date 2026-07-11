using System.Collections;
using TMPro;
using UnityEngine;

public class FeedbackPanel : MonoBehaviour
{
    public static FeedbackPanel Instance;

    [Header("Referencias UI")]
    public GameObject panel;
    public TextMeshProUGUI mensajeText;

    [Header("Configuración")]
    public float duracionPorDefecto = 2.5f;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (panel != null)
            panel.SetActive(false);
    }

    public void MostrarMensaje(string mensaje, float duracion = -1f)
    {
        if (duracion < 0f) duracion = duracionPorDefecto;

        StopAllCoroutines();

        if (mensajeText != null) mensajeText.text = mensaje;
        if (panel != null) panel.SetActive(true);

        StartCoroutine(OcultarDespues(duracion));
    }

    IEnumerator OcultarDespues(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        if (panel != null) panel.SetActive(false);
    }
}
