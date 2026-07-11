using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("Referencia al panel de UI")]
    public GameObject dialoguePanel;

    private DialogueNode currentNode;
    private bool isDialogueOpen = false;

    // Variables nuevas para el timer
    private float tiempoRestante;
    private bool timerActivo = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // El Update lee el tiempo cada fotograma
    void Update()
    {
        if (isDialogueOpen && timerActivo)
        {
            tiempoRestante -= Time.deltaTime; // Restar el tiempo real
            UIManager_Dialogos.Instance.ActualizarTimer(tiempoRestante);

            // Si el tiempo se acaba
            if (tiempoRestante <= 0)
            {
                tiempoRestante = 0;
                timerActivo = false;
                Debug.Log("¡Se te acabó el tiempo para responder!");
                
                // Aquí cerramos el diálogo por no responder rápido
                // (Opcionalmente, aquí podrías restarle -10 de confianza al jugador)
                EndDialogue(); 
            }
        }
    }

    public void StartDialogue(DialogueNode startNode)
    {
        isDialogueOpen = true;
        dialoguePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        LoadNode(startNode);
    }

    public void LoadNode(DialogueNode node)
    {
        currentNode = node;
        UIManager_Dialogos.Instance.UpdateUI(node);

        // Verificamos si este nodo en específico tiene un límite de tiempo
        if (node.tiempoLimite > 0)
        {
            tiempoRestante = node.tiempoLimite;
            timerActivo = true;
            UIManager_Dialogos.Instance.MostrarTimer(true);
        }
        else
        {
            // Si el tiempo es 0, apagamos el reloj
            timerActivo = false;
            UIManager_Dialogos.Instance.MostrarTimer(false);
        }
    }

    public void SelectChoice(int choiceIndex)
    {
        // Al elegir una opción, detenemos el timer inmediatamente
        timerActivo = false; 

        DialogueChoice choice = currentNode.choices[choiceIndex];

        VariableManager.Instance.ModificarVariables(
            choice.confianzaChange,
            choice.estresChange,
            choice.puntosChange
        );

        if (choice.nextNode != null)
        {
            LoadNode(choice.nextNode);
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        isDialogueOpen = false;
        timerActivo = false;
        dialoguePanel.SetActive(false);
        currentNode = null;
        
        // ¡Esta es la línea clave que apaga el reloj al terminar!
        UIManager_Dialogos.Instance.MostrarTimer(false);
        
        // Ocultar y bloquear el ratón de nuevo
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public bool IsDialogueOpen() => isDialogueOpen;
}