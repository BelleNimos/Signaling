using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinPicker : MonoBehaviour
{
    [SerializeField] private TMP_Text _coinsText;

    private int _coinsCount = 0;

    private const string _coinTag = "Coin";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == _coinTag)
        {
            _coinsCount++;
            _coinsText.text = _coinsCount.ToString();
            Destroy(collision.gameObject);
        }
    }
}
