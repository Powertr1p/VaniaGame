using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MianBossDissapears : MonoBehaviour
{
    [SerializeField] private AudioClip _laughAudioClip;
    private AudioSource _audioSource;
    private Animator _animator;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
    }

    public void MakeLaugh()
    {
        AudioSource.PlayClipAtPoint(_laughAudioClip, Camera.main.transform.position, 0.3F);
        StartDissapear();
    }

    private void StartDissapear()
    {
        _animator.SetTrigger("New Trigger");
    }
}
