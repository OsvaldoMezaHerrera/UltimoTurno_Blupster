using UnityEngine;

public class VariableManager : MonoBehaviour
{
    public static VariableManager Instance;

    [Header("Variables del jugador")]
    public int confianza = 0;
    public int estres = 0;
    public int puntos = 0;

    [Header("UI")]
    public VariableBarUI variableBarUI;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ModificarVariables(int confianzaChange, int estresChange, int puntosChange)
    {
        confianza += confianzaChange;
        estres += estresChange;
        puntos += puntosChange;

        if (variableBarUI != null)
            variableBarUI.ActualizarBarras(confianza, estres);
    }
}