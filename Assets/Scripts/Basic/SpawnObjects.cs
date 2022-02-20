using System.Collections;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    [SerializeField] private GameObject _template;
    [SerializeField] private Transform[] _points;
    [SerializeField] private float _coolDown;

    private void Start()
    {
        StartCoroutine(Create());
    }

    private IEnumerator Create()
    {
        for (int i = 0; i < _points.Length; i++)
        {
            Instantiate(_template, _points[i].position, Quaternion.identity);

            yield return new WaitForSeconds(_coolDown);
        }
    }
}