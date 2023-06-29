using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterProjectile : Projectile
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine("CollisionEffect");  
    }
}
