using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] List<GameObject> enemySprites;
    [SerializeField] GameObject enemyShadowSprite, enemyEyesSprite;

    [SerializeField] GameController gameController;

    NavMeshAgent agent;

    private bool canSeePlayer;
    public bool _canSeePlayer
    {
        get { return canSeePlayer; }
        set { canSeePlayer = value; }
    }
    private bool followingPlayer;
    public bool timeUp;
    public bool playerOutOfRange;

    [SerializeField] private PlayerController playerController;

    private Vector3 enemyStartPosition;

    // Audio
    [SerializeField] private AudioSource followingSource;
    [SerializeField] private AudioSource afterEyesSource;
    [SerializeField] private AudioSource enemySource;
    [SerializeField] private AudioSource gameSource;
    [SerializeField] private AudioClip enemyAttack, dettection;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        canSeePlayer = false;
        followingPlayer = false;
        timeUp = false;
        playerOutOfRange = true;

        foreach (GameObject sprite in enemySprites) sprite.SetActive(false);
        enemyShadowSprite.SetActive(false);
        enemyEyesSprite.SetActive(false);
        enemyStartPosition = transform.position;

    }

    private void Update()
    {
        if (canSeePlayer)
        {
            agent.SetDestination(target.position);
            followingPlayer = true;
        }

        //if ((!canSeePlayer) && a)
        //{
        //    a = false;
        //    StartCoroutine(Waiting());
        //}

        if (playerController._playerIsHidden && followingPlayer)
        {
            followingPlayer = false;
            StartCoroutine(Waiting());
        }

        if (canSeePlayer && !timeUp)
        {
            timeUp = true;
            StartCoroutine(FollowTimer());
        }

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("Short Range Flashlight") || collision.gameObject.CompareTag("Vision")) && !canSeePlayer && !timeUp)
        {
            Debug.Log("VENDO JOGADOR");
            canSeePlayer = true;
            // Audio deteccao
            gameSource.clip = dettection;
            gameSource.Play();
            //Tocar Audio perseguição
            followingSource.mute = false;
            
        }

        if (collision.gameObject.CompareTag("Long Range Flashlight"))
        {
            enemyShadowSprite.SetActive(true);
            enemyEyesSprite.SetActive(true);
            // Tocar audio após olhos
            afterEyesSource.mute = false;
        }

        if (collision.gameObject.CompareTag("Short Range Flashlight"))
        {
            // Todas as sprites
            foreach (GameObject sprite in enemySprites) sprite.SetActive(true);
        }

        if (collision.gameObject.CompareTag("Vision"))
        {
            foreach (GameObject sprite in enemySprites) sprite.SetActive(true);
            enemyShadowSprite.SetActive(true);
            enemyEyesSprite.SetActive(true);
        }

        if (collision.gameObject.tag == "Player")
        {
            // Tocar audio
            enemySource.clip = enemyAttack;
            enemySource.Play();
            gameController.GameOver();
           
        }
    }


    IEnumerator Waiting()
    {
        Debug.Log("Não vejo mais o player");
        canSeePlayer = false;
        //Mutar audio perseguicao
        followingSource.mute = true;
        //Mutar audio apos olhos
        afterEyesSource.mute = true;
        agent.SetDestination(transform.position);
        yield return new WaitForSeconds(3f);
        Debug.Log("Voltando pra posição inicial");
        agent.SetDestination(enemyStartPosition);
    }

    IEnumerator FollowTimer()
    {
        yield return new WaitForSeconds(5f);
        if (!playerOutOfRange)
        {
            StartCoroutine(Waiting());
        }
        timeUp = false;
    }
}
