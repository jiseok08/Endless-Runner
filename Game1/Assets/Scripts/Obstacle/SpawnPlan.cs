public class SpawnPlan
{
    private readonly ObstacleType?[] lanes = 
        new ObstacleType?[RoadLineInfo.RoadCount];

    public void AddSpawnData(RoadLine roadLine, ObstacleType type)
    {
        lanes[RoadLineInfo.ToIndex(roadLine)] = type;
    }

    public ObstacleType? GetSpawnData(RoadLine roadLine)
    {
        return lanes[RoadLineInfo.ToIndex(roadLine)];
    }
}   
