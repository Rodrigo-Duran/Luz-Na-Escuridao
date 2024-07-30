using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    #region Variables
    // Referencias dos paineis usados na primeira cena
    [SerializeField] GameObject homePanel;
    [SerializeField] GameObject mainMenuPanel;
    #endregion

    #region MainMethods
    private void Update()
    {
        // Se o painel inicial tiver ativo 
        if (homePanel.activeInHierarchy)
        {
            // Se o player apertar qualquer tecla ou bot�o do mouse
            if (Input.anyKeyDown)
            {
                // Painel inicial � desativado
                homePanel.SetActive(false);
                // Painel de menu � ativado
                mainMenuPanel.SetActive(true);
            }
        }
    }
    #endregion

    #region ButtonsOnClickHandler
    // M�todo usado no OnClick do bot�o "Sair", para fechar a aplica��o
    public void QuitGame()
    {
        // Fechar a aplica��o
        Application.Quit();
    }

    // M�todo usado no OnClick do bot�o "Come�ar", para iniciar o jogo
    public void StartGame()
    {
        // Carregar a cena "Game"
        SceneManager.LoadScene("Game");
    }
    #endregion
}
