using UnityEngine;

[CreateAssetMenu(fileName = "SpawnIntervalEffect", menuName = "Item/Effect/Spawn")]
public class SpawnIntervalDecreaseEffect : ItemEffect
{
    [SerializeField] private float spawnInterval;

    public override void Apply()
    {
        ISpawnIntervalReceiver receiver = ItemManager.Instance.Registry.Get<ISpawnIntervalReceiver>();

        receiver.DecreaseSpawnInterval(spawnInterval);
    }
}
