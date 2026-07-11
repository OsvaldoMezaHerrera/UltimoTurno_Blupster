using System;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    // Singleton: permite acceder desde cualquier script sin buscar el objeto
    public static ObjectiveManager Instance;

    [Header("Lista de objetivos")]
    public ObjectiveData[] objetivos;

    // Contadores
    private int objetivosEncontrados = 0;
    private int totalObjetivos = 0;

    // Estado del juego
    private bool juegoCompletado = false;

    public event Action<string> OnObjetivoEncontrado;
    public event Action OnJuegoCompletado;
    public event Action OnNuevaFase;

    void Awake()
    {
        // Configurar Singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Guardar total y resetear objetivos al iniciar
        totalObjetivos = objetivos.Length;
        ResetearObjetivos();
        Debug.Log("ObjectiveManager iniciado. Total objetivos: " + totalObjetivos);
    }

    // Se llama desde InteractionSystem cuando se encuentra un objeto
    public void RegistrarObjetivoEncontrado(string idObjeto)
    {
        foreach (ObjectiveData objetivo in objetivos)
        {
            if (objetivo.idObjeto == idObjeto && !objetivo.encontrado)
            {
                objetivo.encontrado = true;
                objetivosEncontrados++;

                Debug.Log("Encontrado: " + objetivo.nombreObjetivo +
                          " | Progreso: " + objetivosEncontrados + "/" + totalObjetivos);

                OnObjetivoEncontrado?.Invoke(idObjeto);

                if (objetivosEncontrados >= totalObjetivos)
                {
                    juegoCompletado = true;
                    Debug.Log("TODOS LOS OBJETIVOS COMPLETADOS!");
                    OnJuegoCompletado?.Invoke();
                }

                return;
            }
        }
    }

    // Resetea todos los objetivos para rejugar
    public void ResetearObjetivos()
    {
        objetivosEncontrados = 0;
        juegoCompletado = false;

        foreach (ObjectiveData objetivo in objetivos)
        {
            objetivo.encontrado = false;
        }

        Debug.Log("Objetivos reseteados");
    }

    // Carga una nueva fase con nuevos objetivos
    public void CargarNuevaFase(ObjectiveData[] nuevosObjetivos)
    {
        objetivos = nuevosObjetivos;
        // Contar solo objetivos no nulos
        totalObjetivos = 0;
        foreach (ObjectiveData obj in objetivos)
            if (obj != null) totalObjetivos++;
        objetivosEncontrados = 0;
        juegoCompletado = false;

        foreach (ObjectiveData obj in objetivos)
            if (obj != null) obj.encontrado = false;

        OnNuevaFase?.Invoke();
        Debug.Log("Nueva fase iniciada con " + totalObjetivos + " objetivos.");
    }

    // Metodos para que la UI los use
    public int GetObjetivosEncontrados() => objetivosEncontrados;
    public int GetTotalObjetivos() => totalObjetivos;
    public bool IsJuegoCompletado() => juegoCompletado;
    public ObjectiveData[] GetObjetivos() => objetivos;
}
