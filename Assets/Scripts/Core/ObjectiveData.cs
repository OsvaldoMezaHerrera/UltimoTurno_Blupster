using UnityEngine;

// ScriptableObject que define cada objetivo a encontrar
[CreateAssetMenu(fileName = "ObjetivoData", menuName = "Exploracion/Objetivo")]
public class ObjectiveData : ScriptableObject
{
    [Header("Información del objetivo")]
    public string nombreObjetivo;       // Nombre que verá el jugador
    public string descripcion;          // Descripción opcional

    [Header("Identificación")]
    public string idObjeto;             // Debe coincidir con el nombre del GameObject en escena

    [HideInInspector]
    public bool encontrado = false;     // Se activa cuando el jugador lo encuentra
}