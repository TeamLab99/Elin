using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilityController : MonoBehaviour
{
    public ParticleSystem[] playerAbsorptionParticle;
    public ParticleSystem waterElementParticle;
    public ParticleSystem splitPlayerParticle;
    AbilityUI abilityUI;
    [SerializeField] PlayerController playerController;
    [SerializeField] SpriteRenderer spr;
    [SerializeField] Animator anim;

    GameObject alterEgoPlayer;
    GameObject alterEgoProjectile;
    GameObject playerProjectile;

    private int currentAbilityState = 0; 
    private int playerDir = 1; 
    private bool isSeparated = false;  
    private bool coolDownAbsorption = true; // false이면 쿨타임중
    private bool coolDownProjectile = true; // false이면 쿨타임중
    private EProjectileType element = EProjectileType.None;
    private ESeperateDirection seperateDirection = ESeperateDirection.None; 

    private void Awake()
    {
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
                alterEgoPlayer=PlayerPoolManager.instance.GetAlterEgo((int)ESeperateDirection.Right);
                alterEgoPlayer.transform.position = transform.position;
                seperateDirection = ESeperateDirection.Right;
            }
            else
            {
                alterEgoPlayer = PlayerPoolManager.instance.GetAlterEgo((int)ESeperateDirection.Left);
                alterEgoPlayer.transform.position = transform.position;
                seperateDirection = ESeperateDirection.Left;
            }
            isSeparated = true;
            splitPlayerParticle.Play();
            anim.SetTrigger("Split");
            Invoke("DestroySeperationPlayer", 15f);
        }
    }

    private void DestroySeperationPlayer()
    {
        isSeparated = false;
        seperateDirection = ESeperateDirection.None;
    }

    // 흡수
    private void HandleAbsorption()
    {
        if (Input.GetKey(KeyCode.Z) && coolDownAbsorption)
        {
            coolDownAbsorption = false;
            playerAbsorptionParticle[0].Play();
            playerAbsorptionParticle[1].Play();
            anim.SetTrigger("Absorption");
            playerController.ControlPlayer(false);
            Invoke("CanMovePlayer", 0.5f);
            Invoke("CoolDownAbsorption", 2f);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 3f);
            foreach (Collider2D collider in colliders) // 가장 가까운 속성을 따라감
            {
                if (collider.CompareTag("WaterElement"))
                {
                    waterElementParticle.Play();
                    element = EProjectileType.Water;
                    abilityUI.ChangeElementType(EProjectileType.Water);
                    return;
                }
            }
        }
    }

    public void CanMovePlayer()
    {
        playerController.ControlPlayer(true);
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
            Invoke("CoolDownProjectile", 2f);
            playerProjectile = PlayerPoolManager.instance.GetProjectile((int)element);
            playerProjectile.transform.position = transform.position+Vector3.right*playerDir* 0.5f;
            playerProjectile.GetComponent<Projectile>().ShootProjectile(playerDir);  // 발사체 방향은 플레이어가 바라보는 방향으로 설정
            anim.SetTrigger("Split");
            switch (seperateDirection)
            {
                case ESeperateDirection.Right:
                    alterEgoProjectile = PlayerPoolManager.instance.GetProjectile((int)element);
                    alterEgoProjectile.transform.position = alterEgoPlayer.transform.position+Vector3.right* 0.5f;
                    alterEgoProjectile.GetComponent<Projectile>().ShootProjectile(1); // 발사체 방향은 플레이어가 바라보는 방향으로 설정
                    alterEgoPlayer.GetComponentInChildren<PlayerAlterEgoAnimationController>().ShootProjectile();
                    DestroySeperationPlayer();
                    CancelInvoke("DestroySeperationPlayer");
                    break;
                case ESeperateDirection.Left:
                    alterEgoProjectile = PlayerPoolManager.instance.GetProjectile((int)element);
                    alterEgoProjectile.transform.position = alterEgoPlayer.transform.position + Vector3.right*-0.5f;
                    alterEgoProjectile.GetComponent<Projectile>().ShootProjectile(-1);// 발사체 방향은 플레이어가 바라보는 방향으로 설정
                    alterEgoPlayer.GetComponentInChildren<PlayerAlterEgoAnimationController>().ShootProjectile();
                    DestroySeperationPlayer();
                    CancelInvoke("DestroySeperationPlayer");
                    break;
            }
            abilityUI.ChangeElementType(EProjectileType.None);
            element = EProjectileType.None;
        }
    }
    private void CoolDownProjectile()
    {
        coolDownProjectile = true;
    }
}
