using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AlarmTrigger : MonoBehaviour
{
    private AudioSource _audioSource;
    private Coroutine _coroutine;
    private float _currentVolume;

    private const float MinVolume = 0.01f;
    private const float MaxVolume = 1f;

    private void OnEnable()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _currentVolume = _audioSource.volume;
    }

    private IEnumerator Increase()
    {
        for (float i = MaxVolume; i > _currentVolume;)
        {
            _currentVolume += MinVolume;
            _audioSource.volume = _currentVolume;

            yield return null;
        }
    }

    private IEnumerator Reduce()
    {
        for (float i = MinVolume; i < _currentVolume;)
        {
            _currentVolume -= MinVolume;
            _audioSource.volume = _currentVolume;

            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
            _audioSource.Play();

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Increase());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Reduce());

        if (_audioSource.volume <= MinVolume)
            _audioSource.Stop();
    }
}
