using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public enum PlatformObjectType
{
    NPC,
    Puzzle,
    ItemBox
}

abstract public class InteractObject : MonoBehaviour
{
    [SerializeField]PlatformObjectType platformObjectType;
    protected GameObject dialogueObject;
    protected DialogueRunner dialogueRunner;
    private void Awake()
    {
        dialogueObject = GameObject.FindGameObjectWithTag("DL");
        dialogueRunner = dialogueObject.GetComponent<DialogueRunner>();
    }
    protected abstract void OnTriggerEnter2D(Collider2D collision);
}
