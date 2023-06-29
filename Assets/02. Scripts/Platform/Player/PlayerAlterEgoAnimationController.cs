using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAlterEgoAnimationController : MonoBehaviour
{
    Animator anim;
    [SerializeField] ParticleSystem[] destroyParticle;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void ShootProjectile()
    {
        anim.SetTrigger("Split");
    }

    public void DestroyAlterEgoEffect()
    {
        destroyParticle[0].Play();
        destroyParticle[1].Play();
    }

    public void DestroyAlterEgo()
    {
        gameObject.SetActive(false);
    }
}
