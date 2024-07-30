using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryPack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.CompareTag("Vision"))
        //{
        //    Physics2D.IgnoreCollision(gameObject.GetComponent<CapsuleCollider2D>(), collision.gameObject.GetComponent<CircleCollider2D>());
        //}

        if (collision.gameObject.CompareTag("Player Interaction"))
        {
            GameController.Instance.IncreaseBatteryLevel();
            Destroy(this.gameObject);
        }
    }
}
