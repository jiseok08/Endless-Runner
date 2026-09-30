using UnityEngine;

public class ConfigManager : Singleton<ConfigManager>
{
    public GameConfig Config { get; private set; }

    protected override void Initialize()
    {
        Config = ConfigLoader.Load();

        if (Config == null)
        {
            Debug.LogError("GameConfig 로드 실패");
            return;
        }
    }
}

