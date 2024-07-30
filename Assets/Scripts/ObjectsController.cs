using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsController : MonoBehaviour
{
    private PolygonCollider2D polygonCollider;
    private Transform player;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    
    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if(collision.gameObject.tag == "Player Interaction")
        {
            //Debug.Log("EM CONTATO");
            if (player.position.y < transform.position.y)
            {
                spriteRenderer.sortingOrder = 0;
            }
            else
            {
                spriteRenderer.sortingOrder = 17;
            }
        }
    }
}
