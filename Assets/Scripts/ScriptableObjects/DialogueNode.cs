using UnityEngine;

[CreateAssetMenu(fileName = "SO_DialogueNode", menuName = "Dialogue/Node")]
public class DialogueNode : ScriptableObject
{
    [Header("Texto del NPC")]
    [TextArea(2, 5)]
    public string npcText;

    [Header("Configuración de Tiempo")]
    public float tiempoLimite = 0f;
    
    [Header("Opciones del jugador")]
    public DialogueChoice[] choices;
}

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;           // Lo que ve el jugador en el botón

    [Header("Consecuencias")]
    public int confianzaChange;         // Puede ser positivo o negativo
    public int estresChange;
    public int puntosChange;

    [Header("Siguiente nodo")]
    public DialogueNode nextNode;       // null = fin de conversación
}