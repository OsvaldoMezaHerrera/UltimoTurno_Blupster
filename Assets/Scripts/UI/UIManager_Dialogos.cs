using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager_Dialogos : MonoBehaviour
{
    public static UIManager_Dialogos Instance;

    [Header("Referencias UI")]
    public TextMeshProUGUI npcText;
    public Button[] choiceButtons;
    public TextMeshProUGUI[] choiceTexts;
    
    // Nueva variable para el texto del timer
    public TextMeshProUGUI timerText;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateUI(DialogueNode node)
    {
        npcText.text = node.npcText;

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < node.choices.Length)
            {
                choiceButtons[i].gameObject.SetActive(true);
                choiceTexts[i].text = node.choices[i].choiceText;

                int index = i; 
                choiceButtons[i].onClick.RemoveAllListeners();
                choiceButtons[i].onClick.AddListener(() =>
                    DialogueManager.Instance.SelectChoice(index));
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    // Nueva función para actualizar el reloj en pantalla
    public void ActualizarTimer(float tiempo)
    {
        // Convierte el número a formato de 1 decimal (ejemplo: 5.2)
        timerText.text = tiempo.ToString("F1") + "s";
    }

    // Función para mostrar o esconder el reloj
    public void MostrarTimer(bool mostrar)
    {
        timerText.gameObject.SetActive(mostrar);
    }
}