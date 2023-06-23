using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int speed;
    public int damage;
    public float hitTime;
    public ParticleSystem projectileHeadParticle;
    public GameObject projectileHit;
    public GameObject projectileParent;

    protected Rigidbody2D rb;
    protected WaitForSeconds hitParticleTime;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        WaitForSeconds hitParticleTime = new WaitForSeconds(hitTime);
    }

    protected IEnumerator CollisionEffect()
    {
        projectileHeadParticle.Stop();
        projectileHit.SetActive(true);
        yield return hitParticleTime;
        projectileHit.SetActive(false);
        projectileParent.SetActive(false);
    }

    public void ShootProjectile(int _dir)
    {
        rb.velocity = _dir * speed * Vector2.right;
    }

}