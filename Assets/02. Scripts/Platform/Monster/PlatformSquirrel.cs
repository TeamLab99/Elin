using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSquirrel : PlatformMonster
{
    [SerializeField] SpriteRenderer spr;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] GameObject apple;

    private void Update()
    {
        CheckNearPlayer();
        RunAway();
    }

    private void CheckNearPlayer()
    {
        playerHit = Physics2D.Raycast(recognitionPos.position, lookDir * Vector2.right, recognitionRange, playerLayer);
    }

    private void RunAway()
    {
        if (playerHit)
        {
            PlatformEventManager.instance.ControlPlayerMove(false);
            apple.SetActive(false);
            spr.flipX = true;
            rb.velocity = new Vector2(speed, rb.velocity.y);
            anim.SetBool("Walk", true);
            Invoke("Dissapear", 2f);
        }
    }

    private void Dissapear()
    {
        PlatformEventManager.instance.ControlPlayerMove(true);
        gameObject.SetActive(false);
    }    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(recognitionPos.position, lookDir * Vector2.right * recognitionRange);
    }
}
