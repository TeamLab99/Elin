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

    protected Rigidbody2D rb;
    protected WaitForSeconds hitParticleTime= new WaitForSeconds(0.5f);

    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected IEnumerator CollisionEffect()
    {
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
    }
}