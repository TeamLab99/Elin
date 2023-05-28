using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityController : MonoBehaviour
{
    public GameObject separationPlayerPrefab;  // 분열 플레이어 프리팹
    public GameObject projectilePrefab;  // 발사체 프리팹
    public float absorptionRadius = 5f;  // 흡수 반경
    private int currentState = 0;

    private bool isSeparated = false;  // 플레이어가 분리되었는지 여부
    private bool isAbsorbing = false;  // 플레이어가 흡수 중인지 여부
    private float absorptionTime = 0f;  // 흡수 시간

    private void Update()
    {
        KeyInput();

        if(currentState==1)
            HandleSeparation();
        else if (currentState==2)
            HandleAbsorption();
        else if(currentState==3)
            HandleProjectile();
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
            Invoke("CreateSeparationPlayer", 1f);
    }

    private void CreateSeparationPlayer()
    {
        separationPlayerPrefab.SetActive(true);
        Vector3 direction = transform.localScale.x > 0 ? Vector3.right : Vector3.left;
        separationPlayerPrefab.transform.right = direction;
        isSeparated = true;
        Invoke("DestroySeperationPlayer", 15f);
    }

    private void DestroySeperationPlayer()
    {
        isSeparated = false;
        separationPlayerPrefab.SetActive(false);
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
        if (Input.GetKeyDown(KeyCode.Z) && isSeparated)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody2D>().velocity = transform.right * 10f;  // 발사체 방향은 플레이어가 바라보는 방향으로 설정
        }
    }
}
