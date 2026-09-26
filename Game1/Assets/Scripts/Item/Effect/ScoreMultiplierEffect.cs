using UnityEngine;

[CreateAssetMenu(fileName = "ScoreMultiplier", menuName = "Item/Effect/Score")]
public class ScoreMultiplierEffect : ItemEffect
{
    [SerializeField] private float scoreMultiplier;

    public override void Apply()
    {
        IScoreMultiplierReceiver receiver = ItemManager.Instance.Registry.Get<IScoreMultiplierReceiver>();

        receiver.SetScoreMultiplier(scoreMultiplier);
    }
}
