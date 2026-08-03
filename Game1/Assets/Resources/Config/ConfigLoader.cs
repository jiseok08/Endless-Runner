using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public static class ConfigLoader
{
    private const string ConfigPath = "Config/balance_config";

    public static GameConfig Load()
    {
        TextAsset json = Resources.Load<TextAsset>(ConfigPath);
        if (json == null)
        {
            Debug.LogError("json == null (Config/balance_config)");
            return null;
        }

        GameConfig config = JsonUtility.FromJson<GameConfig>(json.text);
        if (config == null)
        {
            Debug.LogError("config == null(Config / balance_config)");
            return null;
        }

        return config;
    }
}