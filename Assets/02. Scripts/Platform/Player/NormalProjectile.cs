using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalProjectile : Projectile
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine("CollisionEffect");
    }
}
