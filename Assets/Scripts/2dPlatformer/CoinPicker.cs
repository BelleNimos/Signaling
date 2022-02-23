using UnityEngine;
using TMPro;

public class CoinPicker : MonoBehaviour
{
    [SerializeField] private TMP_Text _coinsText;

    private int _coinsCount = 0;

    private const string CoinTag = "Coin";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Coin>(out Coin coin))
        {
            Destroy(collision.gameObject);
            _coinsCount++;
            _coinsText.text = _coinsCount.ToString();
        }
    }
}