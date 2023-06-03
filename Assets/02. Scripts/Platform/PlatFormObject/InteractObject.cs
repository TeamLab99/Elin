using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public enum InteractType
{
    NPC,
    Merchant,
    Switch,
    ItemBox
}

abstract public class InteractObject : MonoBehaviour
{
    [SerializeField] InteractType platformObjectType;
    protected abstract void OnTriggerEnter2D(Collider2D collision);
}
