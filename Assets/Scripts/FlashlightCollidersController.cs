using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightCollidersController : MonoBehaviour
{
    #region Variables
    // Referencia ao colisor do pr�prio objeto, dependendo do objeto � um poligono ou circulo
    [SerializeField] private PolygonCollider2D polygonCollider;
    [SerializeField] private CircleCollider2D circleCollider;
    // Referencia ao script do player
    private PlayerController playerController;
    #endregion

    #region MainMethods
    private void Awake()
    {
        // Encontrando o objeto com a tag Player e pegando o seu script
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    #endregion

    #region CollisionHandler
    // M�todo para verificar se houve uma colis�o 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Se a colis�o for com um objeto de tag "Moita"
        if (collision.gameObject.tag == "Bush" && !playerController._playerIsHidden)
        {
            Debug.Log("PODE SE ESCONDER NA MOITA");
            // Player pode se esconder
            playerController._playerCanHide = true;
            // Passando o objeto moita para o player ter como referencia
            playerController._bushToHide = collision.gameObject;
        }

        /*if (collision.gameObject.tag == "Placa")
        {
            playerController._playerCanInteract = true;
            playerController._hintContent = collision.gameObject.GetComponent<HintsController>();
        }*/
    }

    // M�todo para verificar se houve um final de colis�o
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Se a colis�o for com um objeto de tag "Moita"
        if (collision.gameObject.tag == "Bush")
        {
            Debug.Log("N�O PODE SE ESCONDER NA MOITA MAIS");
            // Player n�o pode se esconder
            playerController._playerCanHide = false;
            // Objeto passado como null, para receber um outro numa proxima oportunidade
            //playerController._bushToHide = null;
        }

       /* if (collision.gameObject.tag == "Placa")
        {
            playerController._playerCanInteract = false;
        }*/
    }
    #endregion
}
