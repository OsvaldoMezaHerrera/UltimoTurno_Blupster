using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveUI : MonoBehaviour
{
    [Header("Referencias UI")]
    public TextMeshProUGUI progressText;
    public Transform objectiveList;

    private readonly Dictionary<string, TextMeshProUGUI> itemsUI = new Dictionary<string, TextMeshProUGUI>();

    void Start()
    {
        if (ObjectiveManager.Instance == null)
        {
            Debug.LogWarning("ObjectiveUI: ObjectiveManager no encontrado en escena");
            return;
        }

        ObjectiveManager.Instance.OnObjetivoEncontrado += ActualizarObjetivo;
        ObjectiveManager.Instance.OnJuegoCompletado += MostrarCompletado;
        ObjectiveManager.Instance.OnNuevaFase += ReiniciarLista;

        InicializarLista();
    }

    void OnDestroy()
    {
        if (ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.OnObjetivoEncontrado -= ActualizarObjetivo;
            ObjectiveManager.Instance.OnJuegoCompletado -= MostrarCompletado;
            ObjectiveManager.Instance.OnNuevaFase -= ReiniciarLista;
        }
    }

    void InicializarLista()
    {
        if (objectiveList == null) return;

        // Obtener fuente de progressText para asignarla a los items nuevos
        TMP_FontAsset fuente = progressText != null ? progressText.font : null;

        foreach (ObjectiveData objetivo in ObjectiveManager.Instance.GetObjetivos())
        {
            if (objetivo == null) continue;

            GameObject item = new GameObject(objetivo.idObjeto + "_item");
            item.transform.SetParent(objectiveList, false);

            TextMeshProUGUI texto = item.AddComponent<TextMeshProUGUI>();
            if (fuente != null) texto.font = fuente;
            texto.text = "○ " + objetivo.nombreObjetivo;
            texto.fontSize = 22;
            texto.color = Color.white;

            RectTransform rt = item.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(0f, 28f);

            itemsUI[objetivo.idObjeto] = texto;
            Debug.Log("[ObjectiveUI] Item creado: " + objetivo.nombreObjetivo);
        }

        ActualizarProgreso();
    }

    void ActualizarObjetivo(string idObjeto)
    {
        if (itemsUI.TryGetValue(idObjeto, out TextMeshProUGUI texto))
        {
            texto.text = "✓ " + ObtenerNombre(idObjeto);
            texto.color = new Color(0.45f, 1f, 0.45f);
        }

        ActualizarProgreso();

        if (FeedbackPanel.Instance != null)
            FeedbackPanel.Instance.MostrarMensaje("¡Objeto encontrado!");
    }

    void MostrarCompletado()
    {
        if (FeedbackPanel.Instance != null)
            FeedbackPanel.Instance.MostrarMensaje("¡Todos los objetivos completados!", 4f);

        StartCoroutine(OcultarChecklist());
    }

    void ReiniciarLista()
    {
        StartCoroutine(ReiniciarListaCoroutine());
    }

    IEnumerator ReiniciarListaCoroutine()
    {
        // Limpiar items anteriores
        foreach (Transform hijo in objectiveList)
            Destroy(hijo.gameObject);
        itemsUI.Clear();

        // Esperar un frame para que Destroy se complete
        yield return null;

        // Mostrar el panel si estaba oculto
        if (objectiveList != null)
            objectiveList.parent.gameObject.SetActive(true);

        // Crear nuevos items
        InicializarLista();

        // Forzar recalculo del layout
        yield return null;
        if (objectiveList != null)
            LayoutRebuilder.ForceRebuildLayoutImmediate(objectiveList.parent.GetComponent<RectTransform>());

        Debug.Log("[ObjectiveUI] Lista reiniciada con " + ObjectiveManager.Instance.GetTotalObjetivos() + " objetivos.");
    }

    IEnumerator OcultarChecklist()
    {
        yield return new WaitForSeconds(2f);

        // Ocultar el panel del checklist (padre de objectiveList)
        if (objectiveList != null)
            objectiveList.parent.gameObject.SetActive(false);
    }

    void ActualizarProgreso()
    {
        if (progressText == null) return;
        progressText.text = ObjectiveManager.Instance.GetObjetivosEncontrados()
                            + "/" + ObjectiveManager.Instance.GetTotalObjetivos();
    }

    string ObtenerNombre(string idObjeto)
    {
        foreach (ObjectiveData obj in ObjectiveManager.Instance.GetObjetivos())
            if (obj.idObjeto == idObjeto) return obj.nombreObjetivo;
        return idObjeto;
    }
}
