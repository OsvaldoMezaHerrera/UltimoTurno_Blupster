using System.Collections;
using UnityEngine;

// Coloca este script en el objeto barrera de las escaleras.
// Se destruye automaticamente cuando se completan todos los objetivos.
public class BarreraFinal : MonoBehaviour
{
    [Header("Configuracion")]
    public float retardo = 1f; // segundos de espera antes de destruirse

    void Start()
    {
        if (ObjectiveManager.Instance != null)
            ObjectiveManager.Instance.OnJuegoCompletado += OnCompletado;
        else
            Debug.LogWarning("BarreraFinal: No se encontro ObjectiveManager en la escena.");
    }

    void OnDestroy()
    {
        if (ObjectiveManager.Instance != null)
            ObjectiveManager.Instance.OnJuegoCompletado -= OnCompletado;
    }

    void OnCompletado()
    {
        StartCoroutine(RomperBarrera());
    }

    IEnumerator RomperBarrera()
    {
        yield return new WaitForSeconds(retardo);

        // Animacion de caida antes de destruir
        float tiempo = 0f;
        Vector3 posInicial = transform.position;
        Quaternion rotInicial = transform.rotation;

        while (tiempo < 0.5f)
        {
            tiempo += Time.deltaTime;
            transform.position = posInicial + Vector3.down * (tiempo * 3f);
            transform.rotation = rotInicial * Quaternion.Euler(tiempo * 200f, 0f, 0f);
            yield return null;
        }

        Destroy(gameObject);
    }
}
