using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityController : MonoBehaviour
{
    SpriteRenderer spr;

    public GameObject leftSeperationPlayer;
    public GameObject rightSeperationPlayer;
    public GameObject projectilePrefab;  // 발사체 프리팹

    public float absorptionRadius = 5f;  // 흡수 반경
    private int currentState = 0;

    private bool isRight = true;
    private bool isSeparated = false;  // 플레이어가 분리되었는지 여부
    private bool isAbsorbing = false;  // 플레이어가 흡수 중인지 여부
    private float absorptionTime = 0f;  // 흡수 시간
    private SeperateDirection seperateDirection = SeperateDirection.None;
    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
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
            isRight = true;
        else
            isRight = false;
    }

    private void KeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            currentState = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            currentState = 2;
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            currentState = 3;
    }

    private void HandleSeparation()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isSeparated && !isAbsorbing)
            SeperatePlayer();
    }

    private void SeperatePlayer()
    {
        if (isRight)
        {
            rightSeperationPlayer.SetActive(true);
            seperateDirection = SeperateDirection.Right;
        }
        else
        {
            leftSeperationPlayer.SetActive(true);
            seperateDirection = SeperateDirection.Left;
        }
        isSeparated = true;
        Invoke("DestroySeperationPlayer", 15f);
    }

    private void DestroySeperationPlayer()
    {
        if (seperateDirection== SeperateDirection.Right)
            rightSeperationPlayer.SetActive(false);
        if (seperateDirection == SeperateDirection.Left)
            leftSeperationPlayer.SetActive(false);
        isSeparated = false;
        seperateDirection = SeperateDirection.None;
    }

    private void HandleAbsorption()
    {
        // 흡수 시작
        if (Input.GetKeyDown(KeyCode.Z) && !isAbsorbing)
            isAbsorbing = true;
        // 흡수 종료
        if (Input.GetKeyUp(KeyCode.Z) && isAbsorbing)
        {
            isAbsorbing = false;
            absorptionTime = 0f;
        }
            

        if (isAbsorbing)
        {
            absorptionTime += Time.deltaTime;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, absorptionRadius);
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject == gameObject)
                    continue;

                if (collider.gameObject.CompareTag("Absorbable"))
                {
                    // 흡수할 오브젝트를 끌어당기는 로직 추가
                    Vector3 direction = transform.position - collider.transform.position;
                    float distance = direction.magnitude;
                    float pullForce = Mathf.Lerp(10f, 1f, absorptionTime);  // 흡수 시간에 따라 힘을 조절 (필요에 따라 조정)
                    collider.GetComponent<Rigidbody2D>().AddForce(direction.normalized * pullForce);

                    // 플레이어와 오브젝트가 충돌하면 흡수가 완료되도록 처리
                    if (distance < 0.5f)
                    {
                        Destroy(collider.gameObject);
                        isAbsorbing = false;
                    }
                }
            }
        }
    }

    private void HandleProjectile()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody2D>().velocity = transform.right * 10f;  // 발사체 방향은 플레이어가 바라보는 방향으로 설정
            switch (seperateDirection)
            {
                case SeperateDirection.Right:
                    GameObject rightProjectile = Instantiate(projectilePrefab, rightSeperationPlayer.transform.position, Quaternion.identity);
                    rightProjectile.GetComponent<Rigidbody2D>().velocity = transform.right * 10f;  // 발사체 방향은 플레이어가 바라보는 방향으로 설정
                    break;
                case SeperateDirection.Left:
                    GameObject leftProjectile = Instantiate(projectilePrefab, leftSeperationPlayer.transform.position, Quaternion.identity);
                    leftProjectile.GetComponent<Rigidbody2D>().velocity = transform.right * (-10f);  // 발사체 방향은 플레이어가 바라보는 방향으로 설정
                    break;
            }
        }
    }
}
