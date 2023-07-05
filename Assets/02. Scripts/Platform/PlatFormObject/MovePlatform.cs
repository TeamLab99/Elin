using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    public Transform startPoint; // 플랫폼의 시작 위치
    public Transform endPoint; // 플랫폼의 끝 위치
    public float speed = 3f; // 플랫폼의 이동 속도

    private Vector3 currentTarget; // 현재 목표 위치

    void Start()
    {
        // 시작 위치를 현재 위치로 설정
        transform.position = startPoint.position;
        currentTarget = endPoint.position;
    }

    void Update()
    {
        // 플랫폼을 목표 위치로 이동
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);

        // 플랫폼이 목표 위치에 도달했을 때
        if (transform.position == currentTarget)
        {
            // 현재 목표 위치를 반대로 변경
            if (currentTarget == startPoint.position)
                currentTarget = endPoint.position;
            else
                currentTarget = startPoint.position;
        }
    }
}
