using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformObjectType
{
    NPC,
    ItemBox
}

abstract public class InteractObject : MonoBehaviour
{
    [SerializeField]PlatformObjectType platformObjectType;
    protected abstract void OnTriggerEnter2D(Collider2D collision);
}
