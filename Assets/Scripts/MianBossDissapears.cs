using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MianBossDissapears : MonoBehaviour
{
    [SerializeField] private AudioClip _laughAudioClip;
    private Animator _animator;

    private const string _dissapearAnimation = "Dissapear";

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void MakeLaugh()
    {
        AudioSource.PlayClipAtPoint(_laughAudioClip, Camera.main.transform.position, 0.3F);
        StartDissapear();
    }

    private void StartDissapear()
    {
        _animator.SetTrigger(_dissapearAnimation);
    }
}
