using UnityEngine;

public class IntroManager_Dialogos : MonoBehaviour
{
    [Header("Scripts a congelar")]
    // Al poner el nombre exacto, obligamos a Unity a no equivocarse de componente
    public PlayerController playerController;
    public InteractionSystem interactionSystem;

    void Start()
    {
        // Al iniciar, apagamos el movimiento y la interacción
        if (playerController != null) playerController.enabled = false;
        if (interactionSystem != null) interactionSystem.enabled = false;
    }

    void Update()
    {
        // Detectamos si presiona la barra espaciadora
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Volvemos a prender el movimiento y la interacción
            if (playerController != null) playerController.enabled = true;
            if (interactionSystem != null) interactionSystem.enabled = true;

            // Apagamos la pantalla negra
            gameObject.SetActive(false);
        }
    }
}