using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VariableBarUI : MonoBehaviour
{
    [Header("Barra de Confianza")]
    public Slider confianzaSlider;
    public TextMeshProUGUI confianzaLabel;

    [Header("Barra de Estrés")]
    public Slider estresSlider;
    public TextMeshProUGUI estresLabel;

    public void ActualizarBarras(int confianza, int estres)
    {
        confianzaSlider.value = confianza;
        estresSlider.value = estres;

        confianzaLabel.text = "Confianza: " + confianza;
        estresLabel.text = "Estrés: " + estres;
    }
}