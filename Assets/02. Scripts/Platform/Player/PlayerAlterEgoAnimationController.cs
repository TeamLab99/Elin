using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAlterEgoAnimationController : MonoBehaviour
{
    Animator anim;
    [SerializeField] ParticleSystem destroyParticle;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void ShootProjectile()
    {
        anim.SetTrigger("launch");
        Invoke("DestroyParticle", 0.2f);
        Invoke("DestroyAlterEgo", 0.5f);
    }

    public void DestroyParticle()
    {
        destroyParticle.Play();
    }

    public void DestroyAlterEgo()
    {
        gameObject.SetActive(false);
    }
}
