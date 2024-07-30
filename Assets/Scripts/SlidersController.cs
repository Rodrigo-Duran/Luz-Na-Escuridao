using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlidersController : MonoBehaviour
{
    // Referencia do slider do pr�prio objeto
    [SerializeField] Slider slider;

    // M�todo para atualizar o valor do slider, recebendo o valor atual e o valor m�ximo
    public void UpdateSliderValue(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }
}
