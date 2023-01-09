using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//다양한 enum들을 선언한 파일
public class Define
{
    public enum Scene
    {
        Unknown,
        DeckCreator
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum UIEvent
    {
        Click,
        Drag,

    }
    public enum MouseEvent
    {
        Press,
        Click,
    }

    public enum CameraMode
    {
        QuarterView
    }

    public enum Sorting
    {
        Default,
        Fire,
        Water,
        Wind,
        Earth
    }
}
