using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FlashLightLevelControl : MonoBehaviour
{
    #region Variables
    // Referencias dos objetos de niveis de luz
    [SerializeField] GameObject lowLight, mediumLight, highLight;
    [SerializeField] GameObject darkVision;

    // Variavel para saber o nivel da luz atual
    private int lightLevel;
    public int _lightLevel
    {
        get { return lightLevel; }
        set { lightLevel = value; }
    }

    // Variavel para saber o nivel da luz atual
    private bool flashLightIsActive;
    public bool _flashLightIsActive
    {
        get { return flashLightIsActive; }
        set { flashLightIsActive = value; }
    }

    // Variavel para saber se pode interagir com a lanterna
    private bool canInteractWithFlashlight;
    public bool _canInteractWithFlashlight
    {
        get { return canInteractWithFlashlight; }
        set { canInteractWithFlashlight = value; }
    }

    // AUDIO
    [SerializeField] private AudioSource playerSourceUnique;
    [SerializeField] private AudioClip turnOn, turnOff;

    // HUD
    [SerializeField] private GameObject flashlightOn, flashlightOff;

    #endregion

    #region MainMethods
    private void Awake()
    {
        // Nivel da luz começa como 0
        lightLevel = 0;
        // Lanterna esta desligada
        flashLightIsActive = false;
        // Pode interagir com a lanterna
        canInteractWithFlashlight = true;
        // Desativando todos os niveis de luz para evitar bugs
        lowLight.SetActive(false);
        mediumLight.SetActive(false);
        highLight.SetActive(false);
    }

    private void Update()
    {
        //DESABILITADO POR QUESTÃO DE USAR OUTRA MECANICA
        /*
        if (Input.GetButtonDown("Fire1") && lightLevel >= 3)
        {
            lightLevel = 0;
            FlashLightLevel();
        }

        else if (Input.GetButtonDown("Fire1") && lightLevel < 3)
        {
            lightLevel++;
            FlashLightLevel();
        }*/

        
        if (!GameController.gameIsPaused)
        {
            // Se o player apertar o botão esquerdo do mouse e puder interagir com a lanterna
            if (Input.GetMouseButtonDown(0) && canInteractWithFlashlight)
            {
                // Se a lanterna estiver ligada
                if (flashLightIsActive)
                {
                    Debug.Log("DESLIGAR LANTERNA");
                    // Desligar a lanterna
                    flashLightIsActive = false;
                    // Nivel de luz setado para 0
                    lightLevel = 0;
                    //DarkVision ativa
                    darkVision.SetActive(true);
                    // Tocar audio
                    playerSourceUnique.clip = turnOff;
                    playerSourceUnique.Play();
                    //Alterar a imagem da lanterna na HUD (Desligada)
                    flashlightOn.SetActive(false);
                    flashlightOff.SetActive(true);
                }
                // Se a lanterna estiver desligada
                else
                {
                    Debug.Log("LIGAR LANTERNA");
                    // Ligar a lanterna
                    flashLightIsActive = true;
                    //DarkVision ativa
                    darkVision.SetActive(false);
                    // Tocar audio
                    playerSourceUnique.clip = turnOn;
                    playerSourceUnique.Play();
                    //Alterar a imagem da lanterna na HUD (Ligada)
                    flashlightOff.SetActive(false);
                    flashlightOn.SetActive(true);
                }
            }
        }

        // Checando a todo momento qual o nível atual da luz
        FlashLightLevel();
    }
    #endregion

    #region FlashlightLevelHandler
    // Método para checar o nível atual da luz
    private void FlashLightLevel()
    {
        switch (lightLevel)
        {
            // Se estiver desligada ou sem bateria
            case 0:
                // Desativar todos os niveis de luz
                lowLight.SetActive(false);
                mediumLight.SetActive(false);
                highLight.SetActive(false);
                break;
            // Se estiver com luz baixa
            case 1:
                // Ativar apenas o nível baixo de luz
                lowLight.SetActive(true);
                mediumLight.SetActive(false);
                highLight.SetActive(false);
                break;
            // Se estiver com luz média
            case 2:
                // Ativar apenas o nível médio de luz
                lowLight.SetActive(false);
                mediumLight.SetActive(true);
                highLight.SetActive(false);
                break;
            // Se estiver com luz alta
            case 3:
                // Ativar apenas o nível alto de luz
                lowLight.SetActive(false);
                mediumLight.SetActive(false);
                highLight.SetActive(true);
                break;
        }
    }
    #endregion
}
