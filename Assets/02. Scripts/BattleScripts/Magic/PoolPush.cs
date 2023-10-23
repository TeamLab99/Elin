using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolPush : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(Push());
    }

    IEnumerator Push()
    {
        yield return new WaitForSeconds(3f);
        Managers.Pool.Push(GetComponent<Poolable>());
    }
}
