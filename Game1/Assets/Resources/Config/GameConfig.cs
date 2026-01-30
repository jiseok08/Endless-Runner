using System;

[Serializable]
public class GameConfig
{
    public RunnerConfig Runner;
    public SpeedManagerConfig SpeedManager;
    public ObstacleManagerConfig ObstacleManager;
    public BonusManagerConfig BonusManager;
}

[Serializable] public class RunnerConfig { public float jumpCooldown, positionX, jumpPower; }
[Serializable] public class SpeedManagerConfig { public float startSpeed, limitSpeed, increaseSpeed, increaseTime; }
[Serializable] public class ObstacleManagerConfig { public float startCycle, minCycle, cycleDecrease; public int createCount, obstacles_Capacity, longObstacles_Capacity, tripleProb, standardSec, longObstacleIndex; public string longObstacleName; public string[] obstacleNames; }
[Serializable] public class BonusManagerConfig { public int stdScore, maxCombo, startComboTime, textHoldingTime; }