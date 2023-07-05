using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonPlantProjectile : Projectile
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            PlayerStatManager.instance.DamagePlayer(damage);
        StartCoroutine("CollisionEffect");
    }
  
}
