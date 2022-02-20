using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AlarmTrigger : MonoBehaviour
{
    private AudioSource _audioSource;
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

            yield return new WaitForSeconds(MinVolume);
        }
    }

    private IEnumerator Reduce()
    {
        for (float i = MinVolume; i < _currentVolume;)
        {
            _currentVolume -= MinVolume;
            _audioSource.volume = _currentVolume;

            yield return new WaitForSeconds(MinVolume);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PhysicsMovement>(out PhysicsMovement physicsMovement))
        {
            _audioSource.Play();
            StartCoroutine(Increase());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        StartCoroutine(Reduce());

        if (_audioSource.volume <= MinVolume)
            _audioSource.Stop();
    }
}
