using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogueFunc : DialogueViewBase
{
    DialogueRunner runner;

    void Awake()
    {
        runner = GetComponentInParent<DialogueRunner>();
        runner.AddCommandHandler("Test", Test);
    }

    public void Test()
    {
        Debug.Log("테스트중입니다.!");
    }
}
