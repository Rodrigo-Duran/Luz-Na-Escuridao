using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    #region Variables
    // Referencia a camera da cena
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameController gameController;
    // Variavel para pegar a posi��o do mouse
    private Vector3 mousePosition;
    #endregion

    #region MainMethods
    // Update
    void Update()
    {
        if (!GameController.gameIsPaused) 
        {
            // Pegando a posi��o do mouse atrav�s do input do mouse na camera
            mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            // Vari�vel que pega a diferen�a de rota��o do mouse pro objeto
            Vector3 rotation = mousePosition - transform.position;

            // Vari�vel que transforma a rota��o em graus
            float rotationInZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

            // Colocando essa rota��o no objeto, fazendo com que ele siga a rota��o do mouse
            transform.rotation = Quaternion.Euler(0, 0, rotationInZ);
        }
    }
    #endregion
}
