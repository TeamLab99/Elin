using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPoisonPlant : PlatformMonster
{
    [SerializeField] ParticleSystem[] attackParticle;
    bool isAttacking=false;

    private void Update()
    {
        CheckNearPlayer();
    }

    private void CheckNearPlayer()
    {
        findPlayer = Physics2D.OverlapBox(recognitionPos.position, recognitionRange, 0f, playerLayer);
        if (findPlayer && !isAttacking)
        {
            anim.SetTrigger("Attack");
            isAttacking = true;
        }
    }

    public void AttackPlayer()
    {
        attackParticle[0].Play();
        attackParticle[1].Play();
        isAttacking = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer==18)
            Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(recognitionPos.position, recognitionRange);
    }
}
