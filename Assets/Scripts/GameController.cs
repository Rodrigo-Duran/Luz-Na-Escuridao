using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    [SerializeField] private PlayerController playerController;

    // Lanterna
    [SerializeField] private SlidersController flashlightBatterySliderController;
    [SerializeField] private FlashLightLevelControl flashlightLevelController;
    [SerializeField] private float flashlightTotalBattery = 400f;
    [SerializeField] private float flashlightActualBattery;

    //Sprint
    [SerializeField] private SlidersController sprintSliderController;
    [SerializeField] private float totalSprintValue = 500f;
    [SerializeField] private float actualSprintValue;
    private bool canIncreaseSprint;
    private bool canRebootSprint;
    private bool canRun;
    public bool _canRun 
    { 
        get { return canRun; }
        set { canRun = value; }
    }

    public static GameController Instance;

    [SerializeField] private float batteryPackIncrease = 80f;

    //Teste
    [SerializeField] private Image HUDImage;

    private bool canDecreaseFlashlightBattery;

    public static bool gameIsPaused;

    //[SerializeField] GameObject hintImage;

    [SerializeField] GameObject pausePanel;

    // Audio
    [SerializeField] private List<AudioSource> audioSourceList;
    //[SerializeField] private AudioSource gameSource;
    [SerializeField] private AudioClip gameOverClip;

    // Awake
    void Awake()
    {
        Instance = this;

        flashlightActualBattery = flashlightTotalBattery;
        actualSprintValue = totalSprintValue;
        canIncreaseSprint = true;
        canRebootSprint = false;
        canRun = true;
        canDecreaseFlashlightBattery = true;
        gameIsPaused = false;
        StartCoroutine(TwelveSecondsAudioStart());
    }

    // Update is called once per frame
    void Update()
    {
        // LANTERNA
        // Atualiza slider de bateria da lanterna
        flashlightBatterySliderController.UpdateSliderValue(flashlightActualBattery, flashlightTotalBattery);
        // Altera o range da lanterna de acordo com a bateria atual
        if (flashlightLevelController._flashLightIsActive)
        {
            // Se a bateria estiver em 0, o nível de bateria é setado pra 0
            if (flashlightActualBattery == 0)
            {
                flashlightLevelController._lightLevel = 0;
                HUDImage.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }

            // Se a bateria estiver abaixo de 20, o nível de bateria é setado pra 1
            else if (flashlightActualBattery < 100)
            {
                flashlightLevelController._lightLevel = 1;
                HUDImage.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }

            // Se a bateria estiver abaixo de 60, o nível de bateria é setado pra 2
            else if (flashlightActualBattery < 280)
            {
                flashlightLevelController._lightLevel = 2;
                HUDImage.transform.localScale = new Vector3(0.65f, 0.65f, 0.65f);
            }

            // Se a bateria estiver abaixo ou igual a 100, o nível de bateria é setado pra 3
            else if (flashlightActualBattery <= 400)
            {
                flashlightLevelController._lightLevel = 3;
                HUDImage.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            }
        }

        // Diminui com o tempo o valor da bateria se a lanterna estiver ligada
        if (flashlightLevelController._flashLightIsActive && canDecreaseFlashlightBattery)
        {
            // Travar o método para não ficar em loop
            canDecreaseFlashlightBattery = false;
            // Executa o método de diminuir a bateria
            StartCoroutine(DecreaseBattery());
        }

        //---------------------------------------------------------------------------------------

        // SPRINT

        // Se o player estiver correndo
        if (playerController._playerIsRunning)
        {
            // Chamar o método para diminuir o valor da sprint
            DecreaseSprint();
        }
        // Se o player não estiver correndo correr e o valor da sprint atual estiver maior que 0
        else if ((!playerController._playerIsRunning) && (actualSprintValue > 0 && actualSprintValue < 500) && canIncreaseSprint)
        {
            // Travar o método para não ficar em loop
            canIncreaseSprint = false;
            // Chamar o método para aumentar o valor da sprint
            StartCoroutine(IncreaseSprint());
        }

        // Se o sprint chegar a 0
        if (actualSprintValue == 0 && canRun)
        {
            // Travar o método para não ficar em loop
            canRun = false;
            // Começar o Timer para liberar o Increase
            StartCoroutine(IncreaseSprintTimer());
        }

        // Se ele não puder correr, ou seja, o valor da sprint atingir 0, e o timer ja tiver terminado
        if (!canRun && canRebootSprint && canIncreaseSprint)
        {
            // Travar o método para não ficar em loop
            canIncreaseSprint = false;
            // Chamar o método para aumentar o valor da sprint
            StartCoroutine(IncreaseSprint());
        }

        //-------------------------------------------------------------------------------------------
        // GAME
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }

        /*if (hintImage.activeInHierarchy && Input.anyKeyDown)
        {
            hintImage.SetActive(false);
        }*/
        
        
    }

    // Método para diminuir bateria da lanterna
    IEnumerator DecreaseBattery()
    {
        // Espera meio segundo
        yield return new WaitForSeconds(0.5f);
        // Se a bateria estiver maior que 0, diminui o valor em 1
        if(flashlightActualBattery > 0) flashlightActualBattery--;
        // Atualiza o Slider da bateria visualmente
        //flashlightBatterySliderController.UpdateSliderValue(flashlightActualBattery, flashlightTotalBattery);
        // Libera o código para refazer esse método
        canDecreaseFlashlightBattery = true;
    }

    // Método para diminuir o valor da sprint
    void DecreaseSprint()
    {
        // Diminui 1 no valor atual da sprint
        actualSprintValue --;
        // Atualiza o slider na UI
        sprintSliderController.UpdateSliderValue(actualSprintValue, totalSprintValue);
    }

    // Método para aumentar o valor da sprint
    IEnumerator IncreaseSprint()
    {
        // Espera 0.01 segundos
        yield return new WaitForSeconds(0.01f);
        // Se o valor atual da Sprint for menor que o valor total, aumenta o valor em 1
        if(actualSprintValue < totalSprintValue) actualSprintValue++;
        // Atualiza o slider na UI
        sprintSliderController.UpdateSliderValue(actualSprintValue, totalSprintValue);
        // IF usado para quando o valor atual da sprint atingir 0 (CanRebootSprint)
        // Se o valor atual da sprint estiver maior ou igual a 150
        if(canRebootSprint && actualSprintValue >= 150)
        {
            // Trava o IF, para não ser chamado em loop junto com o método
            canRebootSprint = false;
            // Libera o player para correr
            canRun = true;
        }
        // Libera os IF's que estão chamando o método de chamar novamente
        canIncreaseSprint = true;
    }

    // Método criado para ser um Timer, para quando o player alcançar 0 de sprint, esperar 3 segundos até começar a aumentar a sprint novamente
    IEnumerator IncreaseSprintTimer()
    {
        // Timer de 3 segundos
        yield return new WaitForSeconds(3f);
        // Libera o IF para começar a aumentar o valor atual da sprint
        canRebootSprint = true;
    }

    public void IncreaseBatteryLevel()
    {
        if((flashlightTotalBattery - flashlightActualBattery) < batteryPackIncrease)
        {
            flashlightActualBattery = 400;
            //flashlightBatterySliderController.UpdateSliderValue(flashlightActualBattery, flashlightTotalBattery);
        }

        else if ((flashlightTotalBattery - flashlightActualBattery) >= batteryPackIncrease)
        {
            flashlightActualBattery += batteryPackIncrease;
            //flashlightBatterySliderController.UpdateSliderValue(flashlightActualBattery, flashlightTotalBattery);
        }
    }

    #region GameHandler
    public void PauseGame()
    {
        if (!gameIsPaused)
        {
            //foreach (AudioSource audio in audioSourceList) audio.Stop(); // Tem que pensar um modo para parar todos os sons que estão ativos
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
            gameIsPaused = true;
        }
        else
        {
            Time.timeScale = 1f;
            pausePanel.SetActive(false);
            gameIsPaused = false;
            //foreach (AudioSource audio in audioSourceList) audio.Play(); // Tem que pensar um modo para retomar apenas os sons que estavam ativos
        }
    }

    public void GameOver()
    {
        foreach (AudioSource audio in audioSourceList) audio.Stop();
        audioSourceList[6].clip = gameOverClip;
        audioSourceList[6].Play();
        SceneManager.LoadScene("GameOver");
        Time.timeScale = 0f;
        gameIsPaused = true;
        //StartCoroutine(PlayGameOverClip());
    }

    #endregion

    #region AudioHandler

    IEnumerator TwelveSecondsAudioStart()
    {
        Debug.Log("INICIOU JOGO");
        yield return new WaitForSecondsRealtime(12f);
        Debug.Log("AUDIO INICIADO");
        audioSourceList[1].mute = false;
    }

    IEnumerator PlayGameOverClip()
    {
        yield return new WaitForSeconds(1f);
        audioSourceList[6].clip = gameOverClip;
        audioSourceList[6].Play();
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    #endregion
}
