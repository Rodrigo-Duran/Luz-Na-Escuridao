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
            // Se o player apertar qualquer tecla ou botão do mouse
            if (Input.anyKeyDown)
            {
                // Painel inicial é desativado
                homePanel.SetActive(false);
                // Painel de menu é ativado
                mainMenuPanel.SetActive(true);
            }
        }
    }
    #endregion

    #region ButtonsOnClickHandler
    // Método usado no OnClick do botão "Sair", para fechar a aplicação
    public void QuitGame()
    {
        // Fechar a aplicação
        Application.Quit();
    }

    // Método usado no OnClick do botão "Começar", para iniciar o jogo
    public void StartGame()
    {
        // Carregar a cena "Game"
        SceneManager.LoadScene("Game");
    }
    #endregion
}
