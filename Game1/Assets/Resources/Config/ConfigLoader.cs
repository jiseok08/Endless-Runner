using UnityEngine;

public static class ConfigLoader
{
    private const string ConfigPath = "Config/balance_config";

    public static GameConfig Load()
    {
        TextAsset json = Resources.Load<TextAsset>(ConfigPath);
        if (json == null)
        {
            Debug.LogError($"Config 파일을 찾지 못함: Resources/{ConfigPath}");
            return null;
        }

        GameConfig config = JsonUtility.FromJson<GameConfig>(json.text);
        if (config == null)
        {
            Debug.LogError($"Config JSON 파싱 실패: {ConfigPath}");
            return null;
        }

        return config;
    }
}