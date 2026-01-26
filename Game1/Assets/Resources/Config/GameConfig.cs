using System;

[Serializable]
public class GameConfig
{
    public RunnerConfig runner;
    public SpeedManagerConfig SpeedManager;
    public ObstacleManagerConfig ObstacleManager;
    public BonusZoneConfig BonusZone;
}

[Serializable] public class RunnerConfig { public float jumpCooldown, positionX, jumpPower; }
[Serializable] public class SpeedManagerConfig { public float startSpeed, limitSpeed, increaseSpeed, increaseTime; }
[Serializable] public class ObstacleManagerConfig { public float startCycle, minCycle, cycleDecrease; public int createCount, tripleProb, standardSec; }
[Serializable] public class BonusZoneConfig { public int bonusScore; }