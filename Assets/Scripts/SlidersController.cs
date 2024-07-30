using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlidersController : MonoBehaviour
{
    // Referencia do slider do próprio objeto
    [SerializeField] Slider slider;

    // Método para atualizar o valor do slider, recebendo o valor atual e o valor máximo
    public void UpdateSliderValue(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }
}
