using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleakForestScene : BaseScene
{

    void Start()
    {
        SceneType = EScene.BleakForest;
        DialogueManager.instance.StartDialogue("BleakForest0");
    }

    public override void Clear() { }
}
