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
        FindPlayer();
    }

    private void CheckNearPlayer()
    {
        playerHit = Physics2D.Raycast(recognitionPos.position, lookDir * Vector2.right, recognitionRange, playerLayer);
    }

    private void FindPlayer()
    {
        if (playerHit)
        {
            Managers.Input.PlayerMoveControl(false);
            findParticle.Play();
            Invoke("RunAway", 1f);
        }
    }

    private void RunAway()
    {
        apple.SetActive(false);
        spr.flipX = true;
        rb.velocity = new Vector2(speed, rb.velocity.y);
        anim.SetBool("Walk", true);
        Invoke("Dissapear", 2f);
    }

    private void  Dissapear()
    {
        Managers.Input.PlayerMoveControl(true);
        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(recognitionPos.position, lookDir * Vector2.right * recognitionRange);
    }
}
