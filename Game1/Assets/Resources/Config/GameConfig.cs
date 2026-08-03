using System;

[Serializable]
public class GameConfig
{
    public RunnerConfig runner;
    public SpeedManagerConfig speedManager;
    public ObstacleManagerConfig obstacleManager;
    public BonusManagerConfig bonusManager;
}

[Serializable]
public class RunnerConfig
{
    public float positionX;      // 러너 고정 X 위치
    public float jumpPower;      // 점프 힘
}

[Serializable]
public class SpeedManagerConfig
{
    public float startSpeed;     // 시작 속도
    public float limitSpeed;     // 최대 속도
    public float increaseSpeed;  // 속도 증가량
    public float increaseTime;   // 속도 증가 간격(초)
}

[Serializable]
public class ObstacleManagerConfig
{
    public float startCycle;            // 시작 생성 주기(초)
    public float minCycle;              // 최소 생성 주기(초)
    public float cycleDecrease;         // 주기 감소량
    public int createCount;             // 한 번에 생성 개수
    public int tripleProbability;       // 3연속 생성 확률(%)
    public int standardSecond;          // 기준 시간(초)
    public int longObstacleIndex;       // 긴 장애물 인덱스
    public string longObstacleName;     // 긴 장애물 이름
    public string[] obstacleNames;      // 일반 장애물 이름 목록
}

[Serializable]
public class BonusManagerConfig
{
    public int standardScore;    // 기준 점수
    public int maxCombo;         // 최대 콤보
    public int startComboTime;   // 콤보 시작 시간(초)
    public int textHoldingTime;  // 텍스트 유지 시간(초)
}