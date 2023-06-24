using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMonster : MonoBehaviour
{
    [SerializeField] protected int hp;
    [SerializeField] protected int speed;
    [SerializeField] protected int lookDir;
    [SerializeField] protected Vector2 recognitionRange;
    [SerializeField] protected Transform recognitionPos;
    [SerializeField] protected LayerMask playerLayer;

    protected Animator anim;
    protected bool findPlayer = false;
    
    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
    }


}
