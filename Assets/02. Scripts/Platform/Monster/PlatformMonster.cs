using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMonster : MonoBehaviour
{
    public float recognitionRange;
    [SerializeField] protected int hp;
    [SerializeField] protected int speed;
    [SerializeField] protected int lookDir;
    [SerializeField] protected Transform recognitionPos;
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected ParticleSystem findParticle;

    protected RaycastHit2D playerHit;
    protected Animator anim;
    protected bool findPlayer = false;
    
    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
    }

}
