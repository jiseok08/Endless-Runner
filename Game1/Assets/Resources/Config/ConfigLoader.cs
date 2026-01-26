using UnityEngine;

public static class ConfigLoader
{
    public static GameConfig Config { get; private set; }

    public static void Load()
    {
        if (Config != null) return;

        TextAsset json = Resources.Load<TextAsset>("Config/balance_config");
        if (json == null)
        {
            Debug.LogError("Resources/Config/balance_config.json 못 찾음");
            return;
        }

        Config = JsonUtility.FromJson<GameConfig>(json.text);
        if (Config == null) Debug.LogError("JSON 파싱 실패");
    }
}