using System.Collections;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
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
            Instantiate(_enemy, _points[i].position, Quaternion.identity);

            yield return waitForSeconds;
        }
    }
}