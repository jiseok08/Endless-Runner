using System;
using UnityEngine;

[Serializable]
public class GameConfig
{
    public RunnerConfig runner;
    public SpeedManagerConfig speedManager;
    public ScoreManagerConfig scoreManager;
    public ObstacleSpawnConfig obstacleSpawn;
    public ObstacleManagerConfig obstacleManager;
    public ObstaclePoolConfig obstaclePool;
    public BonusManagerConfig bonusManager;
    public CoinRouteRunnerConfig coinRouteRunner;
    public CoinSpawnerConfig coinSpawner;
    public CoinPoolConfig coinPool;
    public CoinConfig coin;
}

[Serializable]
public class RunnerConfig
{
    public float positionX;
    public float jumpPower;
    public float laneChangeTime;
}

[Serializable]
public class SpeedManagerConfig
{
    public float startSpeed;
    public float limitSpeed;
    public float increaseSpeed;
    public float increaseTime;
}

[Serializable]
public class ScoreManagerConfig
{
    public int baseScore;
    public float scoreInterval;
}
[Serializable]
public class ObstacleSpawnConfig
{
    public Vector3[] spawnPoints; 
}

[Serializable]
public class ObstacleManagerConfig
{
    public float startCycle;
    public float startMinCycle;
    public float limitCycle;
    public float cycleDecrease;

    public int standardStageCount;
    public int lookAheadCount;

    public ObstacleStageConfig[] stages;
}

[Serializable]
public class ObstacleStageConfig
{
    public int stage;
    public PatternWeightConfig[] patterns;
}

[Serializable]
public class PatternWeightConfig
{
    public string pattern;
    public int weight;
}

[Serializable]
public class ObstaclePoolConfig
{
    public int createCount;
    public string longObstacleName;
    public string[] obstacleNames;
}

[Serializable]
public class BonusManagerConfig
{
    public int standardScore;
    public int maxCombo;
    public int startComboTime;
    public int textHoldingTime;
}

[Serializable]
public class CoinRouteRunnerConfig
{
    public float jumpOffset;
    public float passMargin;
}

[Serializable]
public class CoinSpawnerConfig
{
    public float spawnInterval;
    public float heightOffset;
}

[Serializable]
public class CoinPoolConfig
{
    public int createCount;
    public string coinName;
}

[Serializable]
public class CoinConfig
{
    public int coinValue;
}