using UnityEngine;

public class InteractionSystem_Dialogos : MonoBehaviour
{
    public float interactRange = 3f; 
    public Transform playerCamera;   

    [Header("Feedback Visual")]
    public GameObject mensajeInteraccion;

    // Solo recordamos a qué NPC miramos, ya no intentamos memorizar el color
    private GameObject npcMiradoActualmente = null;

    void Update()
    {
        // Si el diálogo está abierto, no hacemos nada
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueOpen())
            return;

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hitInfo;
        bool mirandoAUnNPC = false; 

        if (Physics.Raycast(ray, out hitInfo, interactRange))
        {
            if (hitInfo.collider.CompareTag("NPC"))
            {
                mirandoAUnNPC = true;
                GameObject npcDetectado = hitInfo.collider.gameObject;

                if (npcMiradoActualmente != npcDetectado)
                {
                    ApagarFeedback(); 
                    npcMiradoActualmente = npcDetectado;
                    EncenderFeedback();
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    NPCDialogue npcScript = hitInfo.collider.GetComponent<NPCDialogue>();
                    if (npcScript != null && npcScript.nodoInicial != null)
                    {
                        DialogueManager.Instance.StartDialogue(npcScript.nodoInicial);
                        ApagarFeedback(); 
                    }
                }
            }
        }

        // Si quitamos la vista del NPC, apagamos todo
        if (!mirandoAUnNPC && npcMiradoActualmente != null)
        {
            ApagarFeedback();
        }
    }

    void EncenderFeedback()
    {
        if (mensajeInteraccion != null)
            mensajeInteraccion.SetActive(true);

        Renderer render = npcMiradoActualmente.GetComponent<Renderer>();
        if (render != null)
        {
            // Forzamos el color a amarillo
            render.material.color = Color.yellow;
        }
    }

    void ApagarFeedback()
    {
        if (mensajeInteraccion != null)
            mensajeInteraccion.SetActive(false);

        if (npcMiradoActualmente != null)
        {
            Renderer render = npcMiradoActualmente.GetComponent<Renderer>();
            if (render != null)
            {
                // Forzamos el color a blanco (el color base del material en Unity)
                render.material.color = Color.white; 
            }
            npcMiradoActualmente = null; 
        }
    }
}