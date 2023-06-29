using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int speed;
    public int damage;
    public GameObject projectileHead;
    public GameObject projectileHit;
    public GameObject projectileParent;
    public EProjectileType projectileType;

    protected Rigidbody2D rb;
    protected WaitForSeconds hitParticleTime= new WaitForSeconds(0.5f);
    protected Collider2D collide;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collide = GetComponent<Collider2D>();
    }

    protected IEnumerator CollisionEffect()
    {
        rb.velocity = Vector2.zero;
        collide.enabled = false;
        projectileHead.SetActive(false);
        projectileHit.SetActive(true);
        yield return hitParticleTime;
        projectileHit.SetActive(false);
        projectileParent.SetActive(false);
    }

    public virtual void ShootProjectile(int _dir)
    {
        rb.velocity = _dir * speed * Vector2.right;
    }

    private void OnEnable()
    {
        projectileHead.SetActive(true);
        collide.enabled = true;
    }
}