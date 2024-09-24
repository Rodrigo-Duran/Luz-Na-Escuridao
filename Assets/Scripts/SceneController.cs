using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // AUDIO
    [SerializeField] private AudioSource gameSoundEffects;
    [SerializeField] private AudioClip dogsBark;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player Interaction"))
        {
            gameSoundEffects.clip = dogsBark;
            gameSoundEffects.Play();
            SceneManager.LoadScene("GameWin");
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }

}
