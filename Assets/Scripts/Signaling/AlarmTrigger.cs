using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class AlarmTrigger : MonoBehaviour
{
    private AudioSource _audioSource;
    private Coroutine _volumeUp;
    private Coroutine _volumeDown;
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
        WaitForSeconds waitForSeconds = new WaitForSeconds(MinVolume);

        for (float i = MaxVolume; i > _currentVolume;)
        {
            _currentVolume += MinVolume;
            _audioSource.volume = _currentVolume;

            yield return waitForSeconds;
        }
    }

    private IEnumerator Reduce()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(MinVolume);

        for (float i = MinVolume; i < _currentVolume;)
        {
            _currentVolume -= MinVolume;
            _audioSource.volume = _currentVolume;

            yield return waitForSeconds;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PhysicsMovement>(out PhysicsMovement physicsMovement))
            _audioSource.Play();

        _volumeUp = StartCoroutine(Increase());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _volumeDown = StartCoroutine(Reduce());

        if (_audioSource.volume <= MinVolume)
            _audioSource.Stop();
    }
}
