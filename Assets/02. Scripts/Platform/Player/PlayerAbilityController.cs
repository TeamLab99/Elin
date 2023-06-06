using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilityController : MonoBehaviour
{
    SpriteRenderer spr;
    AbilityUI abilityUI;
    public Slider slider;
    public Image image;
    public Sprite sprite;

    public GameObject leftSeperationPlayer;
    public GameObject rightSeperationPlayer;
    public GameObject projectilePrefab;  // 발사체 프리팹

    private int currentState = 0;
    private int playerDir = 1;
    private bool isRight = true;
    private bool isSeparated = false;  // 플레이어가 분리되었는지 여부
    private float increaseSpeed= 1f;  // 흡수 시간
    private ESeperateDirection seperateDirection = ESeperateDirection.None;
    ESorting element = ESorting.None;
    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
        abilityUI = FindObjectOfType<AbilityUI>(); 
    }

    private void Update()
    {
        KeyInput();
        CheckDirection();
        if (currentState==1)
            HandleSeparation();
        else if (currentState==2)
            HandleAbsorption();
        else if(currentState==3)
            HandleProjectile();
    }

    void CheckDirection()
    {
        if (spr.flipX == true)
        {
            playerDir = -1;
            isRight = false;
        }
        else
        {
            playerDir = 1;
            isRight = true;
        }
    }

    private void KeyInput()
    {

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (currentState <= 2)
                currentState += 1;
            else
                currentState = 1;
            abilityUI.ShowAbilityUI(currentState);
        }
    }

    private void HandleSeparation()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isSeparated)
            SeperatePlayer();
    }

    private void SeperatePlayer()
    {
        if (isRight)
        {
            rightSeperationPlayer.SetActive(true);
            rightSeperationPlayer.transform.position = transform.position;
            seperateDirection = ESeperateDirection.Right;
        }
        else
        {
            leftSeperationPlayer.SetActive(true);
            leftSeperationPlayer.transform.position = transform.position;
            seperateDirection = ESeperateDirection.Left;
        }
        isSeparated = true;
        Invoke("DestroySeperationPlayer", 15f);
    }

    private void DestroySeperationPlayer()
    {
        if (seperateDirection== ESeperateDirection.Right)
            rightSeperationPlayer.SetActive(false);
        if (seperateDirection == ESeperateDirection.Left)
            leftSeperationPlayer.SetActive(false);
        isSeparated = false;
        seperateDirection = ESeperateDirection.None;
    }

    private void HandleAbsorption()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            slider.value += increaseSpeed * Time.deltaTime;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 3f);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Fire"))
                {
                    element = ESorting.Fire;
                    image.sprite = sprite;
                }
            }
            if (slider.value == slider.maxValue)
                Debug.Log("게이지 충전완료");
        }
    }

    private void HandleProjectile()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody2D>().velocity = transform.right * 10f * playerDir;  // 발사체 방향은 플레이어가 바라보는 방향으로 설정
            switch (seperateDirection)
            {
                case ESeperateDirection.Right:
                    GameObject rightProjectile = Instantiate(projectilePrefab, rightSeperationPlayer.transform.position, Quaternion.identity);
                    rightProjectile.GetComponent<Rigidbody2D>().velocity = transform.right * 10f;  // 발사체 방향은 플레이어가 바라보는 방향으로 설정
                    DestroySeperationPlayer();
                    CancelInvoke("DestroySeperationPlayer");
                    break;
                case ESeperateDirection.Left:
                    GameObject leftProjectile = Instantiate(projectilePrefab, leftSeperationPlayer.transform.position, Quaternion.identity);
                    leftProjectile.GetComponent<Rigidbody2D>().velocity = transform.right * -10f;  // 발사체 방향은 플레이어가 바라보는 방향으로 설정
                    DestroySeperationPlayer();
                    CancelInvoke("DestroySeperationPlayer");
                    break;
            }
            slider.value = 0;
        }
    }
}
