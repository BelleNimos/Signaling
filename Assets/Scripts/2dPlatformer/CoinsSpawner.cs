using System.Collections;
using UnityEngine;

public class CoinsSpawner : MonoBehaviour
{
    [SerializeField] private Coin _coin;
    [SerializeField] private Transform[] _points;
    [SerializeField] private float _coolDown;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_coolDown);

        for (int i = 0; i < _points.Length; i++)
        {
            Instantiate(_coin, _points[i].position, Quaternion.identity);

            yield return waitForSeconds;
        }
    }
}