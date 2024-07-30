using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    #region Variables
    // Referencia a camera da cena
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameController gameController;
    // Variavel para pegar a posição do mouse
    private Vector3 mousePosition;
    #endregion

    #region MainMethods
    // Update
    void Update()
    {
        if (!GameController.gameIsPaused) 
        {
            // Pegando a posição do mouse através do input do mouse na camera
            mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            // Variável que pega a diferença de rotação do mouse pro objeto
            Vector3 rotation = mousePosition - transform.position;

            // Variável que transforma a rotação em graus
            float rotationInZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

            // Colocando essa rotação no objeto, fazendo com que ele siga a rotação do mouse
            transform.rotation = Quaternion.Euler(0, 0, rotationInZ);
        }
    }
    #endregion
}
