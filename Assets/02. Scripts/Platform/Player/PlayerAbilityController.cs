using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilityController : PlayerController
{
    AbilityUI abilityUI;
    SpriteRenderer spr;

    public GameObject leftSeperationPlayer;
    public GameObject rightSeperationPlayer;
    public GameObject[] projectilePrefab;

    private int currentAbilityState = 0; 
    private int playerDir = 1; 
    private bool isSeparated = false;  
    private bool coolDownAbsorption = true; // false이면 쿨타임중
    private bool coolDownProjectile = true; // false이면 쿨타임중
    private ESorting element = ESorting.None;
    private ESeperateDirection seperateDirection = ESeperateDirection.None; 

    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
        abilityUI = FindObjectOfType<AbilityUI>();
    }

    private void Update()
    {
        KeyInput();
        CheckDirection();
        switch (currentAbilityState)
        {
            case 0:
                HandleSeparation();
                break;
            case 1:
                HandleAbsorption();
                break;
            case 2:
                HandleProjectile();
                break;
        } 
    }
    private void KeyInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (currentAbilityState < 2)
                currentAbilityState += 1;
            else
                currentAbilityState = 0;
            abilityUI.ChangeAbilityType(currentAbilityState);
        }
    }

    void CheckDirection()
    {
        if (spr.flipX == true)
            playerDir = -1;
        else
            playerDir = 1;
    }

    // 분리
    private void HandleSeparation()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isSeparated)
        {
            if (playerDir == 1)
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

    // 흡수
    private void HandleAbsorption()
    {
        if (Input.GetKey(KeyCode.Z) && coolDownAbsorption)
        {
            coolDownAbsorption = false;
            Invoke("CoolDownAbsorption", 2f);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 3f);
            foreach (Collider2D collider in colliders) // 가장 가까운 속성을 따라감
            {
                if (collider.CompareTag("Fire"))
                {
                    element = ESorting.Fire;
                    abilityUI.ChangeElementType(ESorting.Fire);
                    return;
                }
            }
        }
    }

    private void CoolDownAbsorption()
    {
        coolDownAbsorption = true;
    }

    // 발사
    private void HandleProjectile()
    {
        if (Input.GetKeyDown(KeyCode.Z) && coolDownProjectile)
        {
            coolDownProjectile = false;
            abilityUI.SetLoadingEffect(true);
            Invoke("CoolDownProjectile", 2f);
            GameObject projectile = Instantiate(projectilePrefab[(int)element], transform.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody2D>().velocity = transform.right * 10f * playerDir;  // 발사체 방향은 플레이어가 바라보는 방향으로 설정
            switch (seperateDirection)
            {
                case ESeperateDirection.Right:
                    GameObject rightProjectile = Instantiate(projectilePrefab[(int)element], rightSeperationPlayer.transform.position, Quaternion.identity);
                    rightProjectile.GetComponent<Rigidbody2D>().velocity = transform.right * 10f;  // 발사체 방향은 플레이어가 바라보는 방향으로 설정
                    DestroySeperationPlayer();
                    CancelInvoke("DestroySeperationPlayer");
                    break;
                case ESeperateDirection.Left:
                    GameObject leftProjectile = Instantiate(projectilePrefab[(int)element], leftSeperationPlayer.transform.position, Quaternion.identity);
                    leftProjectile.GetComponent<Rigidbody2D>().velocity = transform.right * -10f;  // 발사체 방향은 플레이어가 바라보는 방향으로 설정
                    DestroySeperationPlayer();
                    CancelInvoke("DestroySeperationPlayer");
                    break;
            }
            abilityUI.ChangeElementType(ESorting.None);
        }
    }
    private void CoolDownProjectile()
    {
        abilityUI.SetLoadingEffect(false);
        coolDownProjectile = true;
    }
}
