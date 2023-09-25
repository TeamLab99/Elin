using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Position, Rotation, Scale 코드 간소화,
/// 접근 및 호출을 편하게 하기 위해 만든 스크립트.
/// </summary>
[System.Serializable]
public class PRS
{
    public Vector3 pos;
    public Quaternion rot;
    public Vector3 scale;

    public PRS(Vector3 pos, Quaternion rot, Vector3 scale)
    {
        this.pos = pos;
        this.rot = rot;
        this.scale = scale;
    }
}

public class Utils
{
    // 회전량이 0인 상태 (0,0,0,1)
    public static Quaternion QI => Quaternion.identity;

    public static bool RandomPercent(float percentage)
    {
        float probability = percentage / 100;
        float rate = 100 - (100 * probability);
        int tmp = (int)Random.Range(0, 100);

        if (tmp <= rate - 1)
        {
            return false;
        }
        return true;
    }
}

