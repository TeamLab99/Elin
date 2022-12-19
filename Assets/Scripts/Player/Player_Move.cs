using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rigid;
    
    float moveDir;
    bool isRight;
    

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Walk();
    }

    private void Walk()
    {
        moveDir = Input.GetAxisRaw("Horizotal");
    }
}
