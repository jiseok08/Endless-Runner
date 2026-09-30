using UnityEngine;
using UnityEngine.UI;

public class CoinUIManager : MonoBehaviour
{
    [SerializeField] Text[] coinText;

    private CoinManager coinManager;

    private void Awake()
    {
        coinManager = GetComponentInParent<CoinManager>();
    }

    private void OnEnable()
    {
        coinManager.CoinChanged += TextUpdate;

        TextUpdate();
    }

    private void TextUpdate()
    {
        string value = coinManager.Coin.ToString();

        foreach (Text text in coinText)
        {
            text.text = value;
        }
    }

    private void OnDisable()
    {
        coinManager.CoinChanged -= TextUpdate;
    }
}