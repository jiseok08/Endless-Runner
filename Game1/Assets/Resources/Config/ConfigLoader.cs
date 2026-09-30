using UnityEngine;

public static class ConfigLoader
{
    private const string BalanceConfig = "Config/balance_config";

    public static GameConfig Load()
    {
        TextAsset json = Resources.Load<TextAsset>(BalanceConfig);

        if (json == null)
        {
            Debug.LogError("JSON 파일을 불러오지 못함");
            return null;
        }

        GameConfig config = JsonUtility.FromJson<GameConfig>(json.text);

        if (config == null)
        {
            Debug.LogError("GameConfig 생성 실패");
            return null;
        }

        return config;
    }
}




