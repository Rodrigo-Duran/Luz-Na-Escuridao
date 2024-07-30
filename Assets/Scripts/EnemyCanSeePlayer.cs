using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCanSeePlayer : MonoBehaviour
{
    [SerializeField] private EnemyBehavior enemyBehavior;
    [SerializeField] private CircleCollider2D enemyVision;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player Interaction"))
        {
            Debug.Log("Não vê jogador");
            enemyBehavior.playerOutOfRange = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player Interaction"))
        {
            Debug.Log("JOGADOR NO RANGE DO INIMIGO");
            enemyBehavior.playerOutOfRange = true;
        }
    }

    void Update()
    {
    }
}
