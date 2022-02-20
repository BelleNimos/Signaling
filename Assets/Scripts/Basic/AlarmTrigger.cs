using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AlarmTrigger : MonoBehaviour
{
    private AudioSource _audioSource;
    private float _currentVolume;

    private bool _isTrigger;

    private const float _minVolume = 0.01f;
    private const float _maxVolume = 1f;

    private void OnEnable()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _currentVolume = _audioSource.volume;
    }

    private void Update()
    {
        if (_isTrigger)
        {
            if (_currentVolume <= _maxVolume)
            {
                _currentVolume += _minVolume + _minVolume * _minVolume;
                _audioSource.volume = _currentVolume;
            }
        }
        else
        {
            if (_currentVolume > _minVolume)
            {
                _currentVolume -= _minVolume + _minVolume * _minVolume;
                _audioSource.volume = _currentVolume;
            }
            else
            {
                _audioSource.Stop();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PhysicsMovement>(out PhysicsMovement physicsMovement))
        {
            _isTrigger = true;
            _audioSource.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _isTrigger = false;
    }
}
