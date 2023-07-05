using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPoisonPlant : PlatformMonster
{
    [SerializeField] ParticleSystem attackEffectParticle;
    GameObject attackProjectile;
    bool isAttacking=false;

    private void Start()
    {
        attackEffectParticle.GetComponent<ParticleSystemRenderer>().flip=new Vector3(lookDir, 0, 0);
    }

    private void Update()
    {
        CheckNearPlayer();
        FindPlayer();
    }

    private void CheckNearPlayer()
    {
        playerHit = Physics2D.Raycast(recognitionPos.position, lookDir*Vector2.right, recognitionRange, playerLayer);
    }

    private void FindPlayer()
    {
        if (playerHit && !isAttacking && !PlayerStatManager.instance.playerDead)
        {
            anim.SetTrigger("Attack");
            isAttacking = true;
        }
    }

    public void AttackPlayer()
    {
        attackEffectParticle.Play();
        attackProjectile = PlayerPoolManager.instance.GetMonsterAttack(0);
        attackProjectile.transform.position = recognitionPos.position;
        attackProjectile.GetComponent<Projectile>().ShootProjectile(lookDir);
    }

    public void CoolDownAttack()
    {
        isAttacking = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(recognitionPos.position, lookDir * Vector2.right * recognitionRange);
    }
}
