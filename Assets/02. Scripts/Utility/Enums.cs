public enum EUILayer
{
    Base,
    UI,
    Setting
};

public enum EScene
{
    LogIn,
    BleakForest,
    GreenForest_one,
    GreenForest_two,
    GreenForest_three,
    HideOut,
    Unknown,
    DeckCreator,

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

public enum EnhanceStat
{
    Health=0,
    Attack=1,
    MaxCost=2,
    CostRecovery=3,
    None=4
}
public enum EUiAnimation
{
    Gauge,
    Cost,
    Buff
}

public enum EMagicType
{
    RandomNormal,
    RandomNightmare,
    HeadButt,
    Heal,
    Rolling,
    Defense,
    Bubble,
    WaterWeed,
    WaterStream,
    WaterCannon,
    FoxRain,
    Spark,
    WindCutter,
    EarthQuake
}