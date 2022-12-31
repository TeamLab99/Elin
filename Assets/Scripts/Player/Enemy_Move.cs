using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy_Move : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer spr;
    public int nextMove;
    public GameObject exclamation;

    private bool findPlayer;
    public float speed;
    public float distance;
    public Transform player;
    
    
    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        Think();
    }
    private void Update()
    {
    }
    private void FixedUpdate()
    {
        if (!findPlayer)
            rb.velocity = new Vector2(nextMove, rb.velocity.y);
        else
        {
            rb.velocity = new Vector2(0, 0);
            TowardPlayer();
            FlipDir();
        }
        // Vector2 frontVec = new Vector2(rb.position.x + nextMove, rb.velocity.y);
        //RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        //  if (rayHit.collider != null)
        //    nextMove *= -1;
    }

   void Think()
    {
        nextMove = Random.Range(-1, 2);
        Invoke("Think", 5f);
    }

    void TowardPlayer()
    {
        if (Mathf.Abs(transform.position.x - player.position.x) > distance && findPlayer)
        {
            transform.Translate(new Vector2(-1, 0) * Time.deltaTime * speed);
        }
    }
    
    void FlipDir()
    {
        if (transform.position.x - player.position.x < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            exclamation.SetActive(true);
            findPlayer = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            exclamation.SetActive(false);
            findPlayer = false;
        }
    }
}
