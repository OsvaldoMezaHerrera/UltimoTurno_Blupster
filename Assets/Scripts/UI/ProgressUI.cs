using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressUI : MonoBehaviour
{
    public static ProgressUI Instance;

    [Header("Configuración del ProgressPanel")]
    public Transform contenedorSegmentos;
    public GameObject prefabSegmentoBarra;

    private List<Image> listaBarritasUI = new List<Image>();

    void Awake()
    {
        Instance = this;
    }

    public void InicializarBarra(int totalPasos)
    {
        if (contenedorSegmentos == null || prefabSegmentoBarra == null) return;

        // Restablece la barra de progreso
        foreach (Transform hijo in contenedorSegmentos) Destroy(hijo.gameObject);
        listaBarritasUI.Clear();

        for (int i = 0; i < totalPasos; i++)
        {
            GameObject nuevoSegmento = Instantiate(prefabSegmentoBarra, contenedorSegmentos);
            Image imagenBarrita = nuevoSegmento.GetComponent<Image>();
            if (imagenBarrita != null)
            {
                imagenBarrita.fillAmount = 1f; 
                imagenBarrita.color = Color.gray; 
                listaBarritasUI.Add(imagenBarrita);
            }
        }

        // Iluminamos el paso 0 de inmediato
        ActualizarBarraVisual(0);
    }

    public void ActualizarBarraVisual(int pasoActual)
    {
        for (int i = 0; i < listaBarritasUI.Count; i++)
        {
            if (listaBarritasUI[i] != null)
            {
                if (i < pasoActual)
                {
                    // Pasos ya completados = Verde
                    listaBarritasUI[i].color = Color.green; 
                }
                else if (i == pasoActual)
                {
                    // Paso en el que estamos = Amarillo
                    listaBarritasUI[i].color = Color.yellow; 
                }
                else
                {
                    // Pasos que aún no hacemos = Gris
                    listaBarritasUI[i].color = Color.gray; 
                }
            }
        }
    }
}