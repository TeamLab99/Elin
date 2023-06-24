public enum EUILayer
{
    Base,
    UI,
    Setting
};

public enum EScene
{
    Unknown,
    DeckCreator
}

public enum ESound
{
    Bgm,
    Effect,
    MaxCount,
}

public enum EUIEvent
{
    Click,
    Drag,

}
public enum EMouseEvent
{
    Press,
    Click,
}

public enum ECameraMode
{
    QuarterView
}

public enum ESorting
{
    Default=0,
    None=1,
    Fire=2,
    Water=3,
    Wind=4,
    Earth=5
}

public enum EItemType
{
    Consumer,
    Equipment,
    Quest,
    Etc
}

public enum EGems
{
    none,
    FireGem,
    WaterGem,
    WindGem,
    EarthGem
}

public enum ESeperateDirection
{
    Left = 0,
    Right=1,
    None = 2
}

public enum EProjectileType
{
    None = 0,
    Water = 1,
    Fire =2
}

public enum EPlayerParticle
{
    Move=0,
    Jump=1,
    Fall=2
}

public enum EUiAnimation
{
    Gauge,
    Cost,
}