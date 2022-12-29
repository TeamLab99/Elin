using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Move : MonoBehaviour
{
    Rigidbody2D rb;
    public int nextMove;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        Think();
    }
    private void FixedUpdate()
    {
        Vector2 frontVec = new Vector2(rb.position.x + nextMove, rb.velocity.y);
        rb.velocity = new Vector2(nextMove, rb.velocity.y);
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayHit.collider != null)
            nextMove *= -1;
    }

   void Think()
    {
        nextMove = Random.Range(-1, 2);
        Invoke("Think", 5f);
    }
   
}
