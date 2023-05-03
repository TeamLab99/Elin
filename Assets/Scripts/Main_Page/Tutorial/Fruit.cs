using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public GameObject textBox;
    public GameObject destroyGround;
    public Camera_Follow cameraFollow;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                Debug.Log("실행중");
                destroyGround.SetActive(false);
                rb.gravityScale = 1f;
            }
            textBox.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            textBox.SetActive(false);
        }
    }
}
