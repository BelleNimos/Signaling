using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class AlarmTrigger : MonoBehaviour
{
    private Animator _animator;
    private AudioSource _audioSource;
    private float _currentVolume;

    private readonly float _minVolume = 0.01f;
    private readonly float _maxVolume = 1f;
    private readonly string _doorOpen = "Open";

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _currentVolume = _audioSource.volume;
    }

    private void Update()
    {
        if (_audioSource.isPlaying)
        {
            if (_currentVolume <= _maxVolume && _animator.GetBool(_doorOpen))
            {
                _currentVolume += _minVolume + _minVolume * _minVolume;
                _audioSource.volume = _currentVolume;
            }
            else if (_audioSource.volume >= _minVolume && _animator.GetBool(_doorOpen) == false)
            {
                _currentVolume -= _minVolume + _minVolume * _minVolume;
                _audioSource.volume = _currentVolume;
            }
            else if (_audioSource.volume <= _minVolume)
            {
                _audioSource.Stop();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PhysicsMovement>(out PhysicsMovement physicsMovement))
        {
            _animator.SetBool(_doorOpen, true);
            _audioSource.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _animator.SetBool(_doorOpen, false);
    }
}
