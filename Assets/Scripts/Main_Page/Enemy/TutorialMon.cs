using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMon : MonoBehaviour
{
    [SerializeField]
    private GameObject isFindPlayer;
    public bool towardPlayer = false;
    private Rigidbody2D rb;
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Mathf.Abs(rb.velocity.x) > 0.1f)
            anim.SetBool("Run", true);
        else
            anim.SetBool("Run", false);
    }
    private void FixedUpdate()
    {
        if (towardPlayer)
        {
            isFindPlayer.SetActive(true);
            rb.velocity = Vector2.right * (-1)*3;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Invoke("FindPlayer", 3f);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("전투 페이지 전환");
        }
    }

    private void FindPlayer()
    {
        towardPlayer = true;
    }
}
