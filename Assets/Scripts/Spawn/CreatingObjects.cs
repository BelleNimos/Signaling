using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatingObjects : MonoBehaviour
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
        WaitForSeconds waitForSeconds = new WaitForSeconds(_coolDown);

        for (int i = 0; i < _points.Length; i++)
        {
            Instantiate(_template, _points[i].position, Quaternion.identity);

            yield return waitForSeconds;
        }
    }
}