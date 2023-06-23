using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalProjectile : Projectile
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("WaterCrystal"))
            Debug.Log("물 속성 크리스탈 충전!");
        StartCoroutine("CollisionEffect");
    }
}
