using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintsController : MonoBehaviour
{
    [SerializeField] public string content;
    private PlayerController playerController;

    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Short Range Flashlight" || collision.gameObject.tag == "Long Range Flashlight" || collision.gameObject.tag == "Vision")
        {
            playerController._playerCanInteract = true;
            playerController._hintContent = content;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Short Range Flashlight" || collision.gameObject.tag == "Long Range Flashlight" || collision.gameObject.tag == "Vision")
        {
            playerController._playerCanInteract = false;
        }
    }
}
